using System.Collections.Generic;

namespace soothsayer.Commands
{
    public interface ICommand<out T> : ICommand
        where T : IOptions, new()
    {
        T Options { get; }
    }

    public interface ICommand
    {
        string CommandText { get; }
        string Description { get; }
		void Execute(IEnumerable<string> arguments);
    }
}
