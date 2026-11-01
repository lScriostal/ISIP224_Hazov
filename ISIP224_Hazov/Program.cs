using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    internal class Program
    {
        static Users currentUser = null;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Маркетплейс GMWOG ===");

                if (currentUser == null)
                {
                    Console.WriteLine("1. Войти");
                    Console.WriteLine("2. Зарегистрироваться");
                    Console.WriteLine("3. Посмотреть товары (Гость)");
                    Console.WriteLine("0. Выход");
                }
                else
                {
                    Console.WriteLine($"Вы вошли как: {currentUser.Login}");
                    Console.WriteLine("1. Посмотреть товары / Добавить в корзину");
                    Console.WriteLine("2. Моя корзина / Оформить заказ");
                    Console.WriteLine("3. Мои заказы");
                    Console.WriteLine("0. Выйти из аккаунта");
                }

                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine() ?? "";

                if (currentUser == null)
                {
                    switch (choice)
                    {
                        case "1": Login(); break;
                        case "2": Register(); break;
                        case "3": ViewProducts(false); break;
                        case "0": return;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1": ViewProducts(true); break;
                        case "2": ViewCartAndCheckout(); break;
                        case "3": ViewOrders(); break;
                        case "0": currentUser = null; break;
                    }
                }
            }
        }

        static void Register()
        {
            Console.Clear();
            Console.Write("Придумайте логин: ");
            string login = Console.ReadLine()?.Trim() ?? "";

            List<Users> users = Core.Context.Users.ToList();

            if (string.IsNullOrEmpty(login) || users.Any(u => u.Login == login))
            {
                Console.WriteLine("Логин пуст или уже занят!"); Console.ReadKey(); return;
            }

            Console.Write("Придумайте пароль: ");
            string pass1 = Console.ReadLine() ?? "";
            Console.Write("Повторите пароль: ");
            string pass2 = Console.ReadLine() ?? "";

            if (pass1 != pass2)
            {
                Console.WriteLine("Пароли не совпадают!"); Console.ReadKey(); return;
            }

            Users newUser = new Users
            {
                Login = login,
                PasswordHash = pass1
            };

            Core.Context.Users.Add(newUser);
            Core.Context.SaveChanges();

            Console.WriteLine("Регистрация успешна! Теперь вы можете войти."); Console.ReadKey();
        }

        static void Login()
        {
            Console.Clear();
            Console.Write("Логин: ");
            string login = Console.ReadLine() ?? "";
            Console.Write("Пароль: ");
            string pass = Console.ReadLine() ?? "";

            List<Users> users = Core.Context.Users.ToList();

            var user = users.FirstOrDefault(u => u.Login == login && u.PasswordHash == pass);

            if (user != null)
            {
                currentUser = user;
                Console.WriteLine("Успешный вход!");
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
            }
            Console.ReadKey();
        }

        static void ViewProducts(bool canAddToCart)
        {
            Console.Clear();
            List<Products> products = Core.Context.Products.ToList();

            Console.WriteLine("=== Список товаров ===");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}. {p.ProductName} - {p.Price} руб.");
            }

            if (canAddToCart)
            {
                Console.Write("\nВведите ID товара для добавления в корзину (0 - назад): ");
                if (int.TryParse(Console.ReadLine(), out int prodId) && prodId != 0)
                {
                    var product = products.FirstOrDefault(p => p.Id == prodId);
                    if (product != null)
                    {
                        List<Carts> cartItems = Core.Context.Carts.ToList();

                        // Убрали восклицательный знак (currentUser!)
                        var cartItem = cartItems.FirstOrDefault(c => c.UserId == currentUser.Id && c.ProductId == prodId);

                        if (cartItem != null)
                        {
                            cartItem.Quantity++;
                            Core.Context.SaveChanges();
                        }
                        else
                        {
                            Carts newCartItem = new Carts
                            {
                                UserId = currentUser.Id, // Убрали !
                                ProductId = prodId,
                                Quantity = 1
                            };
                            Core.Context.Carts.Add(newCartItem);
                            Core.Context.SaveChanges();
                        }
                        Console.WriteLine($"Товар '{product.ProductName}' добавлен в корзину!");
                    }
                    else Console.WriteLine("Товар не найден.");
                }
            }
            Console.ReadKey();
        }

        static void ViewCartAndCheckout()
        {
            Console.Clear();

            List<Carts> cartItems = Core.Context.Carts.ToList();

            // Убрали !
            var myCart = cartItems.Where(c => c.UserId == currentUser.Id).ToList();

            if (!myCart.Any())
            {
                Console.WriteLine("Корзина пуста."); Console.ReadKey(); return;
            }

            Console.WriteLine("=== Ваша корзина ===");
            decimal total = 0;
            List<Products> allProducts = Core.Context.Products.ToList();

            foreach (var item in myCart)
            {
                var product = allProducts.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    decimal sum = product.Price * item.Quantity;
                    total += sum;
                    Console.WriteLine($"ID Товара: {item.ProductId} | {product.ProductName} x{item.Quantity} = {sum} руб.");
                }
            }
            Console.WriteLine($"Итого: {total} руб.");

            Console.WriteLine("\n1. Купить один предмет");
            Console.WriteLine("2. Купить всю корзину сразу");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine() ?? "";

            if (choice == "1" || choice == "2")
            {
                Console.Clear();
                Console.WriteLine("=== Доступные ПВЗ ===");
                List<PickupPoints> pps = Core.Context.PickupPoints.ToList();
                foreach (var pp in pps)
                {
                    Console.WriteLine($"{pp.Id}. {pp.Address}");
                }

                Console.Write("\nВыберите ID ПВЗ для доставки: ");
                if (int.TryParse(Console.ReadLine(), out int ppId) && pps.Any(p => p.Id == ppId))
                {
                    Orders newOrder = new Orders
                    {
                        UserId = currentUser.Id, // Убрали !
                        PickupPointId = ppId,
                        OrderDate = DateTime.Now
                    };
                    Core.Context.Orders.Add(newOrder);
                    Core.Context.SaveChanges();

                    if (choice == "2")
                    {
                        foreach (var item in myCart)
                        {
                            var product = allProducts.First(p => p.Id == item.ProductId);
                            Core.Context.OrderDetails.Add(new OrderDetails
                            {
                                OrderId = newOrder.Id,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                PriceAtPurchase = product.Price
                            });
                            Core.Context.Carts.Remove(item);
                        }
                        Console.WriteLine("Все товары успешно куплены!");
                    }
                    else if (choice == "1")
                    {
                        Console.Write("Введите ID товара из корзины для покупки: ");
                        if (int.TryParse(Console.ReadLine(), out int buyId))
                        {
                            var itemToBuy = myCart.FirstOrDefault(c => c.ProductId == buyId);
                            if (itemToBuy != null)
                            {
                                var product = allProducts.First(p => p.Id == itemToBuy.ProductId);
                                Core.Context.OrderDetails.Add(new OrderDetails
                                {
                                    OrderId = newOrder.Id,
                                    ProductId = itemToBuy.ProductId,
                                    Quantity = itemToBuy.Quantity,
                                    PriceAtPurchase = product.Price
                                });
                                Core.Context.Carts.Remove(itemToBuy);
                                Console.WriteLine($"Товар '{product.ProductName}' успешно куплен!");
                            }
                        }
                    }
                    Core.Context.SaveChanges();
                }
            }
            Console.ReadKey();
        }

        static void ViewOrders()
        {
            Console.Clear();
            Console.WriteLine("=== Мои заказы (сортировка по дате, новые сверху) ===");

            List<Orders> orders = Core.Context.Orders.ToList();

            // Убрали !
            var myOrders = orders
                .Where(o => o.UserId == currentUser.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            if (!myOrders.Any())
            {
                Console.WriteLine("У вас пока нет заказов.");
            }

            List<PickupPoints> pps = Core.Context.PickupPoints.ToList();
            List<OrderDetails> allDetails = Core.Context.OrderDetails.ToList();
            List<Products> allProducts = Core.Context.Products.ToList();

            foreach (var order in myOrders)
            {
                // Убрали ? в pp?.Address
                var pp = pps.FirstOrDefault(p => p.Id == order.PickupPointId);
                Console.WriteLine($"\nЗаказ №{order.Id} от {order.OrderDate:dd.MM.yyyy HH:mm} | ПВЗ: {pp.Address}");

                var details = allDetails.Where(d => d.OrderId == order.Id).ToList();
                foreach (var detail in details)
                {
                    // Убрали ? в prod?.ProductName
                    var prod = allProducts.FirstOrDefault(p => p.Id == detail.ProductId);
                    Console.WriteLine($"  - {prod.ProductName} x{detail.Quantity} (Цена: {detail.PriceAtPurchase} руб.)");
                }
            }
            Console.ReadKey();
        }
    }
}
