using Lab1;
/* Вариант 11
    ЧАСТЬ 1:
    В одномерном массиве, состоящем из N вещественных элементов, вычислить:
        - номер максимального по модулю элемента массива
        - сумму элементов массива, расположенных после первого положительного элемента.
    Преобразовать массив таким образом, чтобы сначала располагались все элементы, целая часть которых лежит в интервале [а, b], а потом — все остальные. 
    
    ЧАСТЬ 2:
    Уплотнить заданную матрицу, удаляя из нее строки и столбцы, заполненные нулями.     
    Найти номер первой из строк, содержащих хотя бы один положительный элемент. */


class Program
{
    static void Main(string[] args)
    {
        // ЧАСТЬ 1
        Console.WriteLine("Часть 1:");
        
        Console.Write("Введите размер массива: ");
        
        int size = int.Parse(Console.ReadLine());
        
        var firstPart = new FirstPart(size);
        
        Console.WriteLine("Исходный массив: ");
        firstPart.PrintArray();
        
        Console.WriteLine("Номер максимального по модулю элемента массива: " + firstPart.GetMaxAbsNumberIndex());
        Console.WriteLine("Сумма элементов массива, расположенных после первого положительного элемента: " + firstPart.SumOfElementsAfterPositive());
        
        Console.Write("Введите а: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите b: ");
        int b = int.Parse(Console.ReadLine());
        
        firstPart.makeArrayDifferent(a,b);
        Console.Write("Переобразование массива: ");
        firstPart.PrintArray();
        
        
        // ЧАСТЬ 2
        Console.WriteLine("Часть 2: ");
        
        Console.Write("Введите строка массива: ");
        int row = int.Parse(Console.ReadLine());
        Console.Write("Введите столбец массива: ");
        int column = int.Parse(Console.ReadLine());

        var secondPart = new SecondPart(row, column);
        
        Console.WriteLine("Before");
        secondPart.PrintArray();
        
        secondPart.RemoveZeroes();
        Console.WriteLine("After");
        secondPart.PrintArray();

        Console.WriteLine("Индекс строки первого позитивного элемента: " + secondPart.FindIndexOfRowWithFirstPositiveNumber());
    }
}
