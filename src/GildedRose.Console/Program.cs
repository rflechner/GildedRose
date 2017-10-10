using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        public IList<Item> Items;

        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program()
                          {
                              Items = new List<Item>
                                          {
                                              new Item ("+5 Dexterity Vest",10,20),
                                              new Item ("Aged Brie", 2, 0),
                                              new Item ("Elixir of the Mongoose", 5, 7),
                                              new Sulfuras(),
                                              new Item ("Backstage passes to a TAFKAL80ETC concert", 15, 20),
                                              new Item ("Conjured Mana Cake", 3, 6)
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                item.Update();
            }
        }

    }
}
