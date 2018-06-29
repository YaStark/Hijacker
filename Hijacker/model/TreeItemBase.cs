using System.Collections.Generic;
using Hijacker.controls;

namespace Hijacker.model
{
    public abstract class TreeItemBase : ITreeItem
    {
        public ITreeItem Root { get; protected set; }
        public List<ITreeItem> Children { get; protected set; }
        public string Name { get; protected set; }
        public string Path { get; protected set; }
        public string FilePath { get; protected set; }
        public bool Visible { get; set; }
        public string Tag { get; set; }

        protected TreeItemBase()
        {
            Tag = "Common";
        }
    }
}
