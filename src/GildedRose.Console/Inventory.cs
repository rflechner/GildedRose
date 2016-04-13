using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.Console.Dsl;

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
            var specifications = new BusinessRulesEngine();

            Func<Item, string> id = item => item.Name;

            var itemsExceptedSulfurasDecreaseSellIn =
                CreateRule.For<Item>().All()
                .ExceptedWhen(item => item.Name == ItemNames.Sulfuras)
                .Then(item => item.SellIn--);

            var normalItemsDecreaseInQuality = 
                CreateRule.For<Item>().All()
                .ExceptedWhen(item => new[]
                {
                    ItemNames.AgedBrie,
                    ItemNames.Sulfuras,
                    ItemNames.BackstageTafkal80Etc
                }.Contains(item.Name))
                .When(item => item.SellIn >= 0)
                .Then(item => item.Quality--);
            
            var dateHhasPassedQualityDegradesTwiceAsFast = 
                CreateRule.For<Item>().All()
                .ExceptedWhen(item => new[]
                {
                    ItemNames.AgedBrie,
                    ItemNames.Sulfuras,
                    ItemNames.BackstageTafkal80Etc
                }.Contains(item.Name))
                .When(item => item.SellIn < 0)
                .Then(item => item.Quality-=2);
            
            var agedBrieIncreaseInQuality =
                CreateRule.For<Item>()
                    .HavingIds(ItemNames.AgedBrie)
                    .IdentitiedBy(id)
                    .When(item => item.SellIn >= 0)
                    .Then(item => item.Quality ++);
            
            var agedBrieIncreaseInQualityTwiceAsFastAfterSellin = 
                CreateRule.For<Item>()
                    .HavingIds(ItemNames.AgedBrie)
                    .IdentitiedBy(id)
                    .When(item => item.SellIn < 0)
                    .Then(item => item.Quality += 2);

            var backstageIncreaseInQuality =
                CreateRule.For<Item>()
                    .HavingIds(ItemNames.BackstageTafkal80Etc)
                    .IdentitiedBy(id)
                    .When(item => item.SellIn > 6)
                    .Then(item => item.Quality += 1);

            var backstageIncreaseby2WhenThereAre10daysOrLess =
                CreateRule.For<Item>()
                    .HavingIds(ItemNames.BackstageTafkal80Etc)
                    .IdentitiedBy(id)
                    .When(item => item.SellIn.IsBetween(0, 6))
                    .Then(item => item.Quality += 2);

            var backstageIncreaseby3WhenThereAre5daysOrLess =
                CreateRule.For<Item>()
                    .HavingIds(ItemNames.BackstageTafkal80Etc)
                    .IdentitiedBy(id)
                    .When(item => item.SellIn < 6)
                    .Then(item => item.Quality += 3);

            var backstageDropsTo0AfterTheConcert = 
                CreateRule.For<Item>()
                .HavingIds(ItemNames.BackstageTafkal80Etc)
                .IdentitiedBy(id)
                .When(item => item.SellIn < 0)
                .Then(item => item.Quality = 0);

            var qualityMaxValueIs50 = 
                CreateRule.For<Item>().All()
                    .ExceptedWhen(item => item.Name == ItemNames.Sulfuras)
                    .When(item => item.Quality > 50)
                    .Then(item => item.Quality = 50);

            var qualityMinValueIs0 =
                CreateRule.For<Item>().All()
                    .ExceptedWhen(item => item.Name == ItemNames.Sulfuras)
                    .When(item => item.Quality < 0)
                    .Then(item => item.Quality = 0);

            var nextState = state.Items.Select(i => i.Clone()).ToList();

            itemsExceptedSulfurasDecreaseSellIn.RegisterTo(specifications);
            normalItemsDecreaseInQuality.RegisterTo(specifications);
            dateHhasPassedQualityDegradesTwiceAsFast.RegisterTo(specifications);
            agedBrieIncreaseInQuality.RegisterTo(specifications);
            agedBrieIncreaseInQualityTwiceAsFastAfterSellin.RegisterTo(specifications);
            backstageIncreaseInQuality.RegisterTo(specifications);
            backstageIncreaseby2WhenThereAre10daysOrLess.RegisterTo(specifications);
            backstageIncreaseby3WhenThereAre5daysOrLess.RegisterTo(specifications);
            backstageDropsTo0AfterTheConcert.RegisterTo(specifications);
            qualityMaxValueIs50.RegisterTo(specifications);
            qualityMinValueIs0.RegisterTo(specifications);

            specifications.ApplyRulesOn<Item>(nextState);
            
            return new InventoryState(nextState);
        }
    }
}