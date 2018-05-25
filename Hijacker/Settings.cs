using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hijacker.controls;

namespace Hijacker
{
    [Serializable]
    public class Settings
    {
        private const string c_fileName = "cfg.ini";

        public string[] VobPaths { get; set; }

        public int[] HiddenItems { get; set; }

        public Settings()
        {
            HiddenItems = new int[0];
            VobPaths = new string[0];
        }

        public Settings(int[] hiddenItems, string[] vobPaths)
        {
            HiddenItems = hiddenItems;
        }

        private static void GetHiddenItems(ITreeItem item, ref List<int> hashs)
        {
            if (!item.Visible) hashs.Add(item.GetHashCode());
            if (item.Children == null) return;
            foreach (ITreeItem child in item.Children)
            {
                GetHiddenItems(child, ref hashs);
            }
        }

        private static void SetHiddenItems(ITreeItem item, int[] hashs)
        {
            item.Visible = !hashs.Contains(item.GetHashCode());
            if (item.Children == null) return;
            foreach (ITreeItem child in item.Children)
            {
                SetHiddenItems(child, hashs);
            }
        }

        public void Apply(ITreeItem[] slns)
        {
            foreach (ITreeItem sln in slns)
            {
                SetHiddenItems(sln, HiddenItems);                
            }
        }
        
        public static void Set(ITreeItem[] items)
        {
            List<int> hashs = new List<int>();
            foreach (ITreeItem item in items)
            {
                GetHiddenItems(item, ref hashs);
            }
            Settings settings = new Settings(hashs.Distinct().ToArray(), null);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream stream = new FileStream(c_fileName, FileMode.Truncate))
                {
                    serializer.Serialize(stream, settings);
                }
            }
            catch
            {
            }
        }

        public static Settings Get()
        {
            Settings settings = null;
            if (File.Exists("cfg.ini"))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    using (FileStream stream = new FileStream(c_fileName, FileMode.Open))
                    {
                        settings = (Settings) serializer.Deserialize(stream);
                    }
                }
                catch
                {
                }
            }
            return settings;
        }
    }
}
