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
        public string ShortesWord { get; set; }
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
            Console.WriteLine("Введите текст состоящий из 100 симоволов: ");
            string input = Console.ReadLine();
            if (input.Length < 100)
            {
                Console.WriteLine($"Ошибка! Текст должен содержать минимум 100 символов. Вы ввели {input.Length} символов");
            }
            TextStatistics stats = AnalyzeText(input);
            history.Add(stats);
            PrintStatics(stats);
            PrintHistory();
        }
    }
}
