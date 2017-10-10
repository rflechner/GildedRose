using System;
using System.Data;

namespace GildedRose.Console
{
    public class Quality
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

        public virtual void Update()
        {
            SellIn = SellIn - 1;

            if (Quality > 0)
            {
                Quality = Quality - 1;
            }
            
            if (SellIn < 0)
            {
                if (Quality > 0)
                {
                    Quality = Quality - 1;
                }
            }
        }
    }

    public class AgedBrie : Item
    {
        public AgedBrie(int sellIn, int quality) : base("Aged Brie", sellIn, quality)
        {
        }

        public override void Update()
        {
            SellIn = SellIn - 1;

            if (Quality < 50)
            {
                Quality = Quality + 1;
            }

            if (SellIn < 0)
            {
                if (Quality < 50)
                {
                    Quality = Quality + 1;
                }
            }
        }
    }

    public class BackstagePass : Item
    {
        public BackstagePass(int sellIn, int quality) : base("Backstage passes to a TAFKAL80ETC concert", sellIn, quality)
        {
        }

        public override void Update()
        {
            if (Quality < 50)
            {
                Quality.Increase();

                if (SellIn < 11)
                {
                    if (Quality < 50)
                    {
                        Quality.Increase();
                    }
                }

                if (SellIn < 6)
                {
                    if (Quality < 50)
                    {
                        Quality.Increase();
                    }
                }
            }

            SellIn = SellIn - 1;

            if (SellIn < 0)
            {
                Quality = Quality - Quality;
            }
        }
    }

    public class Sulfuras : Item
    {
        public Sulfuras() : base("Sulfuras, Hand of Ragnaros", 0, new Quality(80, 80))
        {
        }

        public override void Update()
        {
            
        }
    }


}