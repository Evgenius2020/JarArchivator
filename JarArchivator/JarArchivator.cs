using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace JarArchivator
{
    public class JarArchivator //: IJarArchivator
    {
        public JarArchive Open(string archiveFileName)
        {
            var files = new List<JarArchiveFile>();
            using (ZipArchive archive = ZipFile.OpenRead(archiveFileName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string filepath = entry.FullName;
                    if (entry.Name.Length != 0)
                    {
                        filepath = filepath.Remove(entry.FullName.Length - entry.Name.Length);
                    }
                    var file = new JarArchiveFile(entry.Name, filepath, entry.Length);
                    files.Add(file);
                }
            }
        
            return new JarArchive(files);
        }

        public void Pack(string sourceFolder, string archiveFileName)
        {
            ZipFile.CreateFromDirectory(sourceFolder, archiveFileName);
        }
    }
}
