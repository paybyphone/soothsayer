using System.Collections.Generic;

namespace soothsayer.Infrastructure.IO
{
    public interface IFilesystem
    {
        IEnumerable<string> GetFiles(string directoryPath, string filePattern);
    }
}
