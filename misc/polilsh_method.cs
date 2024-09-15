using System;
using System.Runtime.Intrinsics.X86;

namespace CalculatorNS
{
    class Calculator
    {
        static Dictionary<string, Dictionary<string, int>> ops_table = new Dictionary<string, Dictionary<string, int>>
            {
                {
                    "start", new Dictionary<string, int>{
                        {"+", 1},
                        {"*", 1},
                        {"(", 1},
                        {")", -1},
                        {"end", 0}
                    }
                },
                {
                    "+", new Dictionary<string, int>{
                        {"+", 2},
                        {"*", 1},
                        {"(", 1},
                        {")", 4},
                        {"end", 4}
                    }
                },
                {
                    "*", new Dictionary<string, int>{
                        {"+", 4},
                        {"*", 2},
                        {"(", 1},
                        {")", 4},
                        {"end", 4}
                    }
                },
                {
                    "(", new Dictionary<string, int>{
                        {"+", 1},
                        {"*", 1},
                        {"(", 1},
                        {")", 3},
                        {"end", -1}
                    }
                },
            };

        static Dictionary<string, Func<int, int, int>> ops_funcs = new Dictionary<string, Func<int, int, int>> {
            {
                "+", (a, b) => a + b
            },
            {
                "*", (a, b) => a * b
            },
        };

        private int i;
        private Stack<string> ops_stack;
        private List<string> res_list;

        public Calculator() {
            this.ops_stack = new Stack<string>();
            ops_stack.Push("start");

            this.res_list = new List<string>();
        }

        private string ReadNumber(string s) {
            int j = 0;
            while (i + j < s.Length && Char.IsDigit(s[i + j])) {
                j++;
            }

            i += j;
            return s.Substring(i - j, j);
        }

        public int Calculate(string s)
        {
            for (this.i = 0; this.i <= s.Length;) {
                string symbol;
                if (i == s.Length) {
                    symbol = "end";
                } else {
                    symbol = Char.ToString(s[i]);
                }

                if (i < s.Length && Char.IsDigit(s[this.i])) {
                    res_list.Add(ReadNumber(s));
                } else if (symbol == " ") {
                    i++;
                } else {
                    int op_code = ops_table[ops_stack.Peek()][symbol];

                    if (op_code == 0) {
                        break;
                    } else if (op_code == 1) {
                        ops_stack.Push(symbol);
                        i++;
                    } else if (op_code == 2) {
                        res_list.Add(ops_stack.Peek());
                        ops_stack.Push(symbol);
                        i++;
                    } else if (op_code == 3) {
                        ops_stack.Pop();
                        i++;
                    } else if (op_code == 4) {
                        res_list.Add(ops_stack.Pop());
                    } else if (op_code == 0) {
                        break;
                    }
                }
            }

            Stack<string> ops = new Stack<string>();
            Stack<int> nums = new Stack<int>();

            res_list.ForEach(x => {
                if (x.All(char.IsDigit)) {
                    nums.Push(Convert.ToInt32(x));
                } else {
                    ops.Push(x);   
                }
            });

            while (ops.Count > 0) {
                int first = nums.Pop();
                int second = nums.Pop();

                string op = ops.Pop();

                nums.Push(ops_funcs[op](first, second));
            }

            return nums.Pop();
        }
    }

    class Program
    {
        static void Main(string[] args) 
        {
            Calculator calc = new Calculator();
            
            Console.Write("Input: ");
            string? s = Console.ReadLine();

            if (string.IsNullOrEmpty(s)) {
                return;
            }

            Console.WriteLine(calc.Calculate(s));
        }
    }
}
