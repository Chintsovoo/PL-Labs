namespace Lab2.Commands;

public abstract class AbstractCommand : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public abstract double Execute(double a, double b);

    protected AbstractCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }

}