using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console
{
    public class InventoryState
    {
        public InventoryState(IList<Item> items)
        {
            Items = items;
        }
        
        public IList<Item> Items { get; }

        public Item AgedBrie => Items.Single(i => i.Name == Inventory.ItemNames.AgedBrie);
        public Item ConjuredManaCake => Items.Single(i => i.Name == Inventory.ItemNames.ConjuredManaCake);
        public Item DexterityVest => Items.Single(i => i.Name == Inventory.ItemNames.DexterityVest);
        public Item ElixirOfMongoose => Items.Single(i => i.Name == Inventory.ItemNames.ElixirOfMongoose);
        public Item Sulfuras => Items.Single(i => i.Name == Inventory.ItemNames.Sulfuras);
        public Item BackstageTafkal80Etc => Items.Single(i => i.Name == Inventory.ItemNames.BackstageTafkal80Etc);

    }
}