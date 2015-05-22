using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;

namespace soothsayer.Scanners
{
    public class TermScriptScanner : DownScriptScanner
    {
        public TermScriptScanner(IFilesystem filesystem) : base(filesystem)
        {
        }
    }
}
