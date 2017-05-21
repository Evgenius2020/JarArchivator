namespace JarArchivator
{
    public class JarArchivator : IJarArchivator
    {
        public IJarArchive Open(string archiveFileName)
        {
            return new JarArchive(archiveFileName);
        }

        public IJarArchive Pack(string sourceFolder, string archiveFileName)
        {
            return new JarArchive(sourceFolder, archiveFileName);
        }
    }
}
