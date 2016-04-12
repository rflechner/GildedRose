using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console.Dsl
{
    public class CrossInvariantRule<TModel> : BusinessRule<TModel>
    {
        private readonly Action<TModel> action;
        private readonly List<Func<TModel, bool>> exceptions;

        public CrossInvariantRule(Action<TModel> action, List<Func<TModel, bool>> exceptions)
        {
            this.action = action;
            this.exceptions = exceptions;
        }

        public override void Apply(TModel model)
        {
            if (exceptions.Any(exception => exception(model)))
                return;
            action(model);
        }
    }
}