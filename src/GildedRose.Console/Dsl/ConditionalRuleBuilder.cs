using System;

namespace GildedRose.Console.Dsl
{
    public class ConditionalRuleBuilder<TId, T>
    {
        private readonly Func<T, TId> identitiedBy;
        private readonly TId[] ids;
        private Func<T, bool> when;

        public ConditionalRuleBuilder(TId[] ids, Func<T, TId> selector)
        {
            this.ids = ids;
            identitiedBy = selector;
        }

        public ConditionalRuleBuilder<TId, T> When(Func<T, bool> condition)
        {
            when = condition;
            return this;
        }

        public ConditionalRule<TId, T> Then(Action<T> then)
        {
            if (identitiedBy == null)
                throw new InvalidOperationException("Please use IdentitiedBy before");

            return new ConditionalRule<TId, T>(ids, when, then, identitiedBy);
        }
    }
}