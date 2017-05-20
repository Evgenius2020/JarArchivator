using System;
using System.Collections.Generic;

namespace JarArchivator
{
    public class JarArchive : IJarArchive
    {
        private string _archivePath;
        public IEnumerable<IJarArchiveFile> Files { get; }

        public JarArchive(IEnumerable<IJarArchiveFile> files)
        {
            Files = files;
        }

        public void Unpack(string targetFolder)
        {
            throw new NotImplementedException();
        }
    }
}
