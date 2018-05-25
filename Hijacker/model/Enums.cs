using System;
using System.Collections.Generic;

namespace Hijacker.model
{
    [Flags]
    public enum TreeFilterType
    {
        None = 0,
        NoRelVobs = 1,
        NoToolbox = 2,
        ProductsOnly = 3
    }

    public static class TreeFilterTypeExt
    {
        public static IEnumerable<string> GetDirFilter(this TreeFilterType filter)
        {
            if (filter.HasFlag(TreeFilterType.NoRelVobs))
            {
                yield return "rel";
            }
            if (filter.HasFlag(TreeFilterType.NoToolbox))
            {
                yield return "tlbx";
            }
        }
    }
}
