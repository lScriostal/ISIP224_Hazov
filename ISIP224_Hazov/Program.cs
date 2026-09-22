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
                Console.WriteLine($"Цена: {Price} руб.");
                Console.WriteLine($"Количество: {Quantity} шт. ({status})");
            }
        }

        static List<Product> products = new List<Product>();
        static int nextId = 1;

        static void Main(string[] args)
        {
            InitializeTestData();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УЧЁТА ТОВАРОВ В МАГАЗИНЕ ===");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Показать все товары");
                Console.WriteLine("0. Выход");
                Console.WriteLine("\nВыберите действие: ");

                string choise = Console.ReadLine();
                switch (choise)
                {
                    case "1": AddProduct(); break;
                    case "2": DeleteProduct(); break;
                    case "3": OrderSupply(); break;
                    case "4": SellProduct(); break;
                    case "5": SearchProducts(); break;
                    case "6": ShowAllProducts(); break;
                    case "0": exit = true; break;
                    default:
                        Console.WriteLine("Неверный ввод. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void InitializeTestData()
        {
            products.Add(new Product(nextId++, "Смартфон", 50000, 10, Category.Electronics));
            products.Add(new Product(nextId++, "Хлеб", 50, 100, Category.Groceries));
            products.Add(new Product(nextId++, "Футболка", 1500, 20, Category.Clothing));
            products.Add(new Product(nextId++, "Мыло", 100, 0, Category.Household));
            products.Add(new Product(nextId++, "Наушники", 3000, 15, Category.Electronics));
        }
        static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("--- ДОБАВЛЕНИЕ ТОВАРА ---");
            Console.WriteLine("Введите название: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ошибка: Название не может быть пустым!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Введите цену: ");
            string priceInput = Console.ReadLine();
            priceInput = priceInput?.Replace('.', ',');
            if (!decimal.TryParse(priceInput, out decimal price) || price < 0)
            {
                Console.WriteLine("Ошибка: Некорректная цена. Цена не может быть отрицательной.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Ошибка: Некорректное количество. Оно не может быть отрицательным.");
                Console.ReadKey();
                return;
            }
            Category category = GetCategoryInput();
            Product newProduct = new Product(nextId++, name, price, quantity, category);
            products.Add(newProduct);

            Console.WriteLine("\nТовар успешно добавлен!");
            newProduct.PrintInfo();
            Console.ReadKey();
        }
        static void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("--- УДАЛЕНИЕ ТОВАРА ---");
            Console.WriteLine("Введите код товара для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка: Код должен быть числом.");
                Console.ReadKey();
                return;
            }
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
                Console.WriteLine($"Товар '{product.Name}' (Код: {id}) успешно удален.");
            }
            else
            {
                Console.WriteLine("Товар с таким кодом не найден.");
            }
            Console.ReadKey();
        }
        static void OrderSupply()
        {
            Console.Clear();
            Console.WriteLine("--- ЗАКАЗ ПОСТАВКИ ---");
            Console.Write("Введите код товара: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка: Код должен быть числом.");
                Console.ReadKey();
                return;
            }

            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Товар не найден.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите количество для поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount < 1)
            {
                Console.WriteLine("Ошибка: Некорректное количество. Оно должно быть больше 0.");
                Console.ReadKey();
                return;
            }

            product.Quantity += amount;
            Console.WriteLine($"\nПоставка успешна! Новое количество '{product.Name}': {product.Quantity} шт.");
            Console.ReadKey();
        }
        static void SellProduct()
        {
            Console.Clear();
            Console.WriteLine("--- ПРОДАЖА ТОВАРА ---");
            Console.Write("Введите код товара: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка: Код должен быть числом.");
                Console.ReadKey();
                return;
            }

            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Товар не найден.");
                Console.ReadKey();
                return;
            }
            if (product.Quantity == 0)
            {
                Console.WriteLine($"Ошибка: Товара '{product.Name}' нет в наличии!");
                Console.ReadKey();
                return;
            }

            Console.Write($"Введите количество для продажи (в наличии {product.Quantity}): ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount < 1)
            {
                Console.WriteLine("Ошибка: Некорректное количество. Оно должно быть больше 0.");
                Console.ReadKey();
                return;
            }
            if (amount > product.Quantity)
            {
                Console.WriteLine($"Ошибка: Недостаточно товара. В наличии только {product.Quantity} шт.");
            }
            else
            {
                product.Quantity -= amount;
                Console.WriteLine($"\nПродажа успешна! Продано {amount} шт. Остаток: {product.Quantity} шт.");
            }
            Console.ReadKey();
        }
        static void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("--- ПОИСК ТОВАРОВ ---");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска: ");

            string choice = Console.ReadLine();
            List<Product> results = new List<Product>();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите код: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                        results = products.Where(p => p.Id == id).ToList();
                    break;
                case "2":
                    Console.Write("Введите часть названия: ");
                    string namePart = Console.ReadLine()?.ToLower() ?? "";
                    results = products.Where(p => p.Name.ToLower().Contains(namePart)).ToList();
                    break;
                case "3":
                    Category cat = GetCategoryInput();
                    results = products.Where(p => p.Category == cat).ToList();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }

            Console.WriteLine("\n--- РЕЗУЛЬТАТЫ ПОИСКА ---");
            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
            }
            else
            {
                foreach (var p in results)
                {
                    p.PrintInfo();
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }
        static void ShowAllProducts()
        {
            Console.Clear();
            Console.WriteLine("--- СПИСОК ВСЕХ ТОВАРОВ ---");
            if (products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
            }
            else
            {
                foreach (var p in products)
                {
                    p.PrintInfo();
                    Console.WriteLine();
                }
                
            }
            Console.ReadKey();
        }
        static Category GetCategoryInput()
        {
            Console.WriteLine("\nВыберите категорию:");
            var values = Enum.GetValues(typeof(Category));
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values.GetValue(i)}");
            }

            while (true)
            {
                Console.Write("Ваш выбор: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= values.Length)
                {
                    return (Category)values.GetValue(choice - 1);
                }
                Console.WriteLine("Ошибка: Выберите номер из списка.");
            }
        }
    }
}
