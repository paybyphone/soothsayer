using NUnit.Framework;
using omt.Oracle.Configuration;

namespace omt.Tests.Oracle.Configuration
{
    [TestFixture]
    public class SqlPlusConfigurationTests
    {
        [Test]
        public void can_read_runner_path_from_sqlplus_configuration()
        {
            var sqlPlusConfiguration = new SqlPlusConfiguration();

            var runnerPath = sqlPlusConfiguration.RunnerPath;

            Assert.That(runnerPath, Is.EqualTo("C:\\some\\fake\\path\\to\\sqlplus.exe"));
        }
    }
}

