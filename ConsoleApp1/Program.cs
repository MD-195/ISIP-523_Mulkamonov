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