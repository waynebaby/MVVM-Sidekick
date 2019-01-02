using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace MVVMSidekick.ViewModels
{

    public class ValueContainerIndexer// : DynamicObject
    {
        public ValueContainerIndexer(IBindable model)
        {
            _model = model;
        }

        IBindable _model;

        public IValueContainer this[string propertyName]
        {
            get
            {
                return _model.GetValueContainer(propertyName);
            }

        }
        

    }



}
