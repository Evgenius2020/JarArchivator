using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            using (var process = new Process())
            {
                process.StartInfo.FileName = @"C:\Program Files\Java\jdk1.8.0_131\bin\jar.exe";
                process.StartInfo.Arguments = $"cf {archiveFileName} {sourceFolder}";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
            Files = getFilesList(archiveFileName);
        }

        public void Unpack(string targetFolder)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = @"C:\Program Files\Java\jdk1.8.0_131\bin\jar.exe";
                process.StartInfo.Arguments = $"xf {_archiveFileName} {targetFolder}";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
        }

        private IEnumerable<JarArchiveFile> getFilesList(string archiveFileName)
        {
            var files = new List<JarArchiveFile>();
            using (var process = new Process())
            {
                process.StartInfo.FileName = @"C:\Program Files\Java\jdk1.8.0_131\bin\jar.exe";
                process.StartInfo.Arguments = "tvf " + archiveFileName;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                while (!process.StandardOutput.EndOfStream)
                {
                    List<string> rawData = process.StandardOutput.ReadLine().Split(' ').ToList();
                    rawData.RemoveAll(t => t == "");
                    var size = long.Parse(rawData[0]);
                    var fullName = rawData[7];
                    var indexOfSeparation = fullName.Contains('/') ? fullName.LastIndexOf('/') + 1 : 0;
                    var path = fullName.Substring(0, indexOfSeparation);
                    var name = fullName.Substring(path.Length);
                    var file = new JarArchiveFile(name, path, size);
                    files.Add(file);
                }
            }
            return files;
        }
    }
}
