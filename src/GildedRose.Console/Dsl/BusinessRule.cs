using System.Text;
using System.Threading.Tasks;

namespace GildedRose.Console.Dsl
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
}
