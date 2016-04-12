namespace GildedRose.Console.Dsl
{
    public class RuleBuilder<T>
    {
        public RuleForIdBuider<TId, T> HavingIds<TId>(params TId[] ids)
        {
            return new RuleForIdBuider<TId, T>(ids);
        }

        public CrossInvariantRuleBuilder<T> All()
        {
            return new CrossInvariantRuleBuilder<T>();
        }
    }
}