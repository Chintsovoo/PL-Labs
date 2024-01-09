
using Lab2.Commands;

namespace Lab2;

public class CommandManager
{
    private readonly List<AbstractCommand> _commands;

    public CommandManager()
    {
        _commands = new List<AbstractCommand>();
        var add = new AddCommand();
        var minus = new MinusCommand();
        var multiply = new MultiplyCommand();
        var divide = new DivideCommand();
        _commands.Add(add);
        _commands.Add(minus);
        _commands.Add(multiply);
        _commands.Add(divide);
    }


    public ICommand GetCommandFromOperation(string operation)
    {
        foreach (var command in _commands.Where(command => command.Name.Equals(operation)))
        {
            return command;
        }
        throw new NoSuchCommandException();
    }
}

public class NoSuchCommandException : Exception
{
}