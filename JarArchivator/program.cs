using System;

namespace JarArchivator
{
    public class Program
    {
        public static void Main()
        {
            var archivator = new JarArchivator();
            foreach (var file in archivator.Open("src.jar").Files)
            {
                Console.WriteLine("{0} - {1} - {2}B", file.Path, file.Name, file.Size);
            }
        }
    }
}
