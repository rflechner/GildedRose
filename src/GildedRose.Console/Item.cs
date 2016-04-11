using System;

namespace GildedRose.Console
{
    public class Item : ICloneable
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }

        object ICloneable.Clone()
        {
            return new Item
            {
                Name = Name,
                Quality = Quality,
                SellIn = SellIn
            };
        }

        public Item Clone()
        {
            return (Item)((ICloneable)this).Clone();
        }
    }
}