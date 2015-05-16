using CommandLine;
using CommandLine.Text;

namespace omt
{
    public class ListCommandOptions : IDatabaseCommandOptions
    {
        [Option('c', "connection", Required = true,
          HelpText = "The data source connection string for connecting to the target Oracle instance.")]
        public string ConnectionString { get; set; }

        [Option('s', "schema", Required = true,
          HelpText = "The oracle schema in which the version tables reside. Most likely the same schema as the tables being migrated.")]
        public string Schema { get; set; }

        [Option('u', "username", Required = true,
          HelpText = "The username to use to connect to target Oracle instance.")]
        public string Username { get; set; }

		[Option('p', "password", Required = false,
			HelpText = "The password for connecting to the target Oracle instance. If not provided in the commandline then you will be prompted to enter it in.")]
		public string Password { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage() {
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
    }
}
