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
        }
        static void Main(string[] args)
        {
        }
    }
}
