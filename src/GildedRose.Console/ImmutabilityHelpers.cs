using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GildedRose.Console
{
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
                Expression expression = getter.Body;
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