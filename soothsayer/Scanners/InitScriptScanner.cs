using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;

namespace soothsayer.Scanners
{
    public class InitScriptScanner : UpScriptScanner
    {
        public InitScriptScanner(IFilesystem filesystem) : base(filesystem)
        {
        }
    }
}
