using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Assert.AreEqual (count,1);

            ob.Remove(3);

            Assert.AreEqual(count, 2);

        
        }

    }
}
