using System.Diagnostics;
using System.Text.RegularExpressions;
using Lab3.Commands;
using Lab3.entity;
using Microsoft.EntityFrameworkCore;

namespace Lab3
{
    public partial class Calculator
    {
        private double? A { get; set; }
        
        private readonly CommandManager _commandManager;
        private Dictionary<long, double> _history;
        private readonly Regex _regex = MyRegex();
        private readonly DbContext _db;
        
        private int _current = 1;
        private const string OperandPrompt = ">";
        private const string RegexPattern = @"^#\d+$";
        private const string OperationPrompt = "@:";

        public Calculator(CommandManager commandManager, DbContext dbContext)
        {
            A = null;
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            LoadChanges();
            _history ??= new Dictionary<long, double>();
        }

        private void LoadChanges()
        {
            while (true)
            {
                Console.WriteLine("Load from saves? y/n");
                var result = Console.ReadLine();
                if (result == null || (!result.Equals("y") && !result.Equals("n")))
                {
                    Console.WriteLine("Please write y or n");
                    continue;
                }
                if (result.Equals("n"))
                {
                    DeleteDataFromDatabase();
                    return;
                }
                if (result.Equals("y")) break;
            }

            var done = false;
            while (!done)
            {
                Console.WriteLine("Choose the source: (input the number) \n1) database\n2) json\n3) xml\nIf you choose other than database the database data will be overwritten");
                var result = Console.ReadLine();
                if (result == null || (!result.Equals("1") && !result.Equals("2") && !result.Equals("3")))
                {
                    Console.WriteLine("Please write 1, 2 or 3");
                    continue;
                }
                switch (result)
                {
                    case "1":
                        done = true;
                        LoadFromDatabase();
                        break;
                    case "2":
                        done = true;
                        LoadFromJson();
                        break;
                    case "3":
                        done = true;
                        LoadFromXml();
                        break;
                    default:
                        continue;
                }
            }
            
        }
        
        public void Run()
        {
            DisplayHelp();
            if (A == null)
            {
                A = GetOperand();
                AddToHistory(A.GetValueOrDefault(0));
            }
            else
            {
                Console.WriteLine($"[#{_current-1}]={A}");
            }

            while (true)
            {
                try
                {
                    var command = GetOperation();
                    var b = GetOperand();
                    A = command.Execute(A.GetValueOrDefault(), b);
                    AddToHistory(A.GetValueOrDefault());
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
            var target = Convert.ToInt32(e.Result[1..]);
            var current = _current;
            _current = target;
            A = _history.GetValueOrDefault(target);
            Console.WriteLine($"[#{_current}]={A}");
            _current++;
            var list = _db.Set<CalculatorStateEntity>().ToList();

            for (var i = current-1; i > target; i--)
            {
                _db.Remove(list[i-1]);
                _history.Remove(i);
            }
            _db.SaveChanges();
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
                    switch (temp)
                    {
                        case "q":
                            QuitProgram();
                            break;
                        case "save":
                            SaveToFile();
                            break;
                    }
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
            CalculatorStateEntity entity = new CalculatorStateEntity(_current, value);
            _db.Set<CalculatorStateEntity>().Add(entity);
            _db.SaveChanges();
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
            switch (input)
            {
                case "q":
                    QuitProgram();
                    break;
                case "save":
                    SaveToFile();
                    break;
            }
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

        private void SaveToFile()
        {
            while (true)
            {
                Console.WriteLine("Where do you want to save? (input the number)\n1) json\n2) xml");
                var result = Console.ReadLine();
                if (result == null || (!result.Equals("1") && !result.Equals("2")))
                {
                    Console.WriteLine("Please input 1 or 2");
                    continue;
                }

                if (result.Equals("1"))
                {
                    SaveToJson();
                    break;
                }
                if (result.Equals("2"))
                {
                    SaveToXml();
                    break;
                }
            }            
        }

        private void SaveToXml()
        {
            var dto = new CalculatorFileDTO
            {
                Current = _current,
                History = _history
            };
            LocalSaver.SaveToXml(dto);
        }

        private void SaveToJson()
        {
            var dto = new CalculatorFileDTO
            {
                Current = _current,
                History = _history
            };
            LocalSaver.SaveToJson(dto);
        }

        private void LoadFromDatabase()
        {
            var historyEntries = _db.Set<CalculatorStateEntity>().ToList();
            var historyCount = historyEntries.Count;
            _current = historyCount;
            _history = historyEntries.ToDictionary(obj => (long) obj.Id, obj => obj.A);
            A = _history.GetValueOrDefault(_current);
            _current++;
        }
        
        private void LoadFromJson()
        {
            DeleteDataFromDatabase();
            var file = LocalSaver.ReadFromJson<CalculatorFileDTO>();
            _current = file.Current;
            foreach (var entry in file.History)
            {
                _history.Add(entry.Key, entry.Value);
            }
            A = _history.GetValueOrDefault(_current);
            SetNewDatabaseData();
        }
        
        private void LoadFromXml()
        {
            DeleteDataFromDatabase();
            var file = LocalSaver.ReadFromXml<CalculatorFileDTO>();
            _current = file.Current;
            foreach (var entry in file.History)
            {
                _history.Add(entry.Key, entry.Value);
            }
            A = _history.GetValueOrDefault(_current);
            SetNewDatabaseData();
        }

        private void DeleteDataFromDatabase()
        {
            var historyEntries = _db.Set<CalculatorStateEntity>().ToList();
            foreach (var entry in historyEntries)
            {
                _db.Remove(entry);
            }
            _db.SaveChanges();
        }

        private void SetNewDatabaseData()
        {
            foreach (var entry in _history)
            {
                var entity = new CalculatorStateEntity((int) entry.Key, entry.Value);
                _db.Set<CalculatorStateEntity>().Add(entity);
            }
            _db.SaveChanges();
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Usage:\nwhen a first symbol on the line is ‘>’ – enter operand (number)\n" +
                              "when a first symbol on the line is ‘@’ – enter operation\n" +
                              "operation is one of ‘+’, ‘-‘, ‘/’, ‘*’ or\n" +
                              "‘#’ followed with the number of the evaluation step\n‘q’ to exit\n");
        }

        [GeneratedRegex(RegexPattern)]
        private static partial Regex MyRegex();
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
