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
                Console.Write("Введите количество операций, которые хотите ввести (от 2 до 40)");
                if (int.TryParse(Console.ReadLine(), out operation) && operation >= 2 && operation <= 40)
                {
                    invalidecount = true;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите корректное значение!");
                }
            } while (!invalidecount);
        }
    }
}
