using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace JarArchivator
{
    public class JarArchive : IJarArchive
    {
        private const string _executableFilePath = @"C:\Program Files\Java\jdk1.8.0_131\bin\jar.exe";
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
                process.StartInfo.FileName = _executableFilePath;
                // TODO: Paths not working correclty with spaced filenames.
                // This is not working.
                // process.StartInfo.Arguments = $@"cf ""{archiveFileName}"" -C ""{sourceFolder}"" ./"; 
                process.StartInfo.Arguments = $"cfv {archiveFileName} -C {sourceFolder} ./";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();
                process.WaitForExit();
            }
            Files = getFilesList(archiveFileName);
        }

        public void Unpack(string targetFolder)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = _executableFilePath;
                // TODO: Bad idea. Only relative path is supported.
                process.StartInfo.Arguments = $@"xf ""{Directory.GetCurrentDirectory()}\{_archiveFileName}""";
                process.StartInfo.UseShellExecute = false;
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
                process.StartInfo.WorkingDirectory = Directory.GetParent(targetFolder).ToString();
                process.Start();
                process.WaitForExit();
            }
        }

        private IEnumerable<JarArchiveFile> getFilesList(string archiveFileName)
        {
            var files = new List<JarArchiveFile>();
            using (var process = new Process())
            {
                process.StartInfo.FileName = _executableFilePath;
                process.StartInfo.Arguments = $"tvf {archiveFileName}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                while (!process.StandardOutput.EndOfStream)
                {
                    // Line format : indents(' ')-size-DD-MM-TT-timezone-YY-fullname (splitted by ' ')
                    string rawData = process.StandardOutput.ReadLine().TrimStart(' ');
                    string[] rawDataParts = rawData.Split(' ');
                    var size = long.Parse(rawDataParts[0]);

                    // Parsing dirnames with ' '('sample directory'\)
                    var fullName = string.Empty;
                    for (var i = 7; i < rawDataParts.Length; i++)
                    {
                        fullName += rawDataParts[i];

                    }
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
