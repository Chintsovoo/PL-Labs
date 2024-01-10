namespace Lab3.Commands;


public class DivideCommand : AbstractCommand
{
    public DivideCommand() : base("/","divides two commands"){}

    public override double Execute(double a, double b)
    {
        return a / b;
    }
}