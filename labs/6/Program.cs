class Program {
    delegate bool isSymbolInString(char c, string s);
    static void Main(string[] args) 
    {
        isSymbolInString strIncludes = (c, s) => s.Contains(c);

        Console.Write("Введите строку: ");
        string? s = Console.ReadLine();

        if (string.IsNullOrEmpty(s)) {
            Console.WriteLine("Строка не должна быть пустой");
            return;
        }

        Console.Write("Введите символ:");
        string? cStr = Console.ReadLine();

        if (string.IsNullOrEmpty(cStr)) {
            Console.WriteLine("Символ не должен быть пустым");
            return;
        }

        char c = cStr[0];

        if (strIncludes(c, s)) {
            Console.WriteLine("Символ присутствует в строке");
        } else {
            Console.WriteLine("Символ не присутствует в строке");
        }
    }
}
