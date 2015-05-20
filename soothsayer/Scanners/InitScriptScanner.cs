using soothsayer.Infrastructure;

namespace soothsayer.Scanners
{
    public class InitScriptScanner : UpScriptScanner
    {
        public InitScriptScanner(IFilesystem filesystem) : base(filesystem)
        {
        }
    }
}
