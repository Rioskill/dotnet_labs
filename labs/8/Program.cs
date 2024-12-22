﻿class Program
{
    static int Main()
    {
        Console.WriteLine("Введите 2 числа");

        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Строка не может быть пустой");
            return -1;
        }

        try
        {
            decimal[] numbers = input.Split(' ', 2).Select(decimal.Parse).ToArray();
            if (numbers.Length < 2)
            {
                Console.WriteLine("Нужно ввести 2 числа");
                return -1;
            }
            Console.WriteLine(numbers[0] / numbers[1]);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Ошибка: деление на ноль");
            return -1;
        }
        catch (FormatException)
        {
            Console.WriteLine("Нужно ввести 2 числа");
            return -1;
        }
        catch (OverflowException)
        {
            Console.WriteLine("Введено слишком длинное число");
            return -1;
        }

        return 0;
    }
}
