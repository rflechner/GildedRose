using System;
using System.Linq;
using GildedRose.Console;
using GildedRose.Console.Dsl;
using Xunit;
using Xunit.Sdk;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        [Fact]
        public void When_updating_one_time()
        {
            var state1 = Inventory.InitialState;
            var state2 = state1.UpdateQuality();

            Assert.Equal(state1.AgedBrie.Quality, 0);
            Assert.Equal(state2.AgedBrie.Quality, 1);
            Assert.Equal(state1.AgedBrie.SellIn, 2);
            Assert.Equal(state2.AgedBrie.SellIn, 1);

            Assert.Equal(state1.Sulfuras.SellIn, 0);
            Assert.Equal(state2.Sulfuras.SellIn, 0);
            Assert.Equal(state1.Sulfuras.Quality, 80);
            Assert.Equal(state2.Sulfuras.Quality, 80);

            Assert.Equal(state1.DexterityVest.SellIn, 10);
            Assert.Equal(state2.DexterityVest.SellIn, 9);
            Assert.Equal(state1.DexterityVest.Quality, 20);
            Assert.Equal(state2.DexterityVest.Quality, 19);

            Assert.Equal(state1.ElixirOfMongoose.SellIn, 5);
            Assert.Equal(state2.ElixirOfMongoose.SellIn, 4);
            Assert.Equal(state1.ElixirOfMongoose.Quality, 7);
            Assert.Equal(state2.ElixirOfMongoose.Quality, 6);

            Assert.Equal(state1.BackstageTafkal80Etc.SellIn, 15);
            Assert.Equal(state2.BackstageTafkal80Etc.SellIn, 14);
            Assert.Equal(state1.BackstageTafkal80Etc.Quality, 20);
            Assert.Equal(state2.BackstageTafkal80Etc.Quality, 21);

            Assert.Equal(state1.ConjuredManaCake.SellIn, 3);
            Assert.Equal(state2.ConjuredManaCake.SellIn, 2);
            Assert.Equal(state1.ConjuredManaCake.Quality, 6);
            Assert.Equal(state2.ConjuredManaCake.Quality, 5);
        }

        [Fact]
        public void When_updating_Five_time()
        {
            var state1 = Inventory.InitialState;
            //var state1 = new InventoryState(Inventory.InitialState.Items.Where(item => item.Name == Inventory.ItemNames.ConjuredManaCake).ToList());
            var state2 = state1
                .UpdateQuality()
                .UpdateQuality()
                .UpdateQuality()
                .UpdateQuality()
                .UpdateQuality();

            Assert.Equal(state1.AgedBrie.Quality, 0);
            Assert.Equal(state2.AgedBrie.Quality, 8);
            Assert.Equal(state1.AgedBrie.SellIn, 2);
            Assert.Equal(state2.AgedBrie.SellIn, -3);

            Assert.Equal(state1.Sulfuras.SellIn, 0);
            Assert.Equal(state2.Sulfuras.SellIn, 0);
            Assert.Equal(state1.Sulfuras.Quality, 80);
            Assert.Equal(state2.Sulfuras.Quality, 80);

            Assert.Equal(state1.DexterityVest.SellIn, 10);
            Assert.Equal(state2.DexterityVest.SellIn, 5);
            Assert.Equal(state1.DexterityVest.Quality, 20);
            Assert.Equal(state2.DexterityVest.Quality, 15);

            Assert.Equal(state1.ElixirOfMongoose.SellIn, 5);
            Assert.Equal(state2.ElixirOfMongoose.SellIn, 0);
            Assert.Equal(state1.ElixirOfMongoose.Quality, 7);
            Assert.Equal(state2.ElixirOfMongoose.Quality, 2);

            Assert.Equal(state1.BackstageTafkal80Etc.SellIn, 15);
            Assert.Equal(state2.BackstageTafkal80Etc.SellIn, 10);
            Assert.Equal(state1.BackstageTafkal80Etc.Quality, 20);
            Assert.Equal(state2.BackstageTafkal80Etc.Quality, 25);

            Assert.Equal(state1.ConjuredManaCake.SellIn, 3);
            Assert.Equal(state2.ConjuredManaCake.SellIn, -2);
            Assert.Equal(state1.ConjuredManaCake.Quality, 6);
            Assert.Equal(state2.ConjuredManaCake.Quality, 0);
        }

        [Fact]
        public void When_cloning_item()
        {
            var brie = Inventory.InitialState.AgedBrie;
            var s2 = brie
                .With(s => s.Quality, 200)
                .And(s => s.Name, "popo")
                .Clone();

            Assert.NotEqual(brie.Name, "popo");
            Assert.Equal(s2.Name, "popo");
        }

        [Fact]
        public void When_ConditionalRule_should_be_executed()
        {
            var rule = CreateRule.For<Item>()
                .HavingIds(Inventory.ItemNames.AgedBrie)
                .IdentitiedBy(m => m.Name)
                .When(item => item.SellIn == 2)
                .Then(item => item.Quality = 1);

            var item1 = new Item
            {
                Quality = 4,
                SellIn = 2,
                Name = Inventory.ItemNames.AgedBrie
            };

            rule.Apply(item1);

            Assert.Equal(1, item1.Quality);
        }

        [Fact]
        public void When_ConditionalRule_should_not_be_executed()
        {
            var rule = CreateRule.For<Item>()
                .HavingIds(Inventory.ItemNames.AgedBrie)
                .IdentitiedBy(m => m.Name)
                .When(item => item.SellIn == 2)
                .Then(item => item.Quality = 1);

            var item1 = new Item
            {
                Quality = 4,
                SellIn = 7,
                Name = Inventory.ItemNames.AgedBrie
            };

            rule.Apply(item1);

            Assert.Equal(4, item1.Quality);
        }

        [Fact]
        public void When_using_BusinessRulesEngine_to_describe_domain_rules()
        {
            var engine = new BusinessRulesEngine();
            
            Func<Item, string> id = item => item.Name;
            
            CreateRule.For<Item>().All()
                .Then(item => item.SellIn--)
                .ExceptedWhen(item => item.Name == Inventory.ItemNames.Sulfuras)
                .RegisterTo(engine);

            CreateRule.For<Item>().All()
                .Then(item => item.Quality--)
                .ExceptedWhen(item => new []
                {
                    Inventory.ItemNames.AgedBrie,
                    Inventory.ItemNames.Sulfuras,
                    Inventory.ItemNames.BackstageTafkal80Etc
                }.Contains(item.Name))
                .RegisterTo(engine);

            CreateRule.For<Item>()
                .HavingIds(Inventory.ItemNames.AgedBrie, Inventory.ItemNames.BackstageTafkal80Etc)
                .IdentitiedBy(id)
                .Then(item => item.Quality++)
                .RegisterTo(engine);
            
            var state1 = Inventory.InitialState;
            var state2 = Inventory.InitialState;
            engine.ApplyRulesOn<Item>(state2.Items);

            Assert.Equal(state1.AgedBrie.Quality, 0);
            Assert.Equal(state2.AgedBrie.Quality, 1);
            Assert.Equal(state1.AgedBrie.SellIn, 2);
            Assert.Equal(state2.AgedBrie.SellIn, 1);

            Assert.Equal(state1.Sulfuras.SellIn, 0);
            Assert.Equal(state2.Sulfuras.SellIn, 0);
            Assert.Equal(state1.Sulfuras.Quality, 80);
            Assert.Equal(state2.Sulfuras.Quality, 80);

            Assert.Equal(state1.DexterityVest.SellIn, 10);
            Assert.Equal(state2.DexterityVest.SellIn, 9);
            Assert.Equal(state1.DexterityVest.Quality, 20);
            Assert.Equal(state2.DexterityVest.Quality, 19);

            Assert.Equal(state1.ElixirOfMongoose.SellIn, 5);
            Assert.Equal(state2.ElixirOfMongoose.SellIn, 4);
            Assert.Equal(state1.ElixirOfMongoose.Quality, 7);
            Assert.Equal(state2.ElixirOfMongoose.Quality, 6);

            Assert.Equal(state1.BackstageTafkal80Etc.SellIn, 15);
            Assert.Equal(state2.BackstageTafkal80Etc.SellIn, 14);
            Assert.Equal(state1.BackstageTafkal80Etc.Quality, 20);
            Assert.Equal(state2.BackstageTafkal80Etc.Quality, 21);

            Assert.Equal(state1.ConjuredManaCake.SellIn, 3);
            Assert.Equal(state2.ConjuredManaCake.SellIn, 2);
            Assert.Equal(state1.ConjuredManaCake.Quality, 6);
            Assert.Equal(state2.ConjuredManaCake.Quality, 5);

        }
    }
}