using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace soothsayer.Commands
{
    public class MigrateCommandOptions : IDatabaseCommandOptions
    {
        [Option('d', "down", Required = false, DefaultValue = false,
             HelpText = "Executes the rollback scripts instead of the forward scripts. By default, migrations are run in roll-forward mode.")]
        public bool Down { get; set; }

        [Option('c', "connection", Required = true,
          HelpText = "The data source connection string for connecting to the target Oracle instance.")]
        public string ConnectionString { get; set; }

        [Option('s', "schema", Required = true,
          HelpText = "The oracle schema in which the version tables reside. Most likely the same schema as the tables being migrated.")]
        public string Schema { get; set; }

        [Option("tablespace", Required = false,
          HelpText = "The oracle tablespace in which the version tables should reside. By default the schema name will be used if this is not specified.")]
        public string Tablespace { get; set; }

        [Option('u', "username", Required = true,
          HelpText = "The username to use to connect to target Oracle instance.")]
        public string Username { get; set; }

		[Option('p', "password", Required = false,
			HelpText = "The password for connecting to the target Oracle instance. If not provided in the commandline then you will be prompted to enter it in.")]
		public string Password { get; set; }

        [Option('i', "input", Required = true,
			HelpText = "The input folder containing both the roll-forward (up) and roll-back (down) sql scripts.")]
        public string InputFolder { get; set; }

        [OptionList('e', "environment", Required = false, Separator = ',',
          HelpText = "The environment of the target Oracle instance. This enables running of environment specific scripts. More than one environment can be specified, separated by a comma.")]
        public IList<string> Environment { get; set; }

        [Option('v', "version", Required = false,
			HelpText = "The target database version to migrate up (or down) to. Migration will stop if the next script will bring the database to a higher version than specified here (or lower in the case of roll-backs).")]
        public long? Version { get; set; }

        [Option('y', "noprompt", Required = false, DefaultValue = false,
            HelpText = "The target database version to migrate up (or down) to. Migration will stop if the next script will bring the database to a higher version than specified here (or lower in the case of roll-backs).")]
        public bool NoPrompt { get; set; }

        [Option("concise", Required = false,
            HelpText = "Suppresses verbose information (such as SqlPlus output)")]
        public bool Concise { get; set; }

        [Option("usestored", Required = false, DefaultValue = false, MutuallyExclusiveSet = "downgrade",
            HelpText = "Tells soothsayer to ignore the down migration script files and use the stored scripts in the target database schema.")]
        public bool UseStored { get; set; }

        [Option("force", Required = false, DefaultValue = false,
            HelpText = "Tells soothsayer to ignore any errors from executing scripts within SQL*Plus and continue execution of all the scripts.")]
        public bool Force { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage() {
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
    }
}
