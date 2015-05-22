using System.Data;

namespace soothsayer
{
    public interface IConnectionFactory
    {
        IDbConnection Create(DatabaseConnectionInfo databaseConnectionInfo);
        IDbConnection Create(string connectionString, string username, string password);
    }
}
