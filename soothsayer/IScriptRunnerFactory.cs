namespace soothsayer
{
    public interface IScriptRunnerFactory
    {
        IScriptRunner Create(DatabaseConnectionInfo connectionInfo);
    }
}