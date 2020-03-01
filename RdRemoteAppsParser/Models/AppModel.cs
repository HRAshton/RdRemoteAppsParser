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
        /// <param name="pngFileRaw">Raw png icon.</param>
        /// <param name="rdpFileRaw">Raw .rdp file.</param>
        public AppModel(string title, string parentPath, byte[] pngFileRaw, byte[] rdpFileRaw) : base(parentPath, title)
        {
            PngFileRaw = pngFileRaw;
            RdpFileRaw = rdpFileRaw;
        }

        /// <summary>
        ///     Raw png icon.
        /// </summary>
        public byte[] PngFileRaw { get; set; }

        /// <summary>
        ///     Raw .rdp file.
        /// </summary>
        public byte[] RdpFileRaw { get; set; }
    }
}