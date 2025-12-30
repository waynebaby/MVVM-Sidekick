/// <summary>
/// <para>测试目标模型，用于单元测试的目标模型类</para>
/// <para>Test target model, target model class for unit testing</para>
/// </summary>

using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Core.Test
{
    /// <summary>
    /// <para>测试目标模型类，继承自BindableBase，用于测试绑定功能</para>
    /// <para>Test target model class, inherits from BindableBase, used for testing binding functionality</para>
    /// </summary>
    public class TestTargetModel : BindableBase<TestTargetModel>
    {
        /// <summary>
        /// <para>整数属性1，用于测试整数类型属性的绑定功能</para>
        /// <para>Integer property 1, used for testing binding functionality of integer type properties</para>
        /// </summary>
        public int IntPropery1 { get => _IntPropery1Locator(this).Value; set => _IntPropery1Locator(this).SetValueAndTryNotify(value); }
        #region Property int IntPropery1 Setup        
        protected Property<int> _IntPropery1 = new Property<int>(_IntPropery1Locator);
        static Func<BindableBase, ValueContainer<int>> _IntPropery1Locator = RegisterContainerLocator(nameof(IntPropery1), m => m.Initialize(nameof(IntPropery1), ref m._IntPropery1, ref _IntPropery1Locator, 8));
        #endregion
    }
}
