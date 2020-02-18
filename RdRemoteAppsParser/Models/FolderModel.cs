namespace RdRemoteAppsParser.Models
{
    /// <summary>
    ///     Remote apps folder data.
    /// </summary>
    public class FolderModel : ItemModelBase
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="title">Folder name.</param>
        /// <param name="parentPath">Folder path without current folder name.</param>
        /// <param name="apps">Apps that current folder contains.</param>
        /// <param name="subfolders">Subfoldfers that current folder contains.</param>
        public FolderModel(string title, string parentPath, AppModel[] apps, FolderModel[] subfolders) : base(
            parentPath, title)
        {
            Apps = apps;
            Subfolders = subfolders;
        }

        /// <summary>
        ///     Apps that current folder contains.
        /// </summary>
        public AppModel[] Apps { get; set; }

        /// <summary>
        ///     Subfoldfers that current folder contains.
        /// </summary>
        public FolderModel[] Subfolders { get; set; }
    }
}