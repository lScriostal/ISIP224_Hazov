using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    public class TextStatistics
    {
        public string OriginalText { get; set; }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }
        public TextStatistics()
        {
            LetterFrequency = new Dictionary<char, int>();
        }
    }

    internal class Program
    {
        static List<TextStatistics> history = new List<TextStatistics>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите текст состоящий из 100 симоволов: ");
                string input = Console.ReadLine();
                if (input.Length < 100)
                {
                    Console.WriteLine($"Ошибка! Текст должен содержать минимум 100 символов. Вы ввели {input.Length} символов");
                }
                TextStatistics stats = AnalyzeText(input);
                history.Add(stats);
                PrintStatics(stats);
                Console.WriteLine("Хотите вывести статитсику по прошлам текстам? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    PrintHistory();
                }
            }
            Console.WriteLine("Программа завершена. Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static TextStatistics AnalyzeText(string text)
        {
            TextStatistics stats = new TextStatistics();
            stats.OriginalText = text;
            string[] words = text.Split(new char[] { ' ', '.', ',', '!', '?', ';', ':', '-' },
                StringSplitOptions.RemoveEmptyEntries);
            stats.WordCount = words.Length;
            if (words.Length > 0)
            {
                string shortest = words[0];
                string longest = words[0];
                foreach (string word in words)
                {
                    string cleanWord = word.Trim();
                    if (cleanWord.Length == 0) continue;
                    if (cleanWord.Length < shortest.Length)
                    {
                        shortest = cleanWord;
                    }
                    if (cleanWord.Length > longest.Length)
                    {
                        longest = cleanWord;
                    }
                }
                stats.ShortestWord = shortest;
                stats.LongestWord = longest;
            }
            else
            {
                stats.ShortestWord = "Нет слов";
                stats.LongestWord = "Нет слов";
            }
        }
    }
}
