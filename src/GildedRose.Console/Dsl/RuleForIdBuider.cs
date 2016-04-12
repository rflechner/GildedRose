using System;

namespace GildedRose.Console.Dsl
{
    public class RuleForIdBuider<TId, T>
    {
        private readonly TId[] ids;

        public RuleForIdBuider(TId[] ids)
        {
            this.ids = ids;
        }

        public ConditionalRuleBuilder<TId, T> IdentitiedBy(Func<T, TId> selector)
        {
            return new ConditionalRuleBuilder<TId, T>(ids, selector);
        }
    }
}