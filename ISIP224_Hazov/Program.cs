using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    internal class Program
    {
        public abstract class Person
        {
            private static int _nextId = 1;
            public int Id { get; private set; }
            public string Name { get; private set; }
            public int Age { get; private set; }
            public string Email { get; private set; }
            protected Person(string name, int age, string email) 
            { 
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Имя не может быть пустым.");
                if (age <= 0 || age > 120) throw new ArgumentOutOfRangeException("Возраст должен быть в диапазоне от 1 до 120.");
                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new ArgumentNullException("Некорректный email.");
                Id = _nextId++;
                Name = name;
                Age = age;
                Email = email;
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
