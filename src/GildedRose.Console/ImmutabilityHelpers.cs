using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GildedRose.Console
{
    public abstract class BusinessRule
    {
        internal abstract void Apply(object model);
    }

    public abstract class BusinessRule<TModel> : BusinessRule
    {
        internal override void Apply(object model)
        {
            Apply((TModel)model);
        }

        public abstract void Apply(TModel model);

        public void RegisterTo(BusinessRulesEngine engine)
        {
            engine.Register(this);
        }
    }

    public abstract class BusinessRuleForId<TId, TModel> : BusinessRule<TModel>
    {
        readonly Func<TModel, TId> identitiedBy;

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

    public class ConditionalRule<TId, TModel> : BusinessRuleForId<TId, TModel>
    {
        private readonly Func<TModel, bool> when;
        private readonly Action<TModel> then;

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

    public class BusinessRulesEngine
    {
        private readonly Dictionary<Type, List<BusinessRule>> rules = new Dictionary<Type, List<BusinessRule>>();

        public void Register<T>(BusinessRule<T> rule)
        {
            var type = typeof (T);
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

    public class CreateRule
    {
        public static RuleBuilder<T> For<T>()
        {
            return new RuleBuilder<T>();
        }
    }

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

    public class CrossInvariantRuleBuilder<T>
    {
        private Action<T> action;
        private readonly List<Func<T, bool>> exceptions = new List<Func<T, bool>>();

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

    public class ConditionalRuleBuilder<TId, T>
    {
        private readonly TId[] ids;
        private Func<T, bool> when;
        private readonly Func<T, TId> identitiedBy;
        
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

    public static class ImmutabilityHelpers
    {
        public class CloneBuilder<T>
        {
            private readonly T clone;

            public CloneBuilder(T clone)
            {
                this.clone = clone;
            }

            internal void Assign<TProperty>(Expression<Func<T, TProperty>> getter, TProperty value)
            {
                var expression = getter.Body;
                var memberExpression = expression as MemberExpression;
                if (memberExpression == null)
                    throw new ArgumentException("Please provide a property", "getter");

                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo == null)
                    return;
                var setMethod = propertyInfo.GetSetMethod();
                setMethod.Invoke(clone, new object[] { value });
            }

            public CloneBuilder<T> And<TProperty>(Expression<Func<T, TProperty>> getter, TProperty value)
            {
                Assign(getter, value);
                return this;
            }

            public T Clone()
            {
                return clone;
            }
        }

        public static CloneBuilder<T> With<T, TProperty>(this T o, Expression<Func<T, TProperty>> getter, TProperty value) where T : ICloneable
        {
            var copy = (T)o.Clone();
            var builder = new CloneBuilder<T>(copy);
            builder.Assign(getter, value);

            return builder;
        }
    }
}