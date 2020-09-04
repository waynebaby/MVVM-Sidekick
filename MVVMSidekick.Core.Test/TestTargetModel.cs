using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Core.Test
{

    public class TestTargetModel : BindableBase<TestTargetModel>
    {

        public int IntPropery1 { get => _IntPropery1Locator(this).Value; set => _IntPropery1Locator(this).SetValueAndTryNotify(value); }
        #region Property int IntPropery1 Setup        
        protected Property<int> _IntPropery1 = new Property<int>(_IntPropery1Locator);
        static Func<BindableBase, ValueContainer<int>> _IntPropery1Locator = RegisterContainerLocator(nameof(IntPropery1), m => m.Initialize(nameof(IntPropery1), ref m._IntPropery1, ref _IntPropery1Locator, 8));
        #endregion


    }


}
