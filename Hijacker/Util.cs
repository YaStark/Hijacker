using System;
using System.Collections.Generic;
using System.Linq;

namespace Hijacker
{
    public static class Util
    {
        public static string TrimPath(string path)
        {
            List<string> nodes = path.Split('\\').ToList();
            int i = 1;
            while (true)
            {
                if (i >= nodes.Count) break;
                if (nodes[i] == "..")
                {
                    nodes.RemoveAt(i);
                    nodes.RemoveAt(i - 1);
                }
                i++;
            }
            return string.Join("\\", nodes);
        }

    }
}
