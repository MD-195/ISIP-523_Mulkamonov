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