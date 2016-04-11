using GildedRose.Console;
using Xunit;

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
            var s2 = brie.With(s => s.Quality, 200).And(s => s.Name, "popo").Clone();

            Assert.NotEqual(brie.Name, "popo");
            Assert.Equal(s2.Name, "popo");
        }
    }
}