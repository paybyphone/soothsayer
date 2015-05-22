using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using soothsayer.Commands;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using TinyIoC;

namespace soothsayer
{
	public class Program
	{
		internal static class ExitCode
		{
			public static readonly int Normal = 0;
			public static readonly int NoCommand = 1;
			public static readonly int BadCommand = 2;
		}

		public static void Main(string[] args)
		{
			using (var container = TinyIoCContainer.Current)
			{
				container.AutoRegister(DuplicateImplementationActions.RegisterMultiple);

				var supportedCommands = container.ResolveAll<ICommand>();

				if (!args.Any())
				{
					PrintHelp(supportedCommands);

					Environment.Exit(ExitCode.NoCommand);
				}

				var commandArgument = args.First();
				var remainingArgs = args.Between(1).And(args.Length);
				var foundMatchingCommand = false;

				foreach (var command in supportedCommands)
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

		private static void PrintHelp(IEnumerable<ICommand> supportedCommands)
		{
			Output.Info("soothsayer {0}-beta".FormatWith(Assembly.GetExecutingAssembly().GetName().Version));
			Output.EmptyLine();
			Output.Info("Supported commands:");

			foreach (var supportedCommand in supportedCommands)
			{
				Output.Text("  {0}\t\t\t{1}".FormatWith(supportedCommand.CommandText, supportedCommand.Description));
			}

			Output.EmptyLine();
		}
	}
}
