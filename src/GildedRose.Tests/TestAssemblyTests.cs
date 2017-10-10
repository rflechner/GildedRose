using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.Console;
using Xunit;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        [Fact]
        public void WhenItemQualityIsInferiorThanZero_ThenShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Item(String.Empty, 0, -1);
            });
        }

        [Fact]
        public void WhenItemQualityIsGreatherThanZero_ThenShouldNotThrow()
        {
            var item = new Item(String.Empty, 0, 10);

            Assert.Equal(10, item.Quality.Value);
        }

        [Fact]
        public void WhenItemQualityIsGreatherThan50_ThenShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Item(String.Empty, 0, 51);
            });
        }

        [Theory]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(40)]
        public void WhenTimePassed_ThenQualityDecrease(int quality)
        {
            var item = Update(new Item("Elixir of the Mongoose", 5, quality));

            Assert.True(item.Quality.Value >= 0);
            Assert.Equal(Math.Max(0, quality -1), item.Quality.Value);
        }

        [Theory]
        [InlineData(-1, 5)]
        [InlineData(-1, 0)]
        [InlineData(-15, 5)]
        [InlineData(-11, 1)]
        [InlineData(-11, 0)]
        [InlineData(-20, 40)]
        public void WhenSellInPast_ThenDecreaseBy2(int sellIn, int quality)
        {
            var item = Update(new Item("Elixir of the Mongoose", sellIn, quality));

            Assert.Equal(Math.Max(0, quality - 2), item.Quality.Value);
        }

        [Fact]
        public void WhenTimePassed_ThenSulfurasQualityDoesNotDecrease()
        {
            var item = Update(new Sulfuras());
            
            Assert.Equal(80, item.Quality.Value);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(40)]
        public void WhenSellinPassed_ThenQualityDecreaseTwice(int quality)
        {
            var item = Update(new Item("Elixir of the Mongoose", 0, quality));

            Assert.True(item.Quality.Value >= 0);
            Assert.Equal(Math.Max(0, quality -2), item.Quality.Value);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(15)]
        public void WhenAgeBrieSellinPassedAndQualityLowerThan50_ThenQualityIncrease(int quality)
        {
            var item = Update(new AgedBrie(-10, quality));

            Assert.Equal(quality + 2, item.Quality.Value);
        }

        [Fact]
        public void WhenBackstagePassSellinIs6_ThenQualityIncreaseBy2()
        {
            var item = Update(new BackstagePass(6, 10));

            Assert.Equal(12, item.Quality.Value);
        }

        [Fact]
        public void WhenBackstagePassSellinIs4_ThenQualityIncreaseBy2()
        {
            var item = Update(new BackstagePass(4, 10));

            Assert.Equal(13, item.Quality.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-15)]
        public void WhenConcertIsPassed_ThenBackstagePassQualityIs0(int sellIn)
        {
            var item = Update(new BackstagePass(sellIn, 10));

            Assert.Equal(0, item.Quality.Value);
        }

        private static Item Update(Item arg)
        {
            var program = new Program
            {
                Items = new List<Item>
                {
                    arg
                }
            };

            program.UpdateQuality();

            var item = program.Items.Single();
            return item;
        }

        [Theory]
        [InlineData(9)]
        [InlineData(15)]
        public void WhenAgeBrieSellinNotPassed_ThenQualityIncrease(int quality)
        {
            var program = new Program
            {
                Items = new List<Item>
                {
                    new AgedBrie(10, quality)
                }
            };

            program.UpdateQuality();

            Assert.Equal(quality + 1, program.Items.Single().Quality.Value);
        }


        [Theory]
        [InlineData(9)]
        [InlineData(15)]
        public void WhenAgeBrieSellinPassedAndQualityBiggerThan50_ThenQualityIncrease(int quality)
        {
            var program = new Program
            {
                Items = new List<Item>
                {
                    new AgedBrie(10, quality)
                }
            };

            program.UpdateQuality();

            Assert.Equal(quality + 1, program.Items.Single().Quality.Value);
        }

    }
}