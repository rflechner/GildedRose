namespace GildedRose.Console.Dsl
{
    public static class Extensions
    {
        public static bool IsBetween(this int i, int min, int max)
        {
            return i > min && i < max;
        }
        
    }
}