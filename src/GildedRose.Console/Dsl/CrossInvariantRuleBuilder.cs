using System;
using System.Collections.Generic;

namespace GildedRose.Console.Dsl
{
    public class CrossInvariantRuleBuilder<T>
    {
        private readonly List<Func<T, bool>> exceptions = new List<Func<T, bool>>();
        private Action<T> action;

        public CrossInvariantRuleBuilder<T> DoAllways(Action<T> action)
        {
            this.action = action;
            return this;
        }

        public CrossInvariantRuleBuilder<T> ExceptedWhen(Func<T, bool> exception)
        {
            exceptions.Add(exception);
            return this;
        }

        private CrossInvariantRule<T> Build()
        {
            return new CrossInvariantRule<T>(action, exceptions);
        }

        public void RegisterTo(BusinessRulesEngine engine)
        {
            Build().RegisterTo(engine);
        }
    }
}