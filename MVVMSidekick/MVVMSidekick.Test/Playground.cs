#if WINDOWS_PHONE_8||NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;

#endif

using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace MVVMSidekick.Test
{
    [TestClass]
    public class Playground
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var x = new Dictionary<string, string>();
        //    x.Add("a", "");
        //    x.Add("a", "");
        //}

        [TestMethod]
        public void ObservableCollection_ArgTimes()
        {

            var count = 0;
            var ob = new ObservableCollection<int>() {
            1,2,3,4,5,6,7,8,9

            };

            ob.CollectionChanged += (o, e) =>
                {
                    count++;

                };

            ob.Insert(0, -1);

            Assert.AreEqual(count, 1);

            ob.Remove(3);

            Assert.AreEqual(count, 2);


        }



        //[DataContract(IsReference=true) ] //if you want
        public class SomeViewModel : ViewModelBase<SomeViewModel>
        {
            public SomeViewModel()
            {

                if (IsInDesignMode)
                {
                    //Add design time demo data init here. These will not execute in runtime.
                }
            }

            //Use propvm + tab +tab  to create a new property of vm here:
            //Use propcvm + tab +tab  to create a new property of vm  with complex default value factory here:
            //Use propcmd + tab +tab  to create a new command of vm here:


        }






        [TestMethod]
        public void UnityTestingVM()
        {
            var sl = new Services.UnityServiceLocator();
            var vm1 = sl.Resolve<SomeViewModel>();
            Assert.IsNotNull(vm1.StageManager);
            Assert.IsTrue(vm1.StageManager is TestingStageManager);
        }
    }
}
