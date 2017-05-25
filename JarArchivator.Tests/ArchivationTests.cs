using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace JarArchivator.Tests
{
    [TestClass]
    public class ArchivationTests
    {
        private const string _testDirectory = @"testDir\";
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
            const string srcDirectory = _testDirectory + @"src\";
            Directory.CreateDirectory(srcDirectory);
            const string testTextFileName = "test.txt";
            const string testTextFullFileName = srcDirectory + testTextFileName;
            const string archiveFileName = _testDirectory + "test.jar";
            using (var file = File.CreateText(testTextFullFileName))
            {
                file.Write(new string('t', 50));
            }
            string destDirectory = _testDirectory + "dest/";

            // Pack and open.
            var archivator = new JarArchivator();
            var archive = archivator.Pack(srcDirectory, archiveFileName);
            var filesList = archive.Files as List<JarArchiveFile>;
            Assert.AreNotEqual(0, filesList.Count);
            Assert.IsTrue(filesList.Exists(f => f.Name == testTextFileName));
            var testFile = filesList.Find(f => f.Name == testTextFileName);
            Assert.AreEqual(testTextFileName, testFile.Name);
            Assert.AreEqual("", testFile.Path); // Empty because in the root of source directory.

            // Unpack.
            archive.Unpack(destDirectory);
            Assert.IsTrue(File.Exists(destDirectory + testTextFileName));
        }
    }
}
