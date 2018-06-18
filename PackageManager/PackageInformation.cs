using System.Collections.Generic;

namespace PackageManager
{
    public class PackageInformation
    {
        public string DirectoryName { get; set; }
        public List<PackageFilenformation> Files { get; set; }
        public int DirectoryCount { get; set; }
    }
}
