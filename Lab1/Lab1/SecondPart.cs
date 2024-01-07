using System.Data;

namespace Lab1;
/*Вариант 11
    Уплотнить заданную матрицу, удаляя из нее строки и столбцы, заполненные нулями.     
    Найти номер первой из строк, содержащих хотя бы один положительный элемент. */

public class SecondPart
{
    int[,] Array { get; set; }
    
    public SecondPart(int rowSize, int columnSize)
    {
        if (rowSize <= 0 || columnSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rowSize) + " or " + nameof(columnSize));
        }
        
        Array = new int[rowSize, columnSize];
        
        var random = new Random();
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < columnSize; j++)
            {
                Array[i,j] = random.Next(-10, 10);
            }
        }
    }

    public SecondPart(int[,] array)
    {
        Array = array;
    }
    
    public void PrintArray()
    {
        for (int i = 0; i < Array.GetLength(0); i++)
        {
            for (int j = 0; j < Array.GetLength(1); j++)
            {
                Console.Write(Array[i,j] + "\t");
            }
            Console.WriteLine();
        }
    }
    
    public void RemoveZeroes()
    {
        RemoveZeroesRow();
        RemoveZeroesColumn();
    }
    
    void RemoveZeroesRow()
    {
        for (int i = 0; i < Array.GetLength(0); i++)
        {
            bool onlyZeroes = true;
            for (int j = 0; j < Array.GetLength(1); j++)
            {
                if (Array[i, j] == 0) continue;
                onlyZeroes = false;
                break;
            }
            if (onlyZeroes) RemoveRow(i);
        }
            
    }

    void RemoveZeroesColumn()
    {
        for (int i = 0; i < Array.GetLength(1); i++)
        {
            bool onlyZeroes = true;
            for (int j = 0; j < Array.GetLength(0); j++)
            {
                if (Array[j, i] == 0) continue;
                onlyZeroes = false;
                break;
            }
            if (onlyZeroes) RemoveColumn(i);
        }
    }

    void RemoveRow(int index)
    {
        int rowSize = Array.GetLength(0);
        int columnSize = Array.GetLength(1);
        int[,] newArray = new int[rowSize - 1, columnSize];
        for (int i = 0; i < rowSize; i++)
        {
            if (i == index) continue;
            for (int j = 0; j < columnSize; j++)
            {
                if (i < index)
                {
                    newArray[i, j] = Array[i, j];
                }
                else
                {
                    newArray[i - 1,j] = Array[i,j];
                }
            }
        }
        Array = newArray;
    }

    void RemoveColumn(int index)
    {
        int rowSize = Array.GetLength(0);
        int columnSize = Array.GetLength(1);

        int[,] newArray = new int[rowSize, columnSize-1];

        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < columnSize - 1; j++)
            {
                if (j < index)
                {
                    newArray[i, j] = Array[i, j];
                }
                else
                {
                    newArray[i, j] = Array[i, j + 1];
                }
            }
        }
        Array = newArray;
    }

    public int FindIndexOfRowWithFirstPositiveNumber()
    {
        bool hasFound = false;
        int index = 0;
        
        for (int i = 0; i < Array.GetLength(0); i++)
        {
            for (int j = 0; j < Array.GetLength(1); j++)
            {
                if (Array[i, j] <= 0) continue;
                index = i;
                hasFound = true;
                break;
            }
            if (hasFound) break;
        }

        return index;
    }
}

