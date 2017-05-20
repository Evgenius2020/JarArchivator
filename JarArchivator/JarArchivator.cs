namespace JarArchivator
{
    public class JarArchivator //: IJarArchivator
    {
        public JarArchive Open(string archiveFileName)
        {
            return new JarArchive(archiveFileName);
        }

        public JarArchive Pack(string sourceFolder, string archiveFileName)
        {
            return new JarArchive(sourceFolder, archiveFileName);
        }
    }
}
