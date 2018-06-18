namespace PackageManager
{
    public class PackageFilenformation
    {
        public string FileName { get; set; }
        public int Unknown01 { get; set; }
        public int FileSize { get; set; }
        public byte[] Sha256HashValue { get; set; }
    }
}
