using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace JarArchivator.Tests
{
    [TestClass]
    public class ArchivationTests
    {
        private const string _testDirectory = "testDir/";
        [TestInitialize]
        public void CreateTestDirectory()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
            Directory.CreateDirectory(_testDirectory);
        }
        [TestCleanup]
        public void CleanTestDirectory()
        {
            Directory.Delete(_testDirectory, true);
        }

        [TestMethod]
        public void PackOpenUnpack()
        {
            var testText = new string('t', 50);
            const string srcDirectory = _testDirectory + "src/";
            const string destDirectory = _testDirectory + "dest/";
            Directory.CreateDirectory(srcDirectory);
            const string testTextFileName = "test.txt";
            const string testTextFullFileName = srcDirectory + testTextFileName;
            const string archiveFileName = _testDirectory + "test.jar";
            var testFile = File.CreateText(testTextFullFileName);
            testFile.Write(testText);
            testFile.Dispose();

            // Pack.
            var archivator = new JarArchivator();
            var archive = archivator.Pack(srcDirectory, archiveFileName);
            var filesList = archive.Files as List<JarArchiveFile>;
            Assert.AreEqual(1, filesList.Count);
            var archiveFile = filesList[0];
            Assert.AreEqual(testTextFileName, archiveFile.Name);
            Assert.AreEqual("", archiveFile.Path); // Empty because in the root of source directory.

            // Open.
            archive = archivator.Open(archiveFileName);
            filesList = archive.Files as List<JarArchiveFile>;
            Assert.AreEqual(1, filesList.Count);
            archiveFile = filesList[0];
            Assert.AreEqual(testTextFileName, archiveFile.Name);
            Assert.AreEqual("", archiveFile.Path);

            // Unpack.
            archive.Unpack(destDirectory);
            Assert.IsTrue(File.Exists(destDirectory + testTextFileName));
            Assert.AreEqual(File.ReadAllText(destDirectory + testTextFileName), testText);
        }
    }
}
