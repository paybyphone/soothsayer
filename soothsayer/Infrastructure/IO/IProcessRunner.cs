namespace soothsayer.Infrastructure.IO
{
    public interface IProcessRunner
    {
        int Execute(string processFullPath, string arguments);
    }
}
