using System.Text.RegularExpressions;
using Lab2.Commands;

namespace Lab2
{
    public class Calculator
    {
        private double A { get; set; }
        private readonly CommandManager _commandManager;
        private int _current;
        private readonly Dictionary<long, double> _history;
        private const string OperandPrompt = ">";
        private const string OperationPrompt = "@:";
        private readonly Regex _regex;
        private const string RegexPattern = @"^#\d+$";

        public Calculator(CommandManager commandManager)
        {
            _current = 1;
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _history = new Dictionary<long, double>();
            _regex = new Regex(RegexPattern);
        }

        public void Run()
        {
            DisplayHelp();
            A = GetOperand();
            AddToHistory(A);

            while (true)
            {
                try
                {
                    var command = GetOperation();
                    var b = GetOperand();
                    A = command.Execute(A, b);
                    AddToHistory(A);
                }
                catch (ChangeValueToHistory e)
                {
                    HandleChangeValueToHistory(e);
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }
        }

        private void HandleChangeValueToHistory(ChangeValueToHistory e)
        {
            var target = Convert.ToInt32(e.Result.Substring(1));
            var current = _current;
            _current = target;
            A = _history.GetValueOrDefault(target);
            Console.WriteLine($"[#{_current}]={A}");
            _current++;

            for (var i = current; i > target; i--)
            {
                _history.Remove(i);
            }
        }

        private double GetOperand()
        {
            double result;
            while (true)
            {
                try
                {
                    Console.Write($"{OperandPrompt} ");
                    var temp = Console.ReadLine() ?? throw new Exception("input can not be null!");
                    if (temp.Equals("q")) QuitProgram();
                    result = Convert.ToInt32(temp);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("You must enter a valid number! Or q");
                }
            }
            return result;
        }

        private static void QuitProgram()
        {
            Console.WriteLine("Program exit requested!");
            Environment.Exit(0);
        }

        private void AddToHistory(double value)
        {
            Console.WriteLine($"[#{_current}]={value}");
            _history.Add(_current++, value);
        }

        private ICommand GetOperation()
        {
            string result;
            while (true)
            {
                try
                {
                    Console.Write($"{OperationPrompt} ");
                    result = Console.ReadLine() ?? throw new InvalidOperationException();
                    ValidateOperationInput(result);
                    break;
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Please insert one of the following: '+', '-', '/', '*', '#(number)', q");
                }
            }

            return _commandManager.GetCommandFromOperation(result);
        }

        private void ValidateOperationInput(string input)
        {
            if (input.Equals("q")) QuitProgram();
            if (!input.Trim().Equals("+") && !input.Trim().Equals("-") && !input.Trim().Equals("*") &&
                !input.Trim().Equals("/") && !input.Trim().StartsWith("#"))
            {
                throw new InvalidOperationException();
            }

            if (_regex.IsMatch(input))
            {
                throw new ChangeValueToHistory(input);
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Usage:\nwhen a first symbol on the line is ‘>’ – enter operand (number)\n" +
                              "when a first symbol on the line is ‘@’ – enter operation\n" +
                              "operation is one of ‘+’, ‘-‘, ‘/’, ‘*’ or\n" +
                              "‘#’ followed with the number of the evaluation step\n‘q’ to exit\n");
        }
    }

    internal class ChangeValueToHistory : Exception
    {
        public string Result { get; }

        public ChangeValueToHistory(string result)
        {
            Result = result;
        }
    }
}
