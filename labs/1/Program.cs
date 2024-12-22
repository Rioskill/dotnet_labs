class Program {
    static void Main(string[] args) 
        {
            Console.Write("Введите строку: ");
            string? s = Console.ReadLine();

            if (string.IsNullOrEmpty(s)) {
                return;
            }
            
            string[] words = s.Split(' ');

            string maxLenWord = "";
            int maxLen = 0;
            for (int i = 0; i < words.Length; i++) {
                string word = words[i];
                if (word.Length > maxLen) {
                    maxLen = word.Length;
                    maxLenWord = word;
                }
            }

            Console.Write("Самое длинное слово: ");
            Console.WriteLine(maxLenWord);
            Console.Write("Его длина: ");
            Console.WriteLine(maxLen);
        }
}
