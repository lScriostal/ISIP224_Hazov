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
        }
    }
}
