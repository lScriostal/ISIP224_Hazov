using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int operation = 0;
            bool invalidecount = false;
            do
            {
                Console.Write("Введите количество операций, которые хотите ввести (от 2 до 40): ");
                if (int.TryParse(Console.ReadLine(), out operation) && operation >= 2 && operation <= 40)
                {
                    invalidecount = true;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите корректное значение!");
                }
            } while (!invalidecount);

            string[] titles = new string[operation];
            double[] amounts = new double[operation];

            Console.WriteLine("Ввод трат по шаблону (Название услуги или товара; Количество денег) (Валюта - рубли)");
            Console.WriteLine("Пример: Влажные салфетки (Лента); 235");
            for (int i = 0; i < operation; i++)
            {
                bool isvalideExpense = false;
                do
                {
                    Console.Write($"Операция {i + 1} из {operation}: ");
                    string input = Console.ReadLine();
                    string[] parts = input.Split(new char[] { ';' });
                    if (parts.Length == 2)
                    {
                        string titlePart = parts[0].Trim();
                        string amountPart = parts[1].Trim();
                        if (!string.IsNullOrWhiteSpace(titlePart) && double.TryParse(amountPart, out double amount) && amount >= 0)
                        {
                            titles[i] = titlePart;
                            amounts[i] = amount;
                            isvalideExpense = true;
                        }
                    }
                    if (!isvalideExpense)
                    {
                        Console.WriteLine("Ошибка! Используйте формат: Название; Сумма (сумма - положительное число)");
                    }
                } while (!isvalideExpense);
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика");
                Console.WriteLine("3. Сортировка по цене");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    OutputData(titles, amounts);
                }
                else if (choice == "2")
                {
                    ShowStatistics(amounts);
                }
                else if (choice == "3")
                {
                    SortByPrice(titles, amounts);
                }
                else if (choice == "4")
                {
                    ConvertCurrency(amounts);
                }
                else if (choice == "5")
                {
                    SearchByName(titles, amounts);
                }
                else if (choice == "0")
                {
                    exit = true;
                    Console.WriteLine("Выход из программы.");
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                }
            }
        }

        static void OutputData(string[] titles, double[] amounts)
        {
            Console.WriteLine("\nВаш список трат: ");
            for(int i = 0; i < titles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {titles[i]} - {amounts[i]} руб.");
            }
        }
        static void ShowStatistics(double[] amounts)
        {
            if (amounts.Length == 0) return;

            double sum = 0;
            double max = amounts[0];
            double min = amounts[0];
            double average = 0;
            foreach (double amount in amounts)
            {
                sum += amount;
                max = Math.Max(max, amount);
                min = Math.Min(min, amount);
                average = sum / amounts.Length;


            }
            Console.WriteLine("\nСтатистика: ");
            Console.WriteLine($"Среднее: {average:F2} руб.");
            Console.WriteLine($"Максимальное значение: {max:F2} руб.");
            Console.WriteLine($"Минимальное значение: {min:F2} руб.");
            Console.WriteLine($"Сумма всех значений: {sum:F2} руб.");
        }
        static void SortByPrice(string[] titles, double[] amounts)
        {
            int n = amounts.Length;
            for (int i = 0; i < n -1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    if (amounts[j] > amounts[j + 1])
                    {
                        double tempAmounts = amounts[j];
                        amounts[j] = amounts[j + 1];
                        amounts[j + 1] = tempAmounts;

                        string tempTitles = titles[j];
                        titles[j] = titles[j + 1];
                        titles[j + 1] = tempTitles;
                    }
                }
            }
            Console.WriteLine("\nСортировка выполнена (по возрастанию цены).");
            OutputData(titles, amounts);
        }
        static void ConvertCurrency(double[] amounts)
        {
            Console.WriteLine("\nВыберите валюту для конвертации:");
            Console.WriteLine("1. USD (Доллар)");
            Console.WriteLine("2. EUR (Евро)");
            Console.WriteLine("3. CNY (Юань)");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            double rate = 0;
            string currencyName = "";

            if (choice == "1")
            {
                rate = 84;
                currencyName = "USD";
            }
            else if (choice == "2")
            {
                rate = 98;
                currencyName = "EUR";
            }
            else if (choice == "3")
            {
                rate = 12.5;
                currencyName = "CNY";
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
                return;
            }
            Console.WriteLine($"\nКонвертация по курсу {rate} руб. за 1 {currencyName}:");
            for (int i = 0; i < amounts.Length; i++)
            {
                double converted = amounts[i] / rate;
                Console.WriteLine($"{i + 1}. {amounts[i]} руб. = {converted:F2} {currencyName}");
            }
        }
        static void SearchByName(string[] titles, double[] amounts)
        {
            Console.Write("\nВведите название для поиска: ");
            string query = Console.ReadLine().ToLower();
            bool found = false;

            Console.WriteLine("Результаты поиска: ");
            for (int i = 0; i < titles.Length; i++)
            {
                if (titles[i].ToLower().Contains(query))
                {
                    Console.WriteLine($"{i + 1}. {titles[i]} - {amounts[i]} руб.");
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Ничего не найдено");
            }
        }
    }
}
