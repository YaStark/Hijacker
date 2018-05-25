using System.Text.RegularExpressions;

namespace Hijacker.ext
{
    static class PathEx
    {
        const string c_upDirPathsPattern = @"\\((?!\\)(?!\.\.).)+\\\.\.";

        public static string ApplyAllUpDirPaths(string path)
        {
            while (Regex.Match(path, c_upDirPathsPattern).Success)
            {
                path = Regex.Replace(path, c_upDirPathsPattern, string.Empty);
            }
            return path;
        }
    }
}
