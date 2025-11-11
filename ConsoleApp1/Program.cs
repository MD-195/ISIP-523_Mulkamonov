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
