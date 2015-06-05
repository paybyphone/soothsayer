using soothsayer.Infrastructure.IO;
using soothsayer.Oracle;

namespace soothsayer
{
    public class SqlPlusScriptRunnerFactory : IScriptRunnerFactory
    {
        private readonly IProcessRunner _processRunner;

        public SqlPlusScriptRunnerFactory(IProcessRunner processRunner)
        {
            _processRunner = processRunner;
        }

        public IScriptRunner Create(DatabaseConnectionInfo connectionInfo)
        {
            return new SqlPlusScriptRunner(_processRunner, connectionInfo);
        }
    }
}