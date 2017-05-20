using System.Collections.Generic;

namespace JarArchivator
{
    public interface IJarArchiveFile
    {
        string Name { get; }
        string Path { get; }
        long Size { get; }
    }

    public interface IJarArchive
    {
        IEnumerable<IJarArchiveFile> Files { get; }
        void Unpack(string targetFolder);
    }

    public interface IJarArchivator
    {
        IJarArchive Pack(string sourceFolder, string archiveFileName);
        IJarArchive Open(string archiveFileName);
    }
}
