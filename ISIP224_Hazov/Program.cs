using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    abstract class Person
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

        // Конструктор теперь принимает id, чтобы наследники могли передавать свои счетчики
        protected Person(int id, string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Имя не может быть пустым!");
            if (age <= 0 || age > 120)
                throw new Exception("Возраст должен быть от 1 до 120!");

            Id = id;
            Name = name;
            Age = age;
        }

        public abstract string GetInfo();
    }

    // 2. КЛАСС СТУДЕНТ (Наследование)
    class Student : Person
    {
        // Отдельный счетчик для студентов
        private static int _studentIdCounter = 1;

        public List<Course> Courses { get; private set; } = new List<Course>();

        // Передаем в базовый класс _studentIdCounter++
        public Student(string name, int age) : base(_studentIdCounter++, name, age) { }

        public void Enroll(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                Console.WriteLine($"Студент {Name} записан на курс '{course.Title}'.");
            }
            else
            {
                Console.WriteLine($"Студент {Name} уже записан на этот курс.");
            }
        }

        public override string GetInfo()
        {
            return $"Студент ID: {Id} | {Name} (Возраст: {Age}) | Курсов: {Courses.Count}";
        }
    }

    // 3. КЛАСС ПРЕПОДАВАТЕЛЬ (Наследование)
    class Teacher : Person
    {
        // Отдельный счетчик для преподавателей
        private static int _teacherIdCounter = 1;

        public List<Course> Courses { get; private set; } = new List<Course>();

        // Передаем в базовый класс _teacherIdCounter++
        public Teacher(string name, int age) : base(_teacherIdCounter++, name, age) { }

        // Исправлена опечатка в названии метода
        public void AssignToCourse(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                Console.WriteLine($"Преподаватель {Name} назначен на курс '{course.Title}'.");
            }
        }

        public override string GetInfo()
        {
            return $"Преподаватель ID: {Id} | {Name} (Возраст: {Age}) | Ведет курсов: {Courses.Count}";
        }
    }

    // 4. КЛАСС КУРС
    class Course
    {
        private static int _courseIdCounter = 1;
        public int Id { get; private set; }
        public string Title { get; private set; }
        public Teacher Instructor { get; private set; }
        public List<Student> Students { get; private set; } = new List<Student>();

        public Course(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Название курса не может быть пустым!");

            Id = _courseIdCounter++;
            Title = title;
        }

        public void SetTeacher(Teacher teacher)
        {
            Instructor = teacher;
            teacher.AssignToCourse(this); // Исправлено имя метода
        }

        public void AddStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
                student.Enroll(this);
            }
        }

        public override string ToString()
        {
            string teacherName = Instructor != null ? Instructor.Name : "Не назначен";
            return $"Курс ID: {Id} | {Title} | Преподаватель: {teacherName} | Студентов: {Students.Count}";
        }
    }

    // 5. ГЛАВНАЯ ПРОГРАММА
    internal class Program
    {
        static List<Student> students = new List<Student>();
        static List<Teacher> teachers = new List<Teacher>();
        static List<Course> courses = new List<Course>();

        static void Main(string[] args)
        {
            // Чтобы русские буквы нормально отображались в консоли
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n=== МЕНЮ ===");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Создать курс");
                Console.WriteLine("4. Назначить преподавателя на курс");
                Console.WriteLine("5. Записать студента на курс");
                Console.WriteLine("6. Показать всех студентов");
                Console.WriteLine("7. Показать всех преподавателей");
                Console.WriteLine("8. Показать все курсы");
                Console.WriteLine("9. Показать курсы конкретного студента");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddStudent(); break;
                        case "2": AddTeacher(); break;
                        case "3": CreateCourse(); break;
                        case "4": AssignTeacher(); break;
                        case "5": EnrollStudent(); break;
                        case "6": ShowAllStudents(); break;
                        case "7": ShowAllTeachers(); break;
                        case "8": ShowAllCourses(); break;
                        case "9": ShowStudentCourses(); break;
                        case "0": return;
                        default: Console.WriteLine("Неверный пункт меню."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        // --- ДОБАВЛЕНИЕ ---
        static void AddStudent()
        {
            Console.Write("Имя студента: ");
            string name = Console.ReadLine();
            Console.Write("Возраст студента: ");
            int age = int.Parse(Console.ReadLine());

            students.Add(new Student(name, age));
            Console.WriteLine("Студент добавлен!");
        }

        static void AddTeacher()
        {
            Console.Write("Имя преподавателя: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());

            teachers.Add(new Teacher(name, age));
            Console.WriteLine("Преподаватель добавлен!");
        }

        static void CreateCourse()
        {
            Console.Write("Название курса: ");
            string title = Console.ReadLine();

            courses.Add(new Course(title));
            Console.WriteLine("Курс создан!");
        }

        // --- СВЯЗЫВАНИЕ ---
        static void AssignTeacher()
        {
            if (teachers.Count == 0 || courses.Count == 0)
                throw new Exception("Сначала добавьте преподавателей и курсы!");

            Console.WriteLine("Выберите преподавателя (ID):");
            teachers.ForEach(t => Console.WriteLine($"{t.Id}. {t.Name}"));
            int tId = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите курс (ID):");
            courses.ForEach(c => Console.WriteLine($"{c.Id}. {c.Title}"));
            int cId = int.Parse(Console.ReadLine());

            var teacher = teachers.FirstOrDefault(t => t.Id == tId);
            var course = courses.FirstOrDefault(c => c.Id == cId);

            if (teacher == null || course == null)
                throw new Exception("Неверный ID!");

            course.SetTeacher(teacher);
        }

        static void EnrollStudent()
        {
            if (students.Count == 0 || courses.Count == 0)
                throw new Exception("Сначала добавьте студентов и курсы!");

            Console.WriteLine("Выберите студента (ID):");
            students.ForEach(s => Console.WriteLine($"{s.Id}. {s.Name}"));
            int sId = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите курс (ID):");
            courses.ForEach(c => Console.WriteLine($"{c.Id}. {c.Title}"));
            int cId = int.Parse(Console.ReadLine());

            var student = students.FirstOrDefault(s => s.Id == sId);
            var course = courses.FirstOrDefault(c => c.Id == cId);

            if (student == null || course == null)
                throw new Exception("Неверный ID!");

            course.AddStudent(student);
        }

        // --- ПРОСМОТР ---
        static void ShowAllStudents()
        {
            var sorted = students.OrderBy(s => s.Name).ToList();
            if (sorted.Count == 0) Console.WriteLine("Список пуст.");
            else sorted.ForEach(s => Console.WriteLine(s.GetInfo()));
        }

        static void ShowAllTeachers()
        {
            var sorted = teachers.OrderBy(t => t.Name).ToList();
            if (sorted.Count == 0) Console.WriteLine("Список пуст.");
            else sorted.ForEach(t => Console.WriteLine(t.GetInfo()));
        }

        static void ShowAllCourses()
        {
            var sorted = courses.OrderBy(c => c.Title).ToList();
            if (sorted.Count == 0) Console.WriteLine("Список пуст.");
            else sorted.ForEach(c => Console.WriteLine(c.ToString()));
        }

        static void ShowStudentCourses()
        {
            if (students.Count == 0) throw new Exception("Нет студентов!");

            Console.WriteLine("Выберите студента (ID):");
            students.ForEach(s => Console.WriteLine($"{s.Id}. {s.Name}"));
            int sId = int.Parse(Console.ReadLine());

            var student = students.FirstOrDefault(s => s.Id == sId);
            if (student == null) throw new Exception("Студент не найден!");

            Console.WriteLine($"\nКурсы студента {student.Name}:");
            if (student.Courses.Count == 0)
                Console.WriteLine("Студент никуда не записан.");
            else
                student.Courses.ForEach(c => Console.WriteLine($" - {c.Title}"));
        }
    }
}

