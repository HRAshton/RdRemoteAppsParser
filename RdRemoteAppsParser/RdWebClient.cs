using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using RdRemoteAppsParser.Models;

namespace RdRemoteAppsParser
{
    /// <summary>
    ///     RemoteApps web client 'n parser class.
    /// </summary>
    public class RdWebClient : IRdWebClient, IDisposable
    {
        private const string loginPagePath = "/RDWeb/Pages/en-US/login.aspx";
        private const string defaultPagePath = "/RDWeb/Pages/en-US/Default.aspx";

        private readonly HttpClient client;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="httpClientHandler">Can be use in case of proxy.</param>
        public RdWebClient(HttpMessageHandler? httpClientHandler = null)
        {
            client = httpClientHandler == null
                ? new HttpClient()
                : new HttpClient(httpClientHandler);
        }

        /// <summary>
        ///     Dispose.
        /// </summary>
        public void Dispose()
        {
            client.Dispose();
        }

        /// <summary>
        ///     Connect to server and login.
        /// </summary>
        /// <param name="domain">
        ///     RemoteApps domain (
        ///     <example>https://ra.example.com</example>
        ///     ).
        /// </param>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        public async Task Connect(string domain, string login, string password)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"WorkSpaceID", new Uri(domain).Host},
                {"isUtf8", "1"},
                {"DomainUserName", HttpUtility.UrlEncode(login)},
                {"UserPass", HttpUtility.UrlEncode(password)},
            };

            client.BaseAddress = new Uri(domain);
            var requestContent = string.Join("&", dictionary.Select(x => x.Key + "=" + x.Value));
            var postContent = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(loginPagePath, postContent);
            var message = await response.Content.ReadAsStringAsync();
            var isSuccessful = message.Contains("<Resources>");

            if (isSuccessful) return;

            throw new Exception($"Login error. Server message: {message}");
        }

        /// <summary>
        ///     Fetch all remote resources.
        /// </summary>
        /// <returns>Task with root FolderModel object.</returns>
        public async Task<FolderModel> GetAllRemoteElements()
        {
            var rootFolder = await ParseRootFolder();

            return rootFolder;
        }


        private async Task<FolderModel> ParseRootFolder()
        {
            var folder = await FetchFolder("/", "");

            return folder;
        }

        private async Task<FolderModel> FetchFolder(string folderName, string parentPath)
        {
            Debug.WriteLine("Fetching " + folderName);

            var path = parentPath + folderName;
            var root = await FetchRootFeed(path);

            var subfolders = root!.Elements()
                .SingleOrDefault(x => x.Name.LocalName == "SubFolders")?.Elements()
                .Select(x => x.Attribute("Name")?.Value)
                .Where(x => x != null)
                .AsParallel()
                .Select(nameWithSlash => FetchFolder(nameWithSlash!, path).Result)
                .ToArray();

            var apps = root.Elements()
                .Single(x => x.Name.LocalName == "Resources").Elements()
                .AsParallel()
                .Select(x => ParseAppModel(x, path).Result)
                .ToArray();

            var title = folderName;
            var folder = new FolderModel(title.Substring(1), parentPath, apps, subfolders ?? new FolderModel[0]);

            return folder;
        }

        private async Task<XElement?> FetchRootFeed(string path)
        {
            var xmlContent = await client.GetStringAsync(defaultPagePath + path);
            var rdwaPage = XElement.Parse(xmlContent);

            var root = rdwaPage.Elements()
                .Single(x => x.Name.LocalName == "AppFeed")?.Elements()
                .Single(x => x.Name.LocalName == "ResourceCollection")?.Elements()
                .Single(x => x.Name.LocalName == "Publisher");

            return root;
        }

        private async Task<AppModel> ParseAppModel(XElement resourceElement, string parentPath)
        {
            var title = resourceElement.Attribute("Title")?.Value;
            var rdpFileUrl = resourceElement.Elements()
                .Single(x => x.Name.LocalName == "HostingTerminalServers")?.Elements()
                .Single(x => x.Name.LocalName == "HostingTerminalServer")?.Elements()
                .Single(x => x.Name.LocalName == "ResourceFile")
                .Attribute("URL")?.Value;

            if (title == null || rdpFileUrl == null)
                throw new Exception("Something in " + parentPath + " haven't got ID, Title or ResourceFile.");

            var pngIconUrl = resourceElement.Elements()
                                 .Single(x => x.Name.LocalName == "Icons")?.Elements()
                                 .Single(x => x.Name.LocalName == "Icon32")
                                 .Attribute("FileURL")?.Value
                             ?? string.Empty;

            var rdpFileRawTask = client.GetByteArrayAsync(rdpFileUrl);
            var pngFileRawTask = client.GetByteArrayAsync(pngIconUrl);
            await Task.WhenAll(rdpFileRawTask, pngFileRawTask);

            var app = new AppModel(title, parentPath, rdpFileRawTask.Result, pngFileRawTask.Result);

            Debug.WriteLine("App added: " + title);
            return app;
        }
    }
}