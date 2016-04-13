using System;
using System.Collections.Generic;

namespace GildedRose.Console.Dsl
{
    public class CrossInvariantRuleBuilder<T>
    {
        private readonly List<Func<T, bool>> exceptions = new List<Func<T, bool>>();
        private readonly List<Func<T, bool>> when = new List<Func<T, bool>>();
        private Action<T> action;

        public CrossInvariantRuleBuilder<T> Then(Action<T> action)
        {
            this.action = action;
            return this;
        }

        public CrossInvariantRuleBuilder<T> ExceptedWhen(Func<T, bool> exception)
        {
            exceptions.Add(exception);
            return this;
        }

        public CrossInvariantRuleBuilder<T> When(Func<T, bool> exception)
        {
            when.Add(exception);
            return this;
        }

        private CrossInvariantRule<T> Build()
        {
            return new CrossInvariantRule<T>(action, exceptions, when);
        }

        public void RegisterTo(BusinessRulesEngine engine)
        {
            Build().RegisterTo(engine);
        }
    }
}