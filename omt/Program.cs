using System.Linq;
using omt.Commands;
using omt.Infrastructure;
using System;
using System.Reflection;
using TinyIoC;

namespace omt
{
    public class Program
    {
        internal class ExitCode
        {
            public const int Normal = 0;
            public const int NoCommand = 1;
            public const int BadCommand = 2;
        }

        public static void Main(string[] args)
        {
            if (!args.Any())
            {
                PrintHelp();

                Environment.Exit(ExitCode.NoCommand);
            }

            var commandArgument = args[0];
            var remainingArgs = args.Between(1).And(args.Length);
            var foundMatchingCommand = false;

            using (var container = TinyIoCContainer.Current)
            {
                container.AutoRegister(DuplicateImplementationActions.RegisterMultiple);

                var commands = container.ResolveAll<ICommand>();

                foreach (var command in commands)
                {
                    if (command.CommandText.Equals(commandArgument))
                    {
                        foundMatchingCommand = true;
                        command.Execute(remainingArgs);

                        break;
                    }
                }

                if (!foundMatchingCommand)
                {
                    Output.Text("Invalid command specified '{0}'".FormatWith(commandArgument));
                    Environment.Exit(ExitCode.BadCommand);
                }
            }

            Environment.Exit(ExitCode.Normal);
        }

        private static void PrintHelp()
        {
            Output.Info("omt {0}-beta".FormatWith(Assembly.GetExecutingAssembly().GetName().Version));
            Output.EmptyLine();
            Output.Info("Supported commands:");
            Output.Text("  list\t\t\tDisplay a list of existing versions.");
            Output.Text("  migrate\t\tRun a database migration.");
            Output.EmptyLine();
        }
    }
}
