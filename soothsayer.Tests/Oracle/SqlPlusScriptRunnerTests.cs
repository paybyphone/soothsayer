using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Oracle;
using soothsayer.Scripts;

namespace soothsayer.Tests.Oracle
{
    [TestFixture]
    public class SqlPlusScriptRunnerTests
    {
        private Mock<IProcessRunner> _mockProcessRunner;
        private DatabaseConnectionInfo _fakeDatabaseInfo;

        [SetUp]
        public void SetUp()
        {
            _mockProcessRunner = new Mock<IProcessRunner>();
            _fakeDatabaseInfo = new DatabaseConnectionInfo("connection", "username", "password");
        }

        [Test]
        public void when_a_script_is_executed_through_the_sql_plus_script_runner_then_sql_plus_is_run()
        {
            var scriptRunner = new SqlPlusScriptRunner(_mockProcessRunner.Object, _fakeDatabaseInfo);

            var testScript = new Script(@"FakeScript.sql", 1);
            scriptRunner.Execute(testScript);

            _mockProcessRunner.Verify(m => m.Execute(It.Is<string>(s => s.EndsWith("sqlplus.exe")), It.IsAny<string>()), Times.Once);
        }
    }
}
