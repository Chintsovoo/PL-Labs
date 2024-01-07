namespace Lab1;
/* Вариант 11
    В одномерном массиве, состоящем из N вещественных элементов, вычислить:
        - номер максимального по модулю элемента массива
        - сумму элементов массива, расположенных после первого положительного элемента.
    Преобразовать массив таким образом, чтобы сначала располагались все элементы, целая часть которых лежит в интервале [а, b], а потом — все остальные. */

public class FirstPart
{
    double[] Array { get; set; }

    public FirstPart(int length)    
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }
        
        Array = new double[length];
        
        var random = new Random();
        for (int i = 0; i < Array.Length; i++)
        {
            Array[i] = Math.Truncate((random.NextDouble() * (10 - (-10)) + (-10)) * 1000d) / 1000d;
        }
    }
    
    public int GetMaxAbsNumberIndex()
    {
        double max = Math.Abs(Array[0]);
        int indexOfMax = 0;
        
        for (int i = 1; i < Array.Length; i++)
        {
            if (Math.Abs(Array[i]) > max)
            {
                max = Math.Abs(Array[i]);
                indexOfMax = i;
            }
        }

        return indexOfMax;
    }

    public double SumOfElementsAfterPositive()
    {
        double sum = 0;
        int indexOfFristPostiveElements = 0;
        
        for (int i = 0; i < Array.Length; i++)
        {
            if (Array[i] > 0)
            {
                indexOfFristPostiveElements = i;
                break;
            }
        }

        for (int i = indexOfFristPostiveElements + 1; i < Array.Length; i++)
        {
            sum += Array[i];
        }

        return sum;
    }

    public void PrintArray()
    {
        foreach (double t in Array)
        {
            Console.Write(t + " ");
        }
        Console.WriteLine();
    }

    public void makeArrayDifferent(int a, int b)
    {
        double[] newArray = new double[Array.Length];
        int firstindex = 0;
        int lastindex = Array.Length - 1;
        
        for (int i = 0; i < Array.Length; i++)
        {
            int tselayaChasti = (int)Array[i];
            if (a <= tselayaChasti && tselayaChasti <= b)
            {   
                newArray[firstindex] = Array[i];
                firstindex++;
            }
            else
            {
                newArray[lastindex] = Array[i];
                lastindex--;
            }
        }
        
        Array = newArray;
    }
    
}