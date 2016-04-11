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