namespace Lab2.Commands;

public class MinusCommand : AbstractCommand
{
    public MinusCommand() : base("-", "subtract two numbers")
    {
    }

    public override double Execute(double a, double b)
    {
        return a - b;
    }
}