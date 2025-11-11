using System;
using System.Collections.Generic;

namespace TextRoguelike
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public virtual string GetStats()
        {
            return $"{Name} (АТК: {Attack}, ЗАЩ: {Defense})";
        }
    }

    public class Weapon : Item
    {
        public Weapon(string name, int attack, int defense = 0)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }
    }

    public class Armor : Item
    {
        public Armor(string name, int defense, int attack = 0)
        {
            Name = name;
            Defense = defense;
            Attack = attack;
        }
    }

    public class HealthPotion : Item
    {
        public HealthPotion()
        {
            Name = "Лечебное зелье";
            Attack = 0;
            Defense = 0;
        }

        public override string GetStats()
        {
            return $"{Name} (полное восстановление здоровья)";
        }
    }

    public class Player
    {
        public int MaxHP { get; private set; } = 100;
        public int HP { get; private set; }
        public Weapon EquippedWeapon { get; private set; }
        public Armor EquippedArmor { get; private set; }
        public bool IsFrozen { get; set; }

        public int Attack => 10 + (EquippedWeapon?.Attack ?? 0);
        public int Defense => 5 + (EquippedArmor?.Defense ?? 0);

        public Player()
        {
            HP = MaxHP;
            EquippedWeapon = new Weapon("Кулаки", 2);
            EquippedArmor = new Armor("Простая одежда", 1);
        }

        public void TakeDamage(int damage)
        {
            HP = Math.Max(0, HP - damage);
        }

        public void HealFull()
        {
            HP = MaxHP;
        }

        public void EquipWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
        }

        public void EquipArmor(Armor armor)
        {
            EquippedArmor = armor;
        }

        public void DisplayStats()
        {
            Console.WriteLine($"=== Игрок ===");
            Console.WriteLine($"Здоровье: {HP}/{MaxHP}");
            Console.WriteLine($"Атака: {Attack} (база 10 + оружие {EquippedWeapon.Attack})");
            Console.WriteLine($"Защита: {Defense} (база 5 + доспехи {EquippedArmor.Defense})");
            Console.WriteLine($"Оружие: {EquippedWeapon.GetStats()}");
            Console.WriteLine($"Доспехи: {EquippedArmor.GetStats()}");
        }
    }

    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int BaseHP { get; protected set; }
        public int BaseAttack { get; protected set; }
        public int BaseDefense { get; protected set; }
        public int HP { get; protected set; }
        public double CritChance { get; protected set; }
        public bool IgnoreArmor { get; protected set; }
        public double FreezeChance { get; protected set; }

        protected Random random = new Random();

        public Enemy()
        {
            HP = BaseHP;
        }

        public virtual void TakeDamage(int damage)
        {
            HP = Math.Max(0, HP - damage);
        }

        public virtual int CalculateDamage(Player player)
        {
            int damage = BaseAttack;

            if (random.NextDouble() < CritChance)
            {
                damage *= 2;
                Console.WriteLine($"Критический урон! Урон удвоен: {damage}");
            }

            if (!IgnoreArmor)
            {
                damage = Math.Max(1, damage - player.Defense);
            }

            return damage;
        }

        public virtual bool TryFreeze()
        {
            return random.NextDouble() < FreezeChance;
        }

        public bool IsAlive => HP > 0;

        public virtual void DisplayStats()
        {
            Console.WriteLine($"=== {Name} ===");
            Console.WriteLine($"Здоровье: {HP}/{BaseHP}");
            Console.WriteLine($"Атака: {BaseAttack}");
            Console.WriteLine($"Защита: {BaseDefense}");

            List<string> abilities = new List<string>();
            if (CritChance > 0) abilities.Add($"Крит: {CritChance * 100}%");
            if (IgnoreArmor) abilities.Add("Игнор брони");
            if (FreezeChance > 0) abilities.Add($"Заморозка: {FreezeChance * 100}%");

            if (abilities.Count > 0)
                Console.WriteLine($"Способности: {string.Join(", ", abilities)}");
        }
    }

    public class Goblin : Enemy
    {
        public Goblin()
        {
            Name = "Гоблин";
            BaseHP = 30;
            BaseAttack = 8;
            BaseDefense = 2;
            CritChance = 0.2;
            IgnoreArmor = false;
            FreezeChance = 0;
            HP = BaseHP;
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            BaseHP = 25;
            BaseAttack = 10;
            BaseDefense = 3;
            CritChance = 0;
            IgnoreArmor = true;
            FreezeChance = 0;
            HP = BaseHP;
        }
    }

    public class Mage : Enemy
    {
        public Mage()
        {
            Name = "Маг";
            BaseHP = 20;
            BaseAttack = 12;
            BaseDefense = 1;
            CritChance = 0;
            IgnoreArmor = false;
            FreezeChance = 0.2;
            HP = BaseHP;
        }
    }

    public class VVG : Goblin
    {
        public VVG()
        {
            Name = "ВВГ (Босс-Гоблин)";
            BaseHP = (int)(30 * 2.0);
            BaseAttack = (int)(8 * 1.5);
            BaseDefense = (int)(2 * 1.2);
            CritChance = 0.3;
            HP = BaseHP;
        }
    }

    public class Kovalsky : Skeleton
    {
        public Kovalsky()
        {
            Name = "Ковальский (Босс-Скелет)";
            BaseHP = (int)(25 * 2.5);
            BaseAttack = (int)(10 * 1.3);
            BaseDefense = (int)(3 * 1.4);
            HP = BaseHP;
        }
    }

    public class ArchmageCPlusPlus : Mage
    {
        public ArchmageCPlusPlus()
        {
            Name = "Архимаг C++ (Босс-Маг)";
            BaseHP = (int)(20 * 1.8);
            BaseAttack = (int)(12 * 1.6);
            BaseDefense = (int)(1 * 1.1);
            FreezeChance = 0.3;
            HP = BaseHP;
        }
    }

    public class PestovCMinusMinus : Skeleton
    {
        public PestovCMinusMinus()
        {
            Name = "Пестов С-- (Босс-Скелет)";
            BaseHP = (int)(25 * 1.3);
            BaseAttack = (int)(10 * 1.8);
            BaseDefense = (int)(3 * 0.6);
            FreezeChance = 0.35;
            HP = BaseHP;
        }
    }

    public class Game
    {
        private Player player;
        private Random random;
        private int turn;

        private List<Weapon> weapons = new List<Weapon>
        {
            new Weapon("Кинжал", 3),
            new Weapon("Меч", 6),
            new Weapon("Секира", 8),
            new Weapon("Волшебный посох", 5)
        };

        private List<Armor> armors = new List<Armor>
        {
            new Armor("Кожаная броня", 3),
            new Armor("Кольчуга", 5),
            new Armor("Латные доспехи", 8),
            new Armor("Волшебные одежды", 4)
        };

        public Game()
        {
            player = new Player();
            random = new Random();
            turn = 0;
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в текстовую рогалик-игру!");
            Console.WriteLine("Цель: выживать как можно дольше, побеждая врагов и находя предметы.");

            while (player.HP > 0)
            {
                turn++;
                Console.WriteLine($"\n--- Ход {turn} ---");
                player.DisplayStats();

                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                    continue;
                }

                if (turn % 10 == 0)
                {
                    Console.WriteLine("!!! Появляется БОСС !!!");
                    FightBoss();
                }
                else
                {
                    if (random.Next(2) == 0) // 50% шанс
                    {
                        FightEnemy();
                    }
                    else
                    {
                        OpenChest();
                    }
                }

                if (player.HP <= 0)
                {
                    Console.WriteLine("\n=== ИГРА ОКОНЧЕНА ===");
                    Console.WriteLine($"Вы продержались {turn} ходов!");
                    break;
                }
            }
        }

        private void FightEnemy()
        {
            Enemy enemy = CreateRandomEnemy();
            Console.WriteLine($"На вас напал {enemy.Name}!");

            while (enemy.IsAlive && player.HP > 0)
            {
                Console.WriteLine("\n--- Ваш ход ---");
                Console.WriteLine("1 - Атаковать");
                Console.WriteLine("2 - Защищаться");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    int damage = player.Attack;
                    enemy.TakeDamage(damage);
                    Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Вы готовитесь к защите...");
                    bool dodged = random.NextDouble() < 0.4; // 40% шанс уклонения

                    if (dodged)
                    {
                        Console.WriteLine("Вы успешно уклонились от атаки!");
                        continue;
                    }
                    else
                    {
                        double blockPercent = 0.7 + random.NextDouble() * 0.3; // 70-100% защиты
                        int blockedDamage = (int)(player.Defense * blockPercent);
                        Console.WriteLine($"Вы блокируете {blockedDamage} урона своей защитой!");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор, пропускаем ход!");
                }

                if (!enemy.IsAlive)
                {
                    Console.WriteLine($"Вы победили {enemy.Name}!");
                    break;
                }

                // Ход врага
                Console.WriteLine($"\n--- Ход {enemy.Name} ---");

                if (enemy is Mage || enemy is ArchmageCPlusPlus || enemy is PestovCMinusMinus)
                {
                    if (enemy.TryFreeze())
                    {
                        Console.WriteLine($"{enemy.Name} замораживает вас! Вы пропустите следующий ход.");
                        player.IsFrozen = true;
                    }
                }

                int enemyDamage = enemy.CalculateDamage(player);
                player.TakeDamage(enemyDamage);
                Console.WriteLine($"{enemy.Name} наносит вам {enemyDamage} урона!");

                if (player.HP <= 0)
                {
                    Console.WriteLine("Вы погибли в бою...");
                    break;
                }

                enemy.DisplayStats();
            }
        }

        private void FightBoss()
        {
            Enemy[] bosses = { new VVG(), new Kovalsky(), new ArchmageCPlusPlus(), new PestovCMinusMinus() };
            Enemy boss = bosses[random.Next(bosses.Length)];

            Console.WriteLine($"На вас напал {boss.Name}!");
            boss.DisplayStats();

            while (boss.IsAlive && player.HP > 0)
            {
                Console.WriteLine("\n--- Ваш ход ---");
                Console.WriteLine("1 - Атаковать");
                Console.WriteLine("2 - Защищаться");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    int damage = player.Attack;
                    boss.TakeDamage(damage);
                    Console.WriteLine($"Вы нанесли {damage} урона {boss.Name}!");
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Вы готовитесь к защите...");
                    bool dodged = random.NextDouble() < 0.4;

                    if (dodged)
                    {
                        Console.WriteLine("Вы успешно уклонились от атаки!");
                        continue;
                    }
                    else
                    {
                        double blockPercent = 0.7 + random.NextDouble() * 0.3;
                        int blockedDamage = (int)(player.Defense * blockPercent);
                        Console.WriteLine($"Вы блокируете {blockedDamage} урона своей защитой!");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор, пропускаем ход!");
                }

                if (!boss.IsAlive)
                {
                    Console.WriteLine($"Вы победили {boss.Name}!");
                    break;
                }

                // Ход босса
                Console.WriteLine($"\n--- Ход {boss.Name} ---");

                if (boss is Mage || boss is ArchmageCPlusPlus || boss is PestovCMinusMinus)
                {
                    if (boss.TryFreeze())
                    {
                        Console.WriteLine($"{boss.Name} замораживает вас! Вы пропустите следующий ход.");
                        player.IsFrozen = true;
                    }
                }

                int bossDamage = boss.CalculateDamage(player);
                player.TakeDamage(bossDamage);
                Console.WriteLine($"{boss.Name} наносит вам {bossDamage} урона!");

                if (player.HP <= 0)
                {
                    Console.WriteLine("Вы погибли в бою с боссом...");
                    break;
                }

                boss.DisplayStats();
            }
        }

        private Enemy CreateRandomEnemy()
        {
            int enemyType = random.Next(3);
            return enemyType switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                2 => new Mage(),
                _ => new Goblin()
            };
        }

        private void OpenChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            int itemType = random.Next(3);

            switch (itemType)
            {
                case 0: // Зелье здоровья
                    HealthPotion potion = new HealthPotion();
                    Console.WriteLine($"В сундуке: {potion.GetStats()}");
                    player.HealFull();
                    Console.WriteLine("Ваше здоровье полностью восстановлено!");
                    break;

                case 1: // Оружие
                    Weapon newWeapon = weapons[random.Next(weapons.Count)];
                    Console.WriteLine($"В сундуке: {newWeapon.GetStats()}");
                    Console.WriteLine($"Ваше текущее оружие: {player.EquippedWeapon.GetStats()}");
                    Console.Write("Взять новое оружие? (1 - да, 2 - нет): ");
                    string weaponChoice = Console.ReadLine();

                    if (weaponChoice == "1")
                    {
                        player.EquipWeapon(newWeapon);
                        Console.WriteLine($"Вы экипировали: {newWeapon.GetStats()}");
                    }
                    else
                    {
                        Console.WriteLine("Вы оставили оружие в сундуке.");
                    }
                    break;

                case 2: // Доспехи
                    Armor newArmor = armors[random.Next(armors.Count)];
                    Console.WriteLine($"В сундуке: {newArmor.GetStats()}");
                    Console.WriteLine($"Ваши текущие доспехи: {player.EquippedArmor.GetStats()}");
                    Console.Write("Взять новые доспехи? (1 - да, 2 - нет): ");
                    string armorChoice = Console.ReadLine();

                    if (armorChoice == "1")
                    {
                        player.EquipArmor(newArmor);
                        Console.WriteLine($"Вы экипировали: {newArmor.GetStats()}");
                    }
                    else
                    {
                        Console.WriteLine("Вы оставили доспехи в сундуке.");
                    }
                    break;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}