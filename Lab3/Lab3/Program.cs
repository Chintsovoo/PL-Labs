namespace Lab3;

internal static class Program
{
    public static void Main(string[] args)
    {
        var commandManager = new CommandManager();
        var dbContext = new ApplicationContext();
        var calculator = new Calculator(commandManager, dbContext);
        calculator.Run();
    }
}

