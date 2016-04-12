using System;

namespace GildedRose.Console.Dsl
{
    public class ConditionalRule<TId, TModel> : BusinessRuleForId<TId, TModel>
    {
        private readonly Action<TModel> then;
        private readonly Func<TModel, bool> when;

        public ConditionalRule(TId[] targets,
            Func<TModel, bool> when, Action<TModel> then, Func<TModel, TId> identitiedBy)
            : base(targets, identitiedBy)
        {
            this.when = when;
            this.then = then;
        }

        public override void Apply(TModel model)
        {
            if (!MustExecuteFor(model))
                return;

            if (when != null)
            {
                if (when(model))
                    then(model);
            }
            else
            {
                then(model);
            }
        }
    }
}