using System.Collections.Generic;

namespace GildedRose.Console
{
    public static class Inventory
    {
        public class ItemNames
        {
            public const string DexterityVest = "+5 Dexterity Vest";
            public const string AgedBrie = "Aged Brie";
            public const string ElixirOfMongoose = "Elixir of the Mongoose";
            public const string Sulfuras = "Sulfuras, Hand of Ragnaros";
            public const string BackstageTafkal80Etc = "Backstage passes to a TAFKAL80ETC concert";
            public const string ConjuredManaCake = "Conjured Mana Cake";
        }

        public static InventoryState InitialState
        {
            get
            {
                var items = new List<Item>
                {
                    new Item {Name = ItemNames.DexterityVest, SellIn = 10, Quality = 20},
                    new Item {Name = ItemNames.AgedBrie, SellIn = 2, Quality = 0},
                    new Item {Name = ItemNames.ElixirOfMongoose, SellIn = 5, Quality = 7},
                    new Item {Name = ItemNames.Sulfuras, SellIn = 0, Quality = 80},
                    new Item
                    {
                        Name = ItemNames.BackstageTafkal80Etc,
                        SellIn = 15,
                        Quality = 20
                    },
                    new Item {Name = ItemNames.ConjuredManaCake, SellIn = 3, Quality = 6}
                };

                return new InventoryState(items);
            }
        }

        public static InventoryState UpdateQuality(this InventoryState state)
        {
            var nextState = new List<Item>();
            foreach (var i in state.Items)
            {
                var item = i.Clone();
                if (item.Name != ItemNames.AgedBrie && item.Name != ItemNames.BackstageTafkal80Etc)
                {
                    if (item.Quality > 0)
                    {
                        if (item.Name != ItemNames.Sulfuras)
                        {
                            item.Quality = item.Quality - 1;
                        }
                    }
                }
                else
                {
                    if (item.Quality < 50)
                    {
                        item.Quality = item.Quality + 1;

                        if (item.Name == ItemNames.BackstageTafkal80Etc)
                        {
                            if (item.SellIn < 11)
                            {
                                if (item.Quality < 50)
                                {
                                    item.Quality = item.Quality + 1;
                                }
                            }

                            if (item.SellIn < 6)
                            {
                                if (item.Quality < 50)
                                {
                                    item.Quality = item.Quality + 1;
                                }
                            }
                        }
                    }
                }

                if (item.Name != ItemNames.Sulfuras)
                {
                    item.SellIn = item.SellIn - 1;
                }

                if (item.SellIn < 0)
                {
                    if (item.Name != ItemNames.AgedBrie)
                    {
                        if (item.Name != ItemNames.BackstageTafkal80Etc)
                        {
                            if (item.Quality > 0)
                            {
                                if (item.Name != ItemNames.Sulfuras)
                                {
                                    item.Quality = item.Quality - 1;
                                }
                            }
                        }
                        else
                        {
                            item.Quality = item.Quality - item.Quality;
                        }
                    }
                    else
                    {
                        if (item.Quality < 50)
                        {
                            item.Quality = item.Quality + 1;
                        }
                    }
                }
                nextState.Add(item);
            }

            return new InventoryState(nextState);
        }
    }
}