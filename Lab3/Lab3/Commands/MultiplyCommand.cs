namespace Lab3.Commands;


public class MultiplyCommand : AbstractCommand
{
    public MultiplyCommand() : base("*","multiplies two commands") {}

    public override double Execute(double a, double b)
    {
        return a * b;
    }
}