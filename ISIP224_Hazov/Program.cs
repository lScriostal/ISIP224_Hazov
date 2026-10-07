using System;
using System.Collections.Generic;
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

        public static List<Book> books = new List<Book>();
        public static int nextdId = 1;

        static void Main(string[] args)
        {
            books.Add(new Book { Id = nextId++, Title = "Война и мир", Author = "Лев Толстой", Genre = Genre.Fiction, Year = 1869, Price = 1500m });
            books.Add(new Book { Id = nextId++, Title = "1984", Author = "Джордж Оруэлл", Genre = Genre.Science, Year = 1949, Price = 800m });
            books.Add(new Book { Id = nextId++, Title = "Убийство в Восточном экспрессе", Author = "Агата Кристи", Genre = Genre.Detective, Year = 1934, Price = 600m });
            books.Add(new Book { Id = nextId++, Title = "Властелин колец", Author = "Дж. Р. Р. Толкин", Genre = Genre.Fantasy, Year = 1954, Price = 2000m });
            books.Add(new Book { Id = nextId++, Title = "Краткая история времени", Author = "Стивен Хокинг", Genre = Genre.NonFiction, Year = 1988, Price = 1200m });
        }
    }
}
