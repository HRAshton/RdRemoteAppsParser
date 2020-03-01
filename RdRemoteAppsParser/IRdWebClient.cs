using System.Threading.Tasks;
using RdRemoteAppsParser.Models;

namespace RdRemoteAppsParser
{
    /// <summary>
    ///     RemoteApps web client 'n parser class.
    /// </summary>
    public interface IRdWebClient
    {
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
        Task Connect(string domain, string login, string password);

        /// <summary>
        ///     Fetch all remote resources.
        /// </summary>
        /// <returns>Task with root FolderModel object.</returns>
        Task<FolderModel> GetAllRemoteElements();
    }
}