using System.Data;

namespace soothsayer
{
    public interface ICreateFromDatabaseConnection<out T>
    {
         T Create(IDbConnection connection);
    }
}