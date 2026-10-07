using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    internal class Program
    {
        public enum Genre
        {
            Fiction,
            NonFiction,
            Fantasy,
            Science,
            Detective
        }

        public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public Genre Genre { get; set; }
            public int Year { get; set; }
            public decimal Price { get; set; }

            public override string ToString()
            {
                return $"ID: {Id} | Название: {Title} | Автор: {Author} | Жанр: {Genre} | Год: {Year} | Цена: {Price} рублей";
            }
        }

        private static List<Book> books = new List<Book>();
        private static int nextId = 1;

        static void Main(string[] args)
        {
            books.Add(new Book { Id = nextId++, Title = "Война и мир", Author = "Лев Толстой", Genre = Genre.Fiction, Year = 1869, Price = 1500 });
            books.Add(new Book { Id = nextId++, Title = "1984", Author = "Джордж Оруэлл", Genre = Genre.Science, Year = 1949, Price = 800 });
            books.Add(new Book { Id = nextId++, Title = "Убийство в Восточном экспрессе", Author = "Агата Кристи", Genre = Genre.Detective, Year = 1934, Price = 600 });
            books.Add(new Book { Id = nextId++, Title = "Властелин колец", Author = "Дж. Р. Р. Толкин", Genre = Genre.Fantasy, Year = 1954, Price = 2000 });
            books.Add(new Book { Id = nextId++, Title = "Краткая история времени", Author = "Стивен Хокинг", Genre = Genre.NonFiction, Year = 1988, Price = 1200 });

            bool exit = false;
            while (exit) 
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу по ID");
                Console.WriteLine("3. Найти книги (по названию, автору, жанру)");
                Console.WriteLine("4. Сортировать книги по названию");
                Console.WriteLine("5. Сортировать книги по году");
                Console.WriteLine("6. Показать самую дорогую и самую дешёвую книгу");
                Console.WriteLine("7. Группировать книги по авторам");
                Console.WriteLine("8. Показать все книги");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddBook(); break;
                        case "2": RemoveBook(); break;
                        case "3": SearchBooks(); break;
                        case "4": SortByTitle(); break;
                        case "5": SortByYear(); break;
                        case "6": ShowMostExpensiveAndCheapest(); break;
                        case "7": GroupByAuthor(); break;
                        case "8": ShowAllBooks(); break;
                        case "0": exit = true; break;
                        default: Console.WriteLine("Неверный выбор. Попробуйте снова."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void AddBook()
        {
            Console.WriteLine("\n--- Добавление книги ---");
            string title = ReadNonEmptyString("Введите название: ");
            string author = ReadNonEmptyString("Введите автора: ");
            Genre genre = ReadGenre();
            int year = ReadPositiveInt("Введите год издания: ");
            decimal price = ReadPositiveDecimal("Введите цену: ");

            Book newBook = new Book
            {
                Id = nextId++,
                Title = title,
                Author = author,
                Genre = genre,
                Year = year,
                Price = price
            };
            books.Add(newBook);
            Console.WriteLine("Книга успешно добавлена!");
            Console.WriteLine(newBook);
        }

        static void RemoveBook()
        {
            Console.WriteLine("\n--- Удаление книги ---");
            int id = ReadPositiveInt("Введите ID книги для удаления: ");
            var bookToRemove = books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                Console.WriteLine("Книга успешно удалена.");
            }
            else
            {
                Console.WriteLine("Книга с таким ID не найдена.");
            }
        }

        static void SearchBooks()
        {
            Console.WriteLine("\n--- Поиск книг ---");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По автору");
            Console.WriteLine("3. По жанру");
            Console.Write("Выберите вариант поиска: ");
            string choice = Console.ReadLine();
            List<Book> results = new List<Book>();

            switch (choice)
            {
                case "1": 
                    string TitleQuery = ReadNonEmptyString("Введите название (или часть): ");
                    results = books.Where(b => b.Title.ToLower().Contains(TitleQuery.ToLower())).ToList();
                    break;
                case "2":
                    string AuthorQuery = ReadNonEmptyString("Введите автора (или часть): ");
                    results = books.Where(b => b.Author.ToLower().Contains(AuthorQuery.ToLower())).ToList();
                    break;
                case "3":
                    Genre genreQuere = ReadGenre();
                    results = books.Where(b => b.Genre == genreQuere).ToList();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            if (results.Any())
            {
                Console.WriteLine("Резульаты поиска: ");
                foreach (var book in results) {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("Книги не найдены.");
            }
        }

        static void SortByTitle()
        {
            Console.WriteLine("\n--- Книги, отсортированные по названию ---");
            var sorted = books.OrderBy(b => b.Title).ToList();
            PrintBooks(sorted);
        }

        static void SortByYear()
        {
            Console.WriteLine("\n--- Книги, отсортированные по году ---");
            var sorted = books.OrderBy(b => b.Year).ToList();
            PrintBooks(sorted);
        }

        static void ShowMostExpensiveAndCheapest()
        {
            if (!books.Any())
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }
            Console.WriteLine("\n--- Самая дорогая книга ---");
            var mostExpensive = books.OrderByDescending(b => b.Price).First();
            Console.WriteLine(mostExpensive);

            Console.WriteLine("\n--- Самая дешёвая книга ---");
            var cheapest = books.OrderBy(b => b.Price).First();
            Console.WriteLine(cheapest);
        }

        static void GroupByAuthor()
        {
            Console.WriteLine("\n--- Количество книг по авторам ---");
            var grouped = books.GroupBy(b => b.Author).Select(g => new { Author = g.Key, Count = g.Count() }).OrderBy(g => g.Author);
            foreach (var group in grouped)
            {
                Console.WriteLine($"Автор: {group.Author} | Количество книг: {group.Count}");
            }
        }

        static void ShowAllBooks()
        {
            Console.WriteLine("\n--- Все книги ---");
            PrintBooks(books);
        }

        static void PrintBooks(List<Book> list)
        {
            if (!list.Any())
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            foreach (var book in list)
            {
                Console.WriteLine(book);
            }
        }

        static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.WriteLine("Ошибка: строка не может быть пустой. Попробуйте снова.");
            }
        }

        static int ReadPositiveInt(string prompt)
        {
            while (true) 
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result) && result > 0)
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите положительное целое число. Попробуйте снова.");
            }
        }

        static decimal ReadPositiveDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out decimal result) && result > 0)
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите положительное число. Попробуйте снова.");
            }
        }

        static Genre ReadGenre()
        {
            Console.WriteLine("Доступные жанры:");
            foreach (var genre in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine($"- {genre}");
            }

            while (true)
            {
                Console.Write("Введите жанр: ");
                string input = Console.ReadLine();
                if (Enum.TryParse(input, true, out Genre result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: неверный жанр. Попробуйте снова.");
            }
        }
    }
}
