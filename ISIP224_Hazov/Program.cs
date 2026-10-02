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
                Console.WriteLine("Введите текст состоящий из 100 симоволов (или 'exit' для выхода): ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit")
                    break;
                if (input.Length < 100)
                {
                    Console.WriteLine($"Ошибка! Текст должен содержать минимум 100 символов. Вы ввели {input.Length} символов");
                    continue;
                }
                TextStatistics stats = AnalyzeText(input);
                history.Add(stats);
                PrintStatistics(stats);
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

            int sentenceCount = 0;
            foreach (char c in text)
            {
                if (c == '.' || c == '!' || c == '?')
                {
                    sentenceCount++;
                }
            }
            if (sentenceCount == 0 && words.Length > 0)
            {
                sentenceCount = 1;
            }
            stats.SentenceCount = sentenceCount;

            int vowels = 0;
            int consonants = 0;
            string vowelsList = "аеёиоуыэюя";
            string consonantsList = "бвгджзйклмнпрстфхцчшщ";
            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (vowelsList.IndexOf(c) >= 0)
                    {
                        vowels++;
                    }
                    else if (consonantsList.IndexOf(c) >= 0)
                    {
                        consonants++;
                    }

                    if (stats.LetterFrequency.ContainsKey(c))
                    {
                        stats.LetterFrequency[c]++;
                    }
                    else
                    {
                        stats.LetterFrequency.Add(c, 1);
                    }
                }
            }
            stats.VowelCount = vowels;
            stats.ConsonantCount = consonants;
            return stats;
        }
        static void PrintStatistics(TextStatistics stats)
        {
            Console.WriteLine($"Количество слов: {stats.WordCount}");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
            Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
            Console.WriteLine($"Количество гласных букв: {stats.VowelCount}");
            Console.WriteLine($"Количество согласных букв: {stats.ConsonantCount}");
            Console.WriteLine("Частота встречаемости букв: ");
            List<KeyValuePair<char, int>> sortedFreq = new List<KeyValuePair<char, int>>(stats.LetterFrequency);
            sortedFreq.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            foreach (var pair in sortedFreq)
            {
                Console.WriteLine($"'{pair.Key}': {pair.Value}");
            }
        }

        static void PrintHistory()
        {
            Console.WriteLine("\n=== История всех текстов ===");
            if (history.Count == 0)
            {
                Console.WriteLine("История пуста.");
                return;
            }
            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine($"\n--- Текст #{i + 1} ---");
                Console.WriteLine($"Слов: {history[i].WordCount}, Предложений: {history[i].SentenceCount}");
            }
        }
    }
}
