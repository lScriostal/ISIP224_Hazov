using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Hazov
{
    public enum EnemyType { Goblin, Skeleton, Mage }
    public enum ItemType { Weapon, Armor, Potion }
    public enum PlayerAction { Attack, Defend }

    public abstract class Item
    {
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public Item(string name, ItemType type)
        {
            Name = name;
            Type = type;
        }
        public abstract void ShowStats();
    }

    public class Weapon : Item
    {
        public int AttackBonus { get; set; }
        public Weapon(string name, int attackBonus) : base(name, ItemType.Weapon)
        {
            AttackBonus = attackBonus;
        }
        public override void ShowStats()
        {
            Console.WriteLine($"[Оружие] {Name}: + {AttackBonus} к атаке.");
        }
    }

    public class Armor : Item
    {
        public int DefenseBonus { get; set; }
        public Armor(string name, int defenseBonus) : base(name, ItemType.Armor)
        {
            DefenseBonus = defenseBonus;
        }
        public override void ShowStats()
        {
            Console.WriteLine($"[Доспех] {Name}: + {DefenseBonus} к защите.");
        }
    }

    public class Potion : Item
    {
        public Potion() : base("Лечебное зелье", ItemType.Potion) { }
        public override void ShowStats()
        {
            Console.WriteLine($"[Зелье] {Name}: полностью восстанавливает здоровье.");
        }
    }

    public class Player
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int BaseAttack { get; set; } = 10;
        public int BaseDefense { get; set; } = 5;
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; } = false;

        public Player()
        {
            MaxHealth = 100;
            Health = MaxHealth;
        }

        public int GetTotalAttack() => BaseAttack + (EquippedWeapon?.AttackBonus ?? 0);
        public int GetTotalDefense() => BaseDefense + (EquippedArmor?.DefenseBonus ?? 0);

        public void HealFull()
        {
            Health = MaxHealth;
            Console.WriteLine("Вы полностью восстановили здоровье!");
        }
    }

    public abstract class Enemy
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        protected Random rnd = new Random();
        public Enemy(string name, int health, int attack, int defense)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Attack = attack;
            Defense = defense;
        }

        public virtual void PerformSpecialAbility(Player player, ref int damage) { }
        public virtual bool IsCriticalHit() => false;
        public virtual bool IgnoresDefense() => false;
    }

    public class Goblin : Enemy
    {
        public Goblin() : base("Гоблин", 30, 8, 2) { }
        public Goblin(string name, int hp, int atk, int def) : base(name, hp, atk, def) { }
        public override bool IsCriticalHit() => rnd.Next(1, 101) <= 20;
    }

    public class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 40, 10, 5) { }
        public Skeleton(string name, int hp, int atk, int def) : base(name, hp, atk, def) { }
        public override bool IgnoresDefense() => true;
    }

    public class Mage : Enemy
    {
        public Mage() : base("Маг", 25, 12, 3) { }
        public Mage(string name, int hp, int atk, int def) : base(name, hp, atk, def) { }
        public override void PerformSpecialAbility(Player player, ref int damage)
        {
            if (rnd.Next(1, 101) <= 20)
            {
                player.IsFrozen = true;
                Console.WriteLine("Маг наложил на вс заморозку! Вы пропустите следующий ход");
            }
        }
    }

    public class BossGoblinKing : Goblin
    {
        public BossGoblinKing() : base("ВВГ (Король Гоблинов)", 60, 12, 2) { }
        public override bool IsCriticalHit() => rnd.Next(1, 101) <= 30;
    }

    public class BossSkeletonLord : Skeleton
    {
        public BossSkeletonLord() : base("Ковальский (Лорд Скелетов)", 100, 13, 7) { }
    }

    public class BossArcheMage : Mage
    {
        public BossArcheMage() : base("Архимаг C++", 45, 19, 3) { }
        public override void PerformSpecialAbility(Player player, ref int damage)
        {
            if (rnd.Next(1, 101) <= 30)
            {
                player.IsFrozen = true;
                Console.WriteLine("Архимаг C++ наложил на вас мощную заморозку!");
            }
        }
    }

    public class BossPestov : Skeleton
    {
        public BossPestov() : base("Пестов C-- (Аномалия)", 52, 18, 3) { }
        public override void PerformSpecialAbility(Player player, ref int damage)
        {
            if (rnd.Next(1, 101) <= 35)
            {
                player.IsFrozen = true;
                Console.WriteLine("Пество C-- наложил на вас заморозку!");
            }
        }
    }

    internal class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("=== Добро пожаловать в текстовый рогалик! ===");
            Player player = new Player();
            int turnCount = 0;
            while (player.Health > 0)
            {
                turnCount++;
                Console.WriteLine($"\n--- Ход {turnCount} ---");
                if (turnCount % 10 == 0)
                {
                    Console.WriteLine("ВНИМАНИЕ! Вы чувствуете мощную ауру... Появился БОСС!");
                    Enemy boss = GenerateRandomBoss();
                    StartBattle(player, boss);
                }
                else
                {
                    if (rnd.Next(1, 101) <= 50)
                    {
                        Console.WriteLine("Вы нашли сундук!");
                        OpenChest(player);
                    }
                    else
                    {
                        Console.WriteLine("На вас напал враг!");
                        Enemy enemy = GenerateRandomEnemy();
                        StartBattle(player, enemy);
                    }
                }
                if (player.Health > 0)
                {
                    Console.WriteLine($"\n[Статус] Ваше здоровье: {player.Health}/{player.MaxHealth}. Нажмите на клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
            Console.WriteLine("\nВы погибли... Игра окончена.");
        }

        static Enemy GenerateRandomEnemy()
        {
            int type = rnd.Next(1, 4);
            switch (type)
            {
                case 1: return new Goblin();
                case 2: return new Skeleton();
                default: return new Mage();
            }

        }

        static Enemy GenerateRandomBoss()
        {
            int type = rnd.Next(1, 5);
            switch (type)
            {
                case 1: return new BossGoblinKing();
                case 2: return new BossSkeletonLord();
                case 3: return new BossArcheMage();
                default: return new BossPestov();
            }
        }

        static void StartBattle(Player player, Enemy enemy)
        {
            Console.WriteLine($"\nБой с: {enemy.Name} (HP: {enemy.Health}, Атака: {enemy.Attack}, Защита: {enemy.Defense})");
            player.IsFrozen = false;
            while (player.Health > 0 && enemy.Health > 0)
            {
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход");
                    player.IsFrozen = false;
                }
                else
                {
                    Console.WriteLine($"\nВаше здоровье: {player.Health}. Здоровье врага: {enemy.Health}");
                    Console.WriteLine("Выберите действие: 1 - Атака, 2 - Защита");
                    string choice = Console.ReadLine();
                    PlayerAction action = (choice == "2") ? PlayerAction.Defend : PlayerAction.Attack;
                    if (action == PlayerAction.Attack)
                    {
                        int damage = player.GetTotalAttack();
                        enemy.Health -= damage;
                        Console.WriteLine($"Вы атаковали и нанесли {damage} урона!");
                    }
                    else if (action == PlayerAction.Defend)
                    {
                        if (rnd.Next(1, 101) <= 40)
                        {
                            Console.WriteLine("Вы полностью уклонились от следующей атаки!");
                            continue;
                        }
                        else
                        {
                            int blockPercent = rnd.Next(70, 101);
                            int defenceValue = player.GetTotalDefense();
                            int blockedAmount = (defenceValue * blockPercent) / 100;
                            Console.WriteLine($"Вы встали в защиту (заблокировано: {blockPercent}% от {defenceValue} защиты");
                            EnemyAttacksPlayer(player, enemy, blockedAmount);
                            continue;
                        }
                    }
                }
                if (enemy.Health <= 0)
                {
                    Console.WriteLine($"\nВы победили {enemy.Name}!");
                    break;
                }
                EnemyAttacksPlayer(player, enemy, 0);
            }
        }
        static void EnemyAttacksPlayer(Player player, Enemy enemy, int blockedAmount)
        {
            if (player.Health <= 0)
                return;
            int damage = enemy.Attack;
            if (!enemy.IgnoresDefense())
            {
                damage -= blockedAmount;
                if (damage < 0) damage = 0;
            }
            else
            {
                Console.WriteLine($"{enemy.Name} игнорирует вашу защиту!");
            }
            if (enemy.IsCriticalHit())
            {
                damage *= 2;
                Console.WriteLine($"{enemy.Name} нанёс КРИТИЧЕСКИЙ удар!");
            }
            enemy.PerformSpecialAbility(player, ref damage);
            player.Health -= damage;
            Console.WriteLine($"{enemy.Name} атаковал вас и нанёс {damage} урона!");
        }
        static void OpenChest(Player player)
        {
            int itemType = rnd.Next(1, 4);
            if (itemType == 1)
            {
                Potion potion = new Potion();
                potion.ShowStats();
                player.HealFull();
            }
            else if (itemType == 2) 
            {
                Weapon newWeapon = new Weapon($"Меч урона {rnd.Next(3, 8)}", rnd.Next(3, 8));
                Console.WriteLine("\nВы нашли:");
                newWeapon.ShowStats();
                Console.WriteLine("Текущее оружие: ");
                if (player.EquippedWeapon != null) player.EquippedWeapon.ShowStats();
                else Console.WriteLine("Пусто");
                Console.WriteLine("Взять новый предмет? (1 - Да, 2 - Нет)");
                if (Console.ReadLine() == "1") player.EquippedWeapon = newWeapon;
            }
            else
            {
                Armor newArmor = new Armor($"Броня защиты {rnd.Next(2, 6)}", rnd.Next(2, 6));
                Console.WriteLine("\nВы нашли:");
                newArmor.ShowStats();
                Console.WriteLine("Текущий доспех: ");
                if (player.EquippedArmor != null) player.EquippedArmor.ShowStats();
                else Console.WriteLine("Пусто");
                Console.WriteLine("Взять новый предмет? (1 - Да, 2 - Нет)");
                if (Console.ReadLine() == "1") player.EquippedArmor = newArmor;
            }
        }
    }
}
