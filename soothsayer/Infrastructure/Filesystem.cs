using System.Collections.Generic;
using System.IO;

namespace soothsayer.Infrastructure
{
    public class Filesystem : IFilesystem
    {
        public IEnumerable<string> GetFiles(string directoryPath, string filePattern)
        {
            var files = Directory.GetFiles(directoryPath, filePattern);
            return files;
        }
    }
}