using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using CommandLine;
using Moq;
using NUnit.Framework;
using omt.Commands;
using omt.Oracle;

namespace omt.Tests.Commands
{
    [TestFixture]
    public class ListCommandTests : BaseDatabaseCommandTestFixture
    {
        private Mock<IDbConnection> _mockConnection;
        private Mock<IConnectionFactory> _mockConnectionFactory;
        private Mock<IVersionRespository> _mockVersionRepository;
        private Mock<IVersionRespositoryFactory> _mockVersionRepositoryFactory;
        private readonly string[] _requiredListCommandArguments = { "-c", "connection string", "-s", "schema", "-u", "username" };

        [SetUp]
        public void SetUp()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));

            _mockConnection = new Mock<IDbConnection>();
            _mockConnectionFactory = new Mock<IConnectionFactory>();
            _mockConnectionFactory.Setup(m => m.Create(It.IsAny<DatabaseConnectionInfo>())).Returns(_mockConnection.Object);

            _mockVersionRepository = new Mock<IVersionRespository>();
            _mockVersionRepositoryFactory = new Mock<IVersionRespositoryFactory>();
            _mockVersionRepositoryFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockVersionRepository.Object);
        }

        [Test]
        public void when_no_arguments_have_been_specified_then_the_list_command_help_text_is_displayed()
        {
            var stringWriter = new StringWriter();
            typeof(ParserSettings).GetProperty("Consumed", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Parser.Default.Settings, false);
            Parser.Default.Settings.HelpWriter = stringWriter;

            var listCommand = new ListCommand(MockSecureConsole.Object, _mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object);

            listCommand.Execute(new string[] { });

            Assert.That(stringWriter, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void when_list_command_is_executed_then_a_database_connection_is_established()
        {
            var listCommand = new ListCommand(MockSecureConsole.Object, _mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object);

            listCommand.Execute(_requiredListCommandArguments);

            _mockConnectionFactory.Verify(m => m.Create(It.IsAny<DatabaseConnectionInfo>()), Times.Once);
        }

        [Test]
        public void when_list_command_is_executed_then_all_versions_are_fetched_from_database()
        {
            var listCommand = new ListCommand(MockSecureConsole.Object, _mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object);

            listCommand.Execute(_requiredListCommandArguments);

            _mockVersionRepository.Verify(m => m.GetAllVersions(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void when_database_contains_versions_then_they_are_listed_to_the_screen()
        {
            _mockVersionRepository.Setup(m => m.GetAllVersions(It.IsAny<string>()))
                .Returns(new List<DatabaseVersion> { new DatabaseVersion(1, "some script"), new DatabaseVersion(2, "some other script") });

            var listCommand = new ListCommand(MockSecureConsole.Object, _mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object);

            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                listCommand.Execute(_requiredListCommandArguments);

                Assert.That(stringWriter.ToString().Contains("  1\t\tsome script"));
                Assert.That(stringWriter.ToString().Contains("  2\t\tsome other script"));
            }
        }
    }
}