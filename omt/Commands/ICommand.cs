﻿using System.Collections.Generic;

namespace omt.Commands
{
    public interface ICommand<out T> : ICommand
        where T : IOptions, new()
    {
        T Options { get; }
    }

    public interface ICommand
    {
        string CommandText { get; }
		void Execute(IEnumerable<string> arguments);
    }
}