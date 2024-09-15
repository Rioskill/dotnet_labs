using System;
using System.Runtime.Intrinsics.X86;

namespace CalculatorNS
{
    public class Calculator
    {
        // -1: Error
        //  0: NOP
        //  1: Store
        //  2: Fold
        static Dictionary<string, Dictionary<string, int>> ops_table = new()
            {
                ["start"] = new()
                {
                    ["+"] = 1,
                    ["-"] = 1,
                    ["*"] = 1,
                    ["/"] = 1,
                    ["("] = 1,
                    [")"] = -1,
                    ["end"] = 0,
                },
                ["+"] = new()
                {
                    ["+"] = 2,
                    ["-"] = 2,
                    ["*"] = 1,
                    ["/"] = 1,
                    ["("] = 1,
                    [")"] = 2,
                    ["end"] = 2,
                },
                ["-"] = new()
                {
                    ["+"] = 2,
                    ["-"] = 2,
                    ["*"] = 1,
                    ["/"] = 1,
                    ["("] = 1,
                    [")"] = 2,
                    ["end"] = 2,
                },
                ["*"] = new()
                {
                    ["+"] = 2,
                    ["-"] = 2,
                    ["*"] = 2,
                    ["/"] = 2,
                    ["("] = 1,
                    [")"] = 2,
                    ["end"] = 2,
                },
                ["/"] = new()
                {
                    ["+"] = 2,
                    ["-"] = 2,
                    ["*"] = 2,
                    ["/"] = 2,
                    ["("] = 1,
                    [")"] = 2,
                    ["end"] = 2,
                },
                ["("] = new()
                {
                    ["+"] = 1,
                    ["-"] = 1,
                    ["*"] = 1,
                    ["/"] = 1,
                    ["("] = 1,
                    [")"] = 2,
                    ["end"] = -1,
                },
                [")"] = new()
                {
                    ["+"] = 2,
                    ["-"] = 2,
                    ["*"] = 2,
                    ["/"] = 2,
                    ["("] = -1,
                    [")"] = 2,
                    ["end"] = 2,
                }
            };

        static readonly HashSet<char> validOperations = ['+', '-', '*', '/', '(', ')'];

        static Dictionary<string, Func<double, double, double>> ops_funcs = new() {
                ["+"] = (a, b) => a + b,
                ["*"] = (a, b) => a * b,
                ["-"] = (a, b) => a - b,
                ["/"] = (a, b) => a / b
        };

        private Stack<string> stack;
        private List<string> tokens;

        public Calculator() {
            stack = new Stack<string>();
            tokens = new List<string>();
        }

        private static List<string> GetTokens(string expression) {
            expression = expression.Replace(" ", "");
            List<string> result = [];
            string currentNumber = "";

            foreach (char c in expression)
            {
                if (validOperations.Contains(c))
                {
                    if (currentNumber.Length > 0)
                    {
                        result.Add(currentNumber);
                        currentNumber = "";
                    }
                    result.Add("" + c);
                }
                else if (c - '0' >= 0 && c - '0' <= 9)
                {
                    currentNumber += c;
                }
                else
                {
                    throw new Exception(string.Format("unsupported character: '{0}'", c));
                }
            }

            if (currentNumber.Length > 0)
            {
                result.Add(currentNumber);
            }

            result.Add("end");

            return result;
        }

        public int Calculate(string s) {
            stack.Clear();
            stack.Push("start");

            tokens = GetTokens(s);

            while (stack.Count > 1 || tokens.Count > 2)
            {
                string op = tokens[1];
                if (op == "(")
                {
                    throw new Exception("expected operator other than '('");
                }

                if (validOperations.Contains(tokens[0][0]))
                {
                    op = tokens[0];
                }

                if (!ops_table[stack.Peek()].TryGetValue(op, out var handler_code))
                {
                    throw new Exception(string.Format("unknown operation: '{0}'", op));
                }

                if (handler_code == -1) {
                    throw new Exception("unknown handler code");
                } else if (handler_code == 1) {
                    Store();
                } else if (handler_code == 2) {
                    Fold();
                }
            }

            Console.WriteLine(tokens[0]);
            Console.WriteLine(double.Parse(tokens[0]));

            return (int)Math.Round(double.Parse(tokens[0]));
        }

        private void Store() {
            string token;
            do
            {
                token = tokens[0];
                stack.Push(token);
                tokens.RemoveAt(0);
            } while (!validOperations.Contains(token[0]));
        }

        private void Fold()
        {
            if (double.TryParse(tokens[0], out double secondOperand))
            {
                var opName = stack.Pop();
                if (opName == "(")
                {
                    tokens.RemoveAt(1);
                    return;
                }

                var firstOperand = double.Parse(stack.Pop());

                if (ops_funcs.TryGetValue(opName, out Func<double, double, double> op))
                {
                    var result = op(firstOperand, secondOperand);
                    tokens[0] = result.ToString();
                }
                else
                {
                    throw new Exception(string.Format("unknown operation: '{0}'", opName));
                }
            }
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
