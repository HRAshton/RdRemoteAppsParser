namespace RdRemoteAppsParser.Models
{
    /// <summary>
    ///     Remote item (folder or app) data.
    /// </summary>
    public abstract class ItemModelBase
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="title">Item name.</param>
        /// <param name="parentPath">Item path without current item name.</param>
        protected ItemModelBase(string parentPath, string title)
        {
            Path = $"{parentPath}/{title}";
            Title = title;
        }

        /// <summary>
        ///     Item path related to server root.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Item name.
        /// </summary>
        public string Title { get; set; }
    }
}