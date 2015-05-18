using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace soothsayer.Oracle
{
    public class OracleConnectionFactory : IConnectionFactory
    {
        public IDbConnection Create(DatabaseConnectionInfo databaseConnectionInfo)
        {
            return Create(databaseConnectionInfo.ConnectionString, databaseConnectionInfo.Username, databaseConnectionInfo.Password);
        }

        public IDbConnection Create(string connectionString, string username, string password)
        {
            return new OracleConnection(@"DATA SOURCE=" + connectionString + @";User ID=" + username + @";Password="+ password + @";Validate Connection=true;Statement Cache Size=0");
        }
    }
}
