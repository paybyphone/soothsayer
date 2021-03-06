﻿using System.Text;
using ColoredConsole;

namespace soothsayer.Infrastructure.IO
{
    public static class Output
    {
        public static IColorConsole ConsoleProvider = new ColorConsoleAdapter();
        public static bool Concise = false;

        public static void Text(string message)
        {
            Text(message, 0);
        }

        public static void Text(string message, int indent)
        {
            ConsoleProvider.WriteLine((Spaces(indent) + message).DarkGray());
        }

        public static void Verbose(string message)
        {
            Verbose(message, 0);
        }
        public static void Verbose(string message, int indent)
        {
            if (!Concise)
            {
                ConsoleProvider.WriteLine(Spaces(indent) + message);
            }
        }

        public static void Info(string message)
        {
            Info(message, 0);
        }

        public static void Info(string message, int indent)
        {
            ConsoleProvider.WriteLine((Spaces(indent) + message).Green());
        }

        public static void Error(string message)
        {
            ConsoleProvider.WriteLine(message.Red());
        }

        public static void Warn(string message)
        {
            Warn(message, 0);
        }

        public static void Warn(string message, int indent)
        {
            ConsoleProvider.WriteLine((Spaces(indent) + message).Yellow());
        }

        public static void EmptyLine()
        {
            ConsoleProvider.WriteLine();
        }

        internal static string Spaces(int indent)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < indent; i++)
            {
                stringBuilder.Append("    ");
            }

            return stringBuilder.ToString();
        }
    }
}
