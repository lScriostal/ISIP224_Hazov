using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    internal class Program
    {
        public enum Category
        {
            Electronics,
            Groceries,
            Clothing,
            Household
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public Category Category { get; set; }
            public bool IsInStock => Quantity > 0;

            public Product(int id, string name, decimal price, int quantity, Category category)
            {
                Id = id;
                Name = name;
                Price = price;
                Quantity = quantity;
                Category = category;
            }

            public void PrintInfo()
            {
                string status = IsInStock ? "В наличии" : "Нет в наличии";
                Console.WriteLine($"Код: {Id}");
                Console.WriteLine($"Название: {Name}");
                Console.WriteLine($"Категория: {Category}");
                Console.WriteLine($"Цена: {Price:C}");
                Console.WriteLine($"Количество: {Quantity} шт. ({status})");
            }
        }

            static List<Product> products = new List<Product>();
            static int nextId = 1;
        
        static void Main(string[] args)
        {
        }
    }
}
