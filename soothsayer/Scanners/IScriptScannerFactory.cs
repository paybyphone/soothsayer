namespace soothsayer.Scanners
{
    public interface IScriptScannerFactory
    {
        IScriptScanner Create(string scriptFolder);
    }
}