using System.Data;

class Program {
    static int[] readNumbersLine(string line, char delimeter=' ') {
        return line.Split(delimeter).Select(size => Convert.ToInt32(size.Trim())).ToArray();
    }

    static void Main(string[] args) 
        {
            Console.Write("Введите размер матрицы (кол-во строк и столбцов через запятую): ");
            string? sizeS = Console.ReadLine();

            if (string.IsNullOrEmpty(sizeS)) {
                Console.WriteLine("Размер задан неверно!");
                return;
            }
            
            int[] size = readNumbersLine(sizeS, ',');

            if (size.Length != 2) {
                Console.WriteLine("Размер задан неверно!");
                return;
            }

            int[][] matrix = new int[size[0]][];

            Console.WriteLine("Введите матрицу:");
            for (int i = 0; i < size[0]; i++) {
                string? numS = Console.ReadLine();
                if (string.IsNullOrEmpty(numS)) {
                    Console.WriteLine("Строка не может быть пустой");
                    return;
                }

                int[] nums = readNumbersLine(numS);

                if (nums.Length < size[1]) {
                    Console.WriteLine("Столбцов меньше чем было задано!");
                    return;
                }

                if (nums.Length > size[1]) {
                    Console.WriteLine("Столбцов больше чем было задано!");
                    return;
                }

                matrix[i] = nums;
            }

            Console.Write("Введите через запятую номера строк матрицы, которые следует поменять местами: ");
            string? numbersS = Console.ReadLine();

            if (string.IsNullOrEmpty(numbersS)) {
                Console.WriteLine("Номера строк заданы неверно!");
                return;
            }

            int[] numbers = readNumbersLine(numbersS, ',');

            if (numbers.Length != 2 ||
                numbers[0] < 0 || numbers[0] >= size[0] ||
                numbers[1] < 0 || numbers[1] >= size[0]
            ) {
                Console.WriteLine("Номера строк заданы неверно!");
                return;
            }

            int[] t = matrix[numbers[0]];
            matrix[numbers[0]] = matrix[numbers[1]];
            matrix[numbers[1]] = t;

            Console.WriteLine("Получившаяся матрица:");
            for (int i = 0; i < size[0]; i++) {
                for (int j = 0; j < size[1]; j++) {
                    Console.Write(matrix[i][j].ToString());
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
}
