class CustomAttribute : Attribute { }

class ReflectionExample
{
    public int intField;
    public string strField = "";

    [Custom]
    public int intFieldWithAttribute = 0;

    public ReflectionExample(int intFieldValue)
    {
        strField = "field";
        intField = intFieldValue;
    }

    public void Method(string str)
    {
        Console.WriteLine(str);
    }
}

class Program
{
    static int Main()
    {
        var type = typeof(ReflectionExample);
        Console.WriteLine("Конструкторы:");

        foreach (var elem in type.GetConstructors())
        {
            Console.WriteLine(elem);
        }

        Console.WriteLine("Методы:");

        foreach (var elem in type.GetMethods())
        {
            Console.WriteLine(elem);
        }

        Console.WriteLine("Поля:");

        foreach (var elem in type.GetFields())
        {
            Console.WriteLine(elem);
        }

        Console.WriteLine("Поля с аттрибутами:");

        foreach (var elem in type.GetFields())
        {
            if (Attribute.IsDefined(elem, typeof(CustomAttribute)))
            {
                Console.WriteLine(elem);
            }
        }
        Console.WriteLine();

        type.GetMethod("Method")?.Invoke(new ReflectionExample(14), ["Hello"]);
        return 0;
    }
}