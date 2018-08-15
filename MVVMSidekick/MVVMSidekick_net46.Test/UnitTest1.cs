using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.ViewModels;

namespace MVVMSidekick_net46.Test
{
    [TestClass]
    public class UnitTest1
    {



        public class SomeBindable : BindableBase<SomeBindable>
        {
            public SomeBindable()
            {
                // Use propery to init value here:
                if (IsInDesignMode)
                {
                    //Add design time test data init here. These will not execute in runtime.
                }
            }


            public int MyProperty { get => _MyPropertyLocator(this).Value; set => _MyPropertyLocator(this).SetValueAndTryNotify(value); }
            #region Property int MyProperty Setup        
            protected Property<int> _MyProperty = new Property<int>(_MyPropertyLocator);
            static Func<BindableBase, ValueContainer<int>> _MyPropertyLocator = RegisterContainerLocator(nameof(MyProperty), m => m.Initialize(nameof(MyProperty), ref m._MyProperty, ref _MyPropertyLocator, () => default(int)));
            #endregion

            //Use propvm + tab +tab  to create a new property of bindable here
        }


        public class SomeViewModel : ViewModelBase<SomeViewModel>
        {
            public SomeViewModel()
            {

                if (IsInDesignMode)
                {
                    //Add design time mock data init here. These will not execute in runtime.
                }
            }

            //Use propvm + tab +tab  to create a new property of vm here:
            //Use propcvm + tab +tab  to create a new property of vm  with complex default value factory here:
            //Use propcmd + tab +tab  to create a new command of vm here:

            public int MyProperty { get => _MyPropertyLocator(this).Value; set => _MyPropertyLocator(this).SetValueAndTryNotify(value); }
            #region Property int MyProperty Setup        
            protected Property<int> _MyProperty = new Property<int>(_MyPropertyLocator);
            static Func<BindableBase, ValueContainer<int>> _MyPropertyLocator = RegisterContainerLocator(nameof(MyProperty), m => m.Initialize(nameof(MyProperty), ref m._MyProperty, ref _MyPropertyLocator, () => default(int)));
            #endregion


            public int MyProperty123
            {
                get { return _MyProperty123Locator(this).Value; }
                set { _MyProperty123Locator(this).SetValueAndTryNotify(value); }
            }
            #region Property int MyProperty123 Setup        
            protected Property<int> _MyProperty123 = new Property<int>(_MyProperty123Locator);
            static Func<BindableBase, ValueContainer<int>> _MyProperty123Locator = RegisterContainerLocator("MyProperty123", (m, pn) => m.Initialize(pn, ref m._MyProperty123, ref _MyProperty123Locator, () => default(int)));
            #endregion

        }


        [TestMethod]
        public void TestBindableModelLetValue()
        {
            var s = new SomeBindable();
            s.MyProperty = 123;
            Assert.AreEqual(123, s.MyProperty);
        }


        [TestMethod]
        public void TestViewModelLetValue()
        {
            var s = new SomeViewModel();
            s.MyProperty = 123;
            s.MyProperty123 = 123;

            Assert.AreEqual(123, s.MyProperty);
            Assert.AreEqual(123, s.MyProperty123);
        }
    }
}
