using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Hijacker.ext
{
    static class PathEx
    {
        const string c_upDirPathsPattern = @"\\((?!\\)(?!\.\.).)+\\\.\.";
        const string c_pathViewPattern = @"Path[\ ]+(.+)";

        public static string ApplyAllUpDirPaths(string path)
        {
            while (Regex.Match(path, c_upDirPathsPattern).Success)
            {
                path = Regex.Replace(path, c_upDirPathsPattern, string.Empty);
            }
            return path;
        }

        public static string MakeDiskRootFromUncRoot(string uncPath)
        {
            return new Uri(uncPath).LocalPath;
        }

        public static string ConvertUncPathToPhysicalPath(string path)
        {
            if (!IsUncPath(path)) return path;
            var result = Cmd("share " + path.Replace(Environment.MachineName, string.Empty).Trim('\\'), "net.exe");
            var match = Regex.Match(result, c_pathViewPattern);
            if(match.Success)
            {
                return match.Groups[1].Value.Replace("\r", "").Replace("\n", "");
            }
            return path;
        }

        public static bool IsUncPath(string path)
        {
            try { return (new Uri(path)).IsUnc; }
            catch { return false; }
        }

        public static string Cmd(string cmd, string app = "cmd.exe")
        {
            string data = null;
            try
            {
                Process process = new Process();
                process.StartInfo.Arguments = cmd;
                process.StartInfo.FileName = app;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                data = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return data;
        }
    }
}
