class Program {
    static void Main(string[] args) 
    {
        Console.Write("Введите минимальную сумму: ");
        string? s = Console.ReadLine();

        if (string.IsNullOrEmpty(s)) {
            return;
        }

        int minSum = Convert.ToInt32(s);

        FileStream rfs = new FileStream("source.txt", FileMode.Open);
        FileStream wfs = new FileStream("target.txt", FileMode.Create);

        using (StreamReader reader = new StreamReader(rfs)) {
            var lines = reader.ReadToEnd().Split('\n');

            var acceptableLines = lines
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => line.Split(':').Select(word => word.Trim()).ToArray())
                .Select(line => (name: line[0], price: Convert.ToInt32(line[1])))
                .Where(item => item.price >= minSum);

            using (StreamWriter writer = new StreamWriter(wfs)) {
                foreach(var line in acceptableLines) {
                    writer.WriteLine(line.Item1 + ": " + line.Item2);
                }
            }
        }
    }
}
