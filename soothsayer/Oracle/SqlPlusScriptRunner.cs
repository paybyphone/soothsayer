using soothsayer.Infrastructure;
using soothsayer.Oracle.Configuration;
using soothsayer.Scripts;

namespace soothsayer.Oracle
{
    public class SqlPlusScriptRunner : IScriptRunner
    {
        private readonly IProcessRunner _processRunner;

        private readonly string _username;
        private readonly string _password;
        private readonly string _connectionString;
        private readonly ISqlPlusConfiguration _sqlPlusConfiguration;

        public SqlPlusScriptRunner(IProcessRunner processRunner, DatabaseConnectionInfo databaseConnectionInfo)
            : this(databaseConnectionInfo.ConnectionString, databaseConnectionInfo.Username, databaseConnectionInfo.Password)
        {
            _processRunner = processRunner;
            _sqlPlusConfiguration = new SqlPlusConfiguration();
        }

        public SqlPlusScriptRunner(string connectionString, string username, string password)
        {
            _username = username;
            _password = password;
            _connectionString = connectionString;
        }

        public void Execute(IScript script)
        {
            var sqlPlusScript = new SqlPlusScript(script);

            var arguments = "{0}/{1}@\"{2}\" @{3}".FormatWith(_username, _password, _connectionString, sqlPlusScript.Path);

            var exitCode = _processRunner.Execute(_sqlPlusConfiguration.RunnerPath, arguments);

            if (exitCode > 0)
            {
                Output.Error("sqlplus encountered an error running script '{0}'.".FormatWith(script.Name));
                throw new SqlPlusException();
            }
        }
    }
}
