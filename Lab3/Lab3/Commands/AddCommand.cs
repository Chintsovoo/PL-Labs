namespace Lab3.Commands;


public class AddCommand : AbstractCommand
{
    public AddCommand() : base("+", "Adds two numbers") {}

    public override double Execute(double a, double b)
    {
        return a + b;
    }
}