using System.Collections.Generic;
using Hijacker.controls;

namespace Hijacker.model
{
    public class FolderItem : TreeItemBase
    {
        public FolderItem(string path)
        {
            Root = null;
            Name = System.IO.Path.GetFileName(path);
            Children = new List<ITreeItem>();
            Path = path;
            FilePath = path;
            Visible = true;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}
