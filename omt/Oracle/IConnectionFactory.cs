using System.Data;

namespace omt.Oracle
{
    public interface IConnectionFactory
    {
        IDbConnection Create(DatabaseConnectionInfo databaseConnectionInfo);
        IDbConnection Create(string connectionString, string username, string password);
    }
}