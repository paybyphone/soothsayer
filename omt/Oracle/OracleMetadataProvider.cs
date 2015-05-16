using System.Data;
using System.Linq;
using Dapper;
using omt.Infrastructure;

namespace omt.Oracle
{
    public class OracleMetadataProvider : IDatabaseMetadataProvider
    {
        private readonly IDbConnection _connection;

        public OracleMetadataProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool SchemaExists(string schema)
        {
            var matchingSchemas = _connection.Query<string>(@"select username from dba_users where lower(username) = '{0}'".FormatWith(schema));

            return matchingSchemas.Any();
        }
    }
}
