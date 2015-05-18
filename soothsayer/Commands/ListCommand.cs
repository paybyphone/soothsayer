using System.Linq;
using System.Reflection;
using CommandLine;
using soothsayer.Infrastructure;
using soothsayer.Oracle;

namespace soothsayer.Commands
{
    public class ListCommand : DatabaseCommand<ListCommandOptions>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IVersionRespositoryFactory _versionRespositoryFactory;

        public ListCommand(ISecureConsole secureConsole, IConnectionFactory connectionFactory, IVersionRespositoryFactory versionRespositoryFactory)
            : base(secureConsole)
        {
            _connectionFactory = connectionFactory;
            _versionRespositoryFactory = versionRespositoryFactory;
        }

        public override string CommandText
        { 
            get { return "list"; }
        }

        public override string Description
        { 
            get { return "Display a list of existing versions."; }
        }

        protected override void ExecuteCore(string[] arguments)
        {
            var listCommandOptions = Options;
            if (Parser.Default.ParseArguments(arguments, listCommandOptions))
            {
                Output.Info("soothsayer {0}-beta".FormatWith(Assembly.GetExecutingAssembly().GetName().Version));
                Output.EmptyLine();

                string password = GetOraclePassword(listCommandOptions);

                var databaseConnectionInfo = new DatabaseConnectionInfo(listCommandOptions.ConnectionString, listCommandOptions.Username, password);

                using (var connection = _connectionFactory.Create(databaseConnectionInfo))
                {
                    var oracleVersionRepository = _versionRespositoryFactory.Create(connection);

                    var versions = (oracleVersionRepository.GetAllVersions(listCommandOptions.Schema) ?? Enumerable.Empty<DatabaseVersion>()).ToArray();

                    if (versions.Any())
                    {
                        Output.Info("Found the following versions in schema '{0}' (ascending order)".FormatWith(listCommandOptions.Schema));
                        Output.EmptyLine();

                        foreach (var version in versions)
                        {
                            Output.Text("  {0}\t\t{1}".FormatWith(version.Version, version.ScriptName));
                        }

                        Output.EmptyLine();
                    }
                }
            }
        }
    }
}
