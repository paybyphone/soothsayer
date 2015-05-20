using System.Collections.Generic;

namespace soothsayer.Infrastructure
{
    public interface IFilesystem
    {
        IEnumerable<string> GetFiles(string directoryPath, string filePattern);
    }
}
