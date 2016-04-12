namespace GildedRose.Console.Dsl
{
    public class CreateRule
    {
        public static RuleBuilder<T> For<T>()
        {
            return new RuleBuilder<T>();
        }
    }
}