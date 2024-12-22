class Program
{
    class Point
    {
        public double X { get;}
        public double Y { get;}

        public override string ToString()
        {
            return string.Format("{{{0:0.00}, {1:0.00}}}", X, Y);
        }

        public Point(double x, double y) {
            X = x;
            Y = y;
        }
    }

    static List<Point> RandomPoints()
    {
        List<Point> result = [];

        for (int i = 0; i < 5; ++i)
        {
            result.Add(new Point(
                Random.Shared.NextDouble(),
                Random.Shared.NextDouble()
            ));
        }

        return result;
    }

    class DistanceComparer : IComparer<Point>
    {
        public int Compare(Point? a, Point? b)
        {
            if (a == null && b == null)
            {
                return 0;
            }
            if (a == null)
            {
                return -1;
            }
            if (b == null)
            {
                return 1;
            }

            return Comparer<double>.Default.Compare(a.X * a.X + a.Y * a.Y, b.X * b.X + b.Y * b.Y);
        }
    }
    class XComparer : IComparer<Point>
    {
        public int Compare(Point? a, Point? b)
        {
            if (a == null && b == null)
            {
                return 0;
            }
            if (a == null)
            {
                return -1;
            }
            if (b == null)
            {
                return 1;
            }

            return Comparer<double>.Default.Compare(a.X, b.X);
        }
    }

    class YComparer : IComparer<Point>
    {
        public int Compare(Point? a, Point? b)
        {
            if (a == null && b == null)
            {
                return 0;
            }
            if (a == null)
            {
                return -1;
            }
            if (b == null)
            {
                return 1;
            }

            return Comparer<double>.Default.Compare(a.Y, b.Y);
        }
    }

    class DiagComparer : IComparer<Point>
    {
        public int Compare(Point? a, Point? b)
        {
            if (a == null && b == null)
            {
                return 0;
            }
            if (a == null)
            {
                return -1;
            }
            if (b == null)
            {
                return 1;
            }

            return Comparer<double>.Default.Compare(Math.Abs(a.Y - a.X), Math.Abs(b.Y - b.X));
        }
    }

    public static int Main()
    {
        var points = RandomPoints();

        Console.WriteLine("По удалению от начала коодринат:");
        points.Sort(new DistanceComparer());
        points.ForEach(Console.WriteLine);

        Console.WriteLine("\nПо удалению от оси X:");
        points.Sort(new XComparer());
        points.ForEach(Console.WriteLine);

        Console.WriteLine("\nПо удалению от оси Y:");
        points.Sort(new YComparer());
        points.ForEach(Console.WriteLine);

        Console.WriteLine("\nПо удалению от диагонали y = x:");
        points.Sort(new DiagComparer());
        points.ForEach(Console.WriteLine);

        return 0;
    }
}
