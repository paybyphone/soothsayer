using Moq;
using NUnit.Framework;
using soothsayer.Commands;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;

namespace soothsayer.Tests.Commands
{
    [TestFixture]
    public class DatabaseCommandTests : BaseDatabaseCommandTestFixture
    {
        [Test]
        public void when_no_password_provided_in_commandline_options_then_it_is_read_from_the_secure_console()
        {
            MockSecureConsole.Setup(m => m.ReadLine(It.IsAny<char>())).Returns("some password");

            var testDatabaseCommand = new TestDatabaseCommand(MockSecureConsole.Object);
            var password = testDatabaseCommand.GetOraclePassword(new TestDatabaseCommandOptions());

            MockSecureConsole.Verify(m => m.ReadLine(It.IsAny<char>()), Times.Once);
            Assert.That(password, Is.EqualTo("some password"));
        }

        [Test]
        public void when_a_password_has_already_been_provided_in_commandline_options_then_that_password_is_used()
        {
            var testDatabaseCommand = new TestDatabaseCommand(MockSecureConsole.Object);
            var password = testDatabaseCommand.GetOraclePassword(new TestDatabaseCommandOptions { Password = "provided password" });

            MockSecureConsole.Verify(m => m.ReadLine(It.IsAny<char>()), Times.Never);
            Assert.That(password, Is.EqualTo("provided password"));
        }
    }

    internal class TestDatabaseCommandOptions : IDatabaseCommandOptions
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    internal class TestCommandOptions : IOptions
    {
    }

    internal class TestDatabaseCommand : DatabaseCommand<TestCommandOptions>
    {
        public TestDatabaseCommand(ISecureConsole secureConsole)
            : base(secureConsole)
        {
        }

        public override string CommandText
        {
            get { return "test"; }
        }

        public override string Description
        {
            get { return "description"; }
        }

        protected override void ExecuteCore(string[] arguments)
        {
        }

        public new string GetOraclePassword(IDatabaseCommandOptions options)
        {
            return base.GetOraclePassword(options);
        }
    }
}
