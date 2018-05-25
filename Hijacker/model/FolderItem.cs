using System.Collections.Generic;
using Hijacker.controls;

namespace Hijacker.model
{
    public class FolderItem : ITreeItem
    {
        public ITreeItem Root { get; private set; }
        public List<ITreeItem> Children { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string FilePath { get; private set; }
        public bool Visible { get; set; }

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
