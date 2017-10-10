using System;

namespace GildedRose.Console
{
    public struct Quality
    {
        public Quality(int value, int max = 50)
        {
            if (value < 0 || value > max)
                throw new ArgumentOutOfRangeException("Quality cannot be between 0 and 50");

            Value = value;
        }

        public int Value { get; private set; }

        public static implicit operator int(Quality quality)
        {
            return quality.Value;
        }

        public static implicit operator Quality(int quality)
        {
            return new Quality(quality);
        }

        public void Decrease() => Value--;
        public void Increase() => Value++;
    }

    public class Item
    {
        public Item(string name, int sellIn, Quality quality)
        {
            Quality = quality;
            Name = name;
            SellIn = sellIn;
        }

        public string Name { get; }

        public int SellIn { get; set; }

        public Quality Quality { get; set; }
    }

    public class AgedBrie : Item
    {
        public AgedBrie(int sellIn, int quality) : base("Aged Brie", sellIn, quality)
        {
        }
    }

    public class BackstagePass : Item
    {
        public BackstagePass(int sellIn, int quality) : base("Backstage passes to a TAFKAL80ETC concert", sellIn, quality)
        {
        }
    }

    public class Sulfuras : Item
    {
        public Sulfuras() : base("Sulfuras, Hand of Ragnaros", 0, new Quality(80, 80))
        {
        }
    }


}