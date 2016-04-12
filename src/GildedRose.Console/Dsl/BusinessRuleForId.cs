using System;
using System.Linq;

namespace GildedRose.Console.Dsl
{
    public abstract class BusinessRuleForId<TId, TModel> : BusinessRule<TModel>
    {
        private readonly Func<TModel, TId> identitiedBy;

        protected BusinessRuleForId(TId[] targets, Func<TModel, TId> identitiedBy)
        {
            Targets = targets;
            this.identitiedBy = identitiedBy;
        }

        public TId[] Targets { get; }

        protected bool MustExecuteFor(TModel model)
        {
            var id = identitiedBy(model);
            return Targets.Contains(id);
        }
    }
}