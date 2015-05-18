using NUnit.Framework;
using soothsayer.Oracle.Configuration;

namespace soothsayer.Tests.Oracle.Configuration
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

