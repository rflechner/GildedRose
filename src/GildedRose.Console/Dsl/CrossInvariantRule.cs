using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console.Dsl
{
    public class CrossInvariantRule<TModel> : BusinessRule<TModel>
    {
        private readonly Action<TModel> action;
        private readonly List<Func<TModel, bool>> exceptions;
        private readonly List<Func<TModel, bool>> when;

        public CrossInvariantRule(Action<TModel> action, 
            List<Func<TModel, bool>> exceptions, 
            List<Func<TModel, bool>> when)
        {
            this.action = action;
            this.exceptions = exceptions;
            this.when = when;
        }

        public override void Apply(TModel model)
        {
            if (exceptions.Any(exception => exception(model)))
                return;
            if (when.Count == 0 || when.Any(condition => condition(model)))
                action(model);
        }
    }
}