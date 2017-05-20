namespace JarArchivator
{
    public class JarArchiveFile : IJarArchiveFile
    {
        public string Name { get; }
        public string Path { get; }
        public long Size { get; }

        public JarArchiveFile(string name, string path, long size)
        {
            Name = name;
            Path = path;
            Size = size;
        }
    }
}
