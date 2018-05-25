using System.Collections.Generic;

namespace Hijacker.controls
{
    public interface ITreeItem
    {
        ITreeItem Root { get; }
        List<ITreeItem> Children { get; }
        string Name { get; }
        string Path { get; }
        string FilePath { get; }
        bool Visible { get; set; }
    }
}
