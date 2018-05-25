using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Hijacker.controls;

namespace Hijacker.model
{
    public static class TreeBuilder
    {
        public static ITreeItem[] Build(string rootPath, Action<double?, string> progress)
        {
            var roots = Directory.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
            FolderItem root = new FolderItem(rootPath);
            double rateProgress = 1.0/roots.Count();
            int current = 0;
            foreach (string viewFolder in roots)
            {
                current++;
                progress(current * rateProgress, null);
                progress(null, "Folder: " + viewFolder + "\r\n\r\n\r\n");
                var files = Directory.EnumerateFiles(viewFolder, "*.sln", SearchOption.AllDirectories);
                var folderItem = new FolderItem(Path.Combine(rootPath, viewFolder));
                SolutionItem[] slns = files.Select(x => TryCreateSolution(x, folderItem, progress)).Where(x => x != null).ToArray();
                if (slns.Length > 0)
                {
                    folderItem.Children.AddRange(slns);
                    root.Children.Add(folderItem);                    
                }
            }
            return root.Children.ToArray();
        }

        private static SolutionItem TryCreateSolution(string slnName, FolderItem viewFolder, Action<double?, string> progress)
        {
            SolutionItem sln = new SolutionItem(slnName, viewFolder);
            if (sln.Children.Count > 0) progress(null, "Folder: " + viewFolder.Path + "\r\n\r\n" + "Solution: " + sln.Name);
            return sln.Children.Count > 0 ? sln : null;
        }

        public static ITreeItem[] AddFilter(FolderItem[] items, TreeFilterType filter)
        {
            var root = new FolderItem(string.Empty);
            var dirFilter = filter.GetDirFilter();

            foreach (var dir in items)
            {
                var newDir = new FolderItem(dir.Path);
                var filterDirs = dirFilter.Select(x => Path.Combine(dir.Path, x));
                newDir.Children.AddRange(dir.Children.Where(sln => filterDirs.Any(fdir => sln.Path.Contains(fdir))));
                if (newDir.Children.Count > 0)
                {
                    root.Children.Add(newDir);
                }
            }
            return root.Children.ToArray();
        }
    }
}
