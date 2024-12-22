﻿using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Shop;

namespace Shop
{
    public class Customer(string name, string address, decimal discount)
    {
        public string name = name;
        public string address = address;
        public decimal discount = discount;
    }

    public class Product(string name, decimal price)
    {
        public string name = name;
        public decimal price = price;
    }

    public class OrderLine(Product product, int num)
    {
        public Product product = product;
        public int num = num;
    }

    public class Order(int id, Customer customer, decimal price, List<OrderLine> lines)
    {
        public int id = id;
        public Customer customer = customer;
        public decimal discount = customer.discount;
        public decimal price = price;
        public List<OrderLine> lines = lines;
    }

    public class Database(Dictionary<string, Product> products)
    {
        public Dictionary<string, Product> products = products;

        static public Database Deserialize(string data)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, Product>>(data) ?? throw new JsonException();
            return new Database(result);
        }
    }
}

class Program
{
    static int Main()
    {
        var fileData = File.ReadAllText("products.json");

        var db = Database.Deserialize(fileData);

        Console.WriteLine("Введите данные покупателя: <имя> <адрес> <скидка>: ");
        string[]? customerParts = Console.ReadLine()?.Split();
        if (customerParts == null)
        {
            Console.WriteLine("Не удалось прочитать данные");
            return -1;
        }

        Customer customer = new(customerParts[0], customerParts[1], decimal.Parse(customerParts[2]));

        List<OrderLine> orderLines = [];

        string? input;
        do {
            input = Console.ReadLine();

            var parts = input?.Split();

            if (parts == null || parts.Length < 2) {
                continue;
            }

            orderLines.Add(new OrderLine(db.products[parts[0]], Convert.ToInt32(parts[1])));
        } while (input != "end");

        var price = orderLines.Aggregate((decimal)0, (sum, line) => sum + line.num * db.products[line.product.name].price) * (1 - customer.discount);

        var order = new Order(Random.Shared.Next(), customer, price, orderLines);

        Console.WriteLine("Введите название заказа:");
        string? orderName = Console.ReadLine();

        if (string.IsNullOrEmpty(orderName)) {
            Console.WriteLine("Название заказа не может быть пустым");
            return -1;
        }

        using var resultFile = File.Open(orderName + ".json", FileMode.Create);
        resultFile.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(order)));
        return 0;
    }
}