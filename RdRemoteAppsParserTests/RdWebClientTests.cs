using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable once CheckNamespace
namespace RdRemoteAppsParser.Tests
{
    [TestClass]
    public class RdWebClientTests
    {
        private const string domain = "https://***.ru";
        private const string user = "";
        private const string pass = "";

        private readonly RdWebClient client;

        public RdWebClientTests()
        {
            //var proxy = new WebProxy
            //{
            //    Address = new Uri("http://10.0.0.0:8080"),
            //    BypassProxyOnLocal = false,
            //    UseDefaultCredentials = true
            //};

            //client = new RdWebClient(new HttpClientHandler { Proxy = proxy });
            client = new RdWebClient();
        }


        [TestMethod]
        public void ConnectTest_LogonFailed()
        {
            Assert.ThrowsExceptionAsync<Exception>(() => client.Connect(domain, user + "0", pass));
        }

        [TestMethod]
        public void ConnectTest_Successful()
        {
            client.Connect(domain, user, pass).Wait();
        }

        [TestMethod]
        public void GetAllRemoteElementsTest()
        {
            client.Connect(domain, user, pass).Wait();
            var folderModel = client.GetAllRemoteElements().Result;

            var contsinsApps = folderModel.Apps.Any();
            var allPngInRootAreValid = folderModel.Apps.All(app =>
                Regex.IsMatch(app.PngPathUrl, @"^\/(.*?).png$", RegexOptions.Compiled | RegexOptions.Singleline));
            var allRdpInRootAreValid = folderModel.Apps.All(app =>
                Regex.IsMatch(app.RdpFileUrl, @"^\/(.*?).rdp$", RegexOptions.Compiled | RegexOptions.Singleline));
            var appsPathesAreValid = folderModel.Apps.All(app =>
                Regex.IsMatch(app.Path, @"^\/\/[^\/]+$", RegexOptions.Compiled | RegexOptions.Singleline));
            var foldersPathesAreValid = folderModel.Subfolders.All(app =>
                Regex.IsMatch(app.Path, @"^\/\/[^\/]+$", RegexOptions.Compiled | RegexOptions.Singleline));

            Assert.IsTrue(contsinsApps);
            Assert.IsTrue(allPngInRootAreValid);
            Assert.IsTrue(allRdpInRootAreValid);
            Assert.IsTrue(appsPathesAreValid);
            Assert.IsTrue(foldersPathesAreValid);
        }
    }
}