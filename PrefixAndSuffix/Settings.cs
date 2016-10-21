using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrefixAndSuffix
{
    public class Settings
    {
        public static bool usePrefixes = true;
        public static bool useSuffixes = false;
        public static bool ignoreAdminTag = false;
        public static bool ignoreProTag = true;
        public static List<string> ignoreGroupList = new List<string>();
    }
}
