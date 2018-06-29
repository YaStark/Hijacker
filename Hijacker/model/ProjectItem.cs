using System.Collections.Generic;
using System.Linq;
using Hijacker.controls;
using Microsoft.Build.Evaluation;

namespace Hijacker.model
{
    public class ProjectItem : Project, ITreeItem
    {
        public ITreeItem Root { get; private set; }
        public List<ITreeItem> Children { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string FilePath { get; private set; }
        public bool Visible { get; set; }
        public ProjectDescription[] Dependencies { get; private set; }
        public string DllPath { get; private set; }
        public string Tag { get; set; }

        public ProjectItem(SolutionItem solution, string path) : base(path)
        {
            Root = solution;
            Name = System.IO.Path.GetFileName(path);
            Path = Util.TrimPath(System.IO.Path.GetDirectoryName(path));
            FilePath = solution.FilePath;
            DllPath = System.IO.Path.Combine(Path, GetValueOrStringEmpty("OutputPath", "OutDir") + GetValueOrStringEmpty("TargetFileName"));
            DllPath = Util.TrimPath(DllPath);
            Children = new List<ITreeItem>();
            Visible = true;
            Dependencies = Items.Where(x => x.ItemType == "Reference").Select(x => new ProjectDescription(x, Path)).ToArray();
        }

        private string GetValueOrStringEmpty(params string[] keys)
        {
            foreach (string key in keys)
            {
                ProjectProperty value = Properties.FirstOrDefault(x => x.Name == key);
                if (value != null && !string.IsNullOrEmpty(value.EvaluatedValue))
                {
                    return value.EvaluatedValue;
                }
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return (Path + Name).GetHashCode();
        }
    }

    public class ProjectDescription
    {
        public string FileName { get; private set; }

        public string Name { get; private set; }

        public string Path { get; private set; }

        public ProjectDescription(Microsoft.Build.Evaluation.ProjectItem x, string dllPath)
        {
            Name = x.EvaluatedInclude;
            FileName = Name.Split(',').First();
            Path = System.IO.Path.Combine(dllPath, x.GetMetadataValue("HintPath"));
        }
    }
}
