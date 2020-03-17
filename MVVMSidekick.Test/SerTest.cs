using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MVVMSidekick.Test
{
    [TestClass]
    public class SerTest
    {
        [TestMethod]
        public void TestSer()
        {
            var f = new SomeTestingViewModel() {  Result="hahahaha"};

            var dx = new System.Runtime.Serialization.DataContractSerializer(typeof(SomeTestingViewModel));
            var s = new MemoryStream();
            dx.WriteObject(s, f);
            s.Position = 0;
            var s2 = dx.ReadObject(s) as SomeTestingViewModel;
            Assert.AreEqual(s2.Result, f.Result);
        }
    }
}
