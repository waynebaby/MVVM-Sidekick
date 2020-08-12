using System;
using System.Linq.Expressions;

namespace MVVMSidekick.ViewModels
{
    public struct PropertyValiditionSequenceContext<TModel,TValue>
    {
        public ModelValiditionSequenceContext<TModel> ModelContext { get; set; }
        public Expression<Func<TModel, TValue>> PropertyExpression { get; set; }
        public string PropertyName { get; set; }
    }
}
