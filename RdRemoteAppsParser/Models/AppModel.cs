namespace RdRemoteAppsParser.Models
{
    /// <summary>
    ///     Single remote application data.
    /// </summary>
    public sealed class AppModel : ItemModelBase
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="title">App name.</param>
        /// <param name="parentPath">App path without app name.</param>
        /// <param name="rdpFileUrl">Url of .rdp file.</param>
        /// <param name="pngPathUrl">Url of .png icon.</param>
        public AppModel(string title, string parentPath, string rdpFileUrl, string pngPathUrl) : base(parentPath, title)
        {
            PngPathUrl = pngPathUrl;
            RdpFileUrl = rdpFileUrl;
        }

        /// <summary>
        ///     Url of .png icon.
        /// </summary>
        public string PngPathUrl { get; set; }

        /// <summary>
        ///     Url of .rdp file.
        /// </summary>
        public string? RdpFileUrl { get; set; }
    }
}