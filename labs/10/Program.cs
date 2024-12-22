class Program
{
    class Product(string name, string weight) : IComparable<Product>
    {
        decimal weightKg = ParseWeight(weight);
        string weight = weight;
        string name = name;

        static decimal ParseWeight(string weight)
        {
            string weightValue = "";
            int i = 0;
            char ch = weight[i];

            if (ch == '-')
            {
                throw new ArgumentOutOfRangeException("value can not be negative");
            }

            while (ch >= '0' && ch <= '9' && i < weight.Length)
            {
                weightValue += ch;
                ++i;
                ch = weight[i];
            }

            if (weightValue.Length < 0)
            {
                throw new ArgumentOutOfRangeException("value should start with a number");
            }

            decimal weightNum = decimal.Parse(weightValue);
            if (i >= weight.Length)
            {
                return weightNum;
            }

            string? dim = weight[i..];
            double multiplier = dim switch
            {
                "т" => 1000,
                "г" => 0.001,
                _ => 1,
            };
            return weightNum * (decimal)multiplier;
        }

        public int CompareTo(Product? other)
        {
            if (other == null)
            {
                return 1;
            }

            if (weightKg == other.weightKg)
            {
                return 0;
            }

            if (weightKg < other.weightKg)
            {
                return -1;
            }
            return 1;
        }

        override public string ToString()
        {
            return weight + " " + name;
        }
    }

    public static int Main()
    {
        var products = File.ReadAllLines("products.txt")
                        .Select(line => line.Split())
                        .Select(parts => new Product(parts[1], parts[0]))
                        .ToList();

        products.Sort();

        products.ForEach(Console.WriteLine);

        return 0;
    }
}
