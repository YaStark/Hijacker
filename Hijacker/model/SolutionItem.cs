using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hijacker.controls;
using Microsoft.Build.Exceptions;

namespace Hijacker.model
{
    public class SolutionItem : ITreeItem
    {
        //internal class SolutionParser
        //Name: Microsoft.Build.Construction.SolutionParser
        //Assembly: Microsoft.Build, Version=4.0.0.0

        static readonly Type s_solution = Type.GetType("Microsoft.Build.Construction.SolutionParser, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
        static readonly Type s_projectInSolution = Type.GetType("Microsoft.Build.Construction.ProjectInSolution, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
        static readonly PropertyInfo s_solutionParser_solutionReader;
        static readonly MethodInfo s_solutionParser_parseSolution;
        static readonly PropertyInfo s_solutionParser_projects;
        static readonly PropertyInfo s_projectInSolutionRelativePath;

        public ITreeItem Root { get; private set; }
        public List<ITreeItem> Children { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string FilePath { get; private set; }
        public bool Visible { get; set; }
        public ProjectDescription[] Dependencies { get; private set; }

        static SolutionItem()
        {
            s_solutionParser_solutionReader = s_solution.GetProperty("SolutionReader", BindingFlags.NonPublic | BindingFlags.Instance);
            s_solutionParser_projects = s_solution.GetProperty("Projects", BindingFlags.NonPublic | BindingFlags.Instance);
            s_solutionParser_parseSolution = s_solution.GetMethod("ParseSolution", BindingFlags.NonPublic | BindingFlags.Instance);
            s_projectInSolutionRelativePath = s_projectInSolution.GetProperty("RelativePath", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public SolutionItem(string solutionFileName, FolderItem viewFolder)
        {
            var solutionParser = s_solution.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First().Invoke(null);

            FilePath = solutionFileName;
            Path = System.IO.Path.GetDirectoryName(solutionFileName);
            Name = System.IO.Path.GetFileName(solutionFileName);
            Root = viewFolder;
            Children = new List<ITreeItem>();
            Visible = true;

            using (var streamReader = new StreamReader(solutionFileName))
            {
                s_solutionParser_solutionReader.SetValue(solutionParser, streamReader, null);
                try
                {
                    s_solutionParser_parseSolution.Invoke(solutionParser, null);
                }
                catch (TargetInvocationException)
                {
                    return;
                }
            }
            var projects = new List<ProjectItem>();
            var solutionPath = System.IO.Path.GetDirectoryName(solutionFileName);
            var array = (Array)s_solutionParser_projects.GetValue(solutionParser, null);
            for (int i = 0; i < array.Length; i++)
            {
                var relativeProjFileName = s_projectInSolutionRelativePath.GetValue(array.GetValue(i), null) as string;
                if (relativeProjFileName != null && relativeProjFileName.Contains(".csproj"))
                {
                    string projPath = System.IO.Path.Combine(solutionPath, relativeProjFileName);
                    if (File.Exists(projPath))
                    {
                        try
                        {
                            projects.Add(new ProjectItem(this, projPath));
                        }
                        catch (InvalidProjectFileException) { }
                    }
                }
            }
            Children.AddRange(projects);
            Dependencies = projects.SelectMany(x => x.Dependencies).GroupBy(x => x.FileName).Select(x => x.First()).ToArray();
        }

        public bool IsInDependencies(ProjectItem prj)
        {
            if (prj == null) return false;
            return Dependencies.Any(x => x.FileName == System.IO.Path.GetFileNameWithoutExtension(prj.DllPath));
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} projects)", Name, Children == null ? 0 : Children.Count);
        }

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }
    }
}
