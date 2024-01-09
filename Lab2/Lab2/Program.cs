namespace Lab2;

internal static class Program
{
    public static void Main(string[] args)
    {
        var commandManager = new CommandManager();

        var calculator = new Calculator(commandManager);

        calculator.Run();
    }
}

