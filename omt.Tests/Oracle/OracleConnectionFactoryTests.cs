using NUnit.Framework;
using omt.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace omt.Tests.Oracle
{
    [TestFixture]
    public class OracleConnectionFactoryTests
    {
        [Test]
        public void connection_factory_constructs_a_oracle_database_connection()
        {
            var connectionFactory = new OracleConnectionFactory();
            var connection = connectionFactory.Create("connectionString", "username", "password");

            Assert.That(connection, Is.TypeOf<OracleConnection>());
        }

        [Test]
        public void the_constructed_oracle_database_connection_has_correct_string()
        {
            var connectionFactory = new OracleConnectionFactory();
            var connection = connectionFactory.Create("connectionString", "username", "password");

            Assert.That(connection.ConnectionString, Is.EqualTo(@"DATA SOURCE=connectionString;User ID=username;Password=password;Validate Connection=true;Statement Cache Size=0"));
        }
    }
}

