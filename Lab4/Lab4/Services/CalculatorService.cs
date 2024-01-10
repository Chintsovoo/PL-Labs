using Lab4.Models;
using Lab4.Repository;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Services;

public class CalculatorService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<int, double> _values = new Dictionary<int, double>();
    private int _step = 0;

    public CalculatorService(IServiceProvider provider)
    {
        _serviceProvider = provider;
         LoadData();
    }

    private void LoadData()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        var data = context.Set<CalculatorState>().ToList();
        foreach (var entry in data)
        {
            _values.Add(entry.Id, entry.Value);
        }
        _step = _values.Count;
    }

    public double GetValueAtStep(int step)
    {
        if (_values.ContainsKey(step))
            return _values[step]; 
        throw new KeyNotFoundException();
    }

    public Dictionary<int, double> GetAllValues()
    {
        return _values;
    }

    public void AddValue(double operand)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        _values[++_step] = operand;
        var state = new CalculatorState(_step, operand);
        context.Set<CalculatorState>().Add(state);
        context.SaveChanges();
    }

    public double Evaluate(double operand, char operation)
    {
        var result = operation switch
        {
            '+' => _values[_step] + operand,
            '-' => _values[_step] - operand,
            '*' => _values[_step] * operand,
            '/' => _values[_step] / operand,
            _ => throw new OperationNotFound()
        };
        AddValue(result);
        return result;
    }

}

public class OperationNotFound : Exception
{
}