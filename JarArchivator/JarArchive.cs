using System.Collections.Generic;
using System.IO.Compression;

namespace JarArchivator
{
    public class JarArchive : IJarArchive
    {
        private readonly string _archiveFileName;

        public IEnumerable<IJarArchiveFile> Files { get; }

        public JarArchive(string archiveFileName)
        {
            _archiveFileName = archiveFileName;
            Files = getFilesList(archiveFileName);
        }

        public JarArchive(string sourceFolder, string archiveFileName)
        {
            _archiveFileName = archiveFileName;
            ZipFile.CreateFromDirectory(sourceFolder, archiveFileName);
            Files = getFilesList(archiveFileName);
        }

        public void Unpack(string targetFolder)
        {
            // TODO: No file error
            ZipFile.ExtractToDirectory(_archiveFileName, targetFolder);
        }

        private List<JarArchiveFile> getFilesList(string archiveFileName)
        {
            var files = new List<JarArchiveFile>();
            using (ZipArchive archive = ZipFile.OpenRead(archiveFileName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    var filepath = entry.FullName;
                    if (entry.Name.Length != 0)
                    {
                        filepath = filepath.Remove(entry.FullName.Length - entry.Name.Length);
                    }
                    var file = new JarArchiveFile(entry.Name, filepath, entry.Length);
                    files.Add(file);
                }
            }
            return files;
        }
    }
}
