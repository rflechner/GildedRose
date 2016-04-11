using System.Collections.Generic;
using System.Dynamic;

namespace GildedRose.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var state1 = Inventory.InitialState;
            var state2 = state1.UpdateQuality();
            
            System.Console.ReadKey();
        }

        
    }
}
