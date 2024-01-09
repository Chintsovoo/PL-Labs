namespace Lab2.Commands;

public class HelpCommand : AbstractCommand
{
    public HelpCommand() : base("help", "prints the list of commands and their description")
    {
    }

    public override double Execute(double a, double b)
    {
        throw new NotImplementedException();
    }
}