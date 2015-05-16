using System.IO;
using System.Reflection;
using CommandLine;
using Moq;
using NUnit.Framework;
using omt.Commands;
using omt.Infrastructure;

namespace omt.Tests.Commands
{
    [TestFixture]
    public class MigrateCommandTests : BaseDatabaseCommandTestFixture
    {
        private Mock<IMigrator> _mockMigrator;
        private readonly string[] _requiredMigrateCommandArguments = { "-c", "connection string", "-s", "schema", "-u", "username", "-i", "input folder" };

        [SetUp]
        public void SetUp()
        {
            Prompt.ConsoleProvider = new Mock<IConsole>().Object;

            _mockMigrator = new Mock<IMigrator>();
        }

        [Test]
        public void when_no_arguments_have_been_specified_then_the_list_command_help_text_is_displayed()
        {
            var stringWriter = new StringWriter();
            typeof(ParserSettings).GetProperty("Consumed", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Parser.Default.Settings, false);
            Parser.Default.Settings.HelpWriter = stringWriter;

            var listCommand = new MigrateCommand(MockSecureConsole.Object, _mockMigrator.Object);

            listCommand.Execute(new string[] { });

            Assert.That(stringWriter, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void when_migrate_command_is_executed_then_migration_is_run()
        {
            var listCommand = new MigrateCommand(MockSecureConsole.Object, _mockMigrator.Object);

            listCommand.Execute(_requiredMigrateCommandArguments);

            _mockMigrator.Verify(m => m.Migrate(It.IsAny<DatabaseConnectionInfo>(), It.IsAny<MigrationInfo>()));
        }
    }
}