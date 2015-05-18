namespace soothsayer.Infrastructure
{
    public interface IProcessRunner
    {
        int Execute(string processFullPath, string arguments);
    }
}
