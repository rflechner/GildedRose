using System;
using System.Collections.Generic;

namespace GildedRose.Console.Dsl
{
    public class BusinessRulesEngine
    {
        private readonly Dictionary<Type, List<BusinessRule>> rules = new Dictionary<Type, List<BusinessRule>>();

        public void Register<T>(BusinessRule<T> rule)
        {
            var type = typeof(T);
            if (!rules.ContainsKey(type))
                rules.Add(type, new List<BusinessRule>());
            rules[type].Add(rule);
        }

        public void ApplyRulesOn<T>(IEnumerable<T> models)
        {
            foreach (var model in models)
                ApplyRulesOn(model);
        }

        public void ApplyRulesOn<T>(T model)
        {
            var type = typeof(T);
            if (!rules.ContainsKey(type))
                return;
            var businessRules = rules[type];
            foreach (var businessRule in businessRules)
                businessRule.Apply(model);
        }
    }
}