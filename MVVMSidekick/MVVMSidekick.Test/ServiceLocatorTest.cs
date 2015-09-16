using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.Services;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MVVMSidekick.Test
{
    /// <summary>
    /// Summary description for ServiceLocator
    /// </summary>
    [TestClass]
    public class ServiceLocatorTest
    {
        public ServiceLocatorTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public async Task ReactiveCommandTest()
        {
            var c = new ReactiveCommand(true);

            var v = false;
            c.CanExecuteChanged += (o, e) =>
                v = c.CanExecute(null);
            c.ListenCanExecuteObservable(Observable.Delay(Observable.Range(0, 1).Select(xxx => false), DateTimeOffset.Now.AddMinutes(15)));

            await Task.Delay(100);
            Assert.AreEqual(v, true);

        }


        [TestMethod]
        public void TestRegRes()
        {
            //
            // TODO: Add test logic here
            //

            Services.ServiceLocator.Instance.Register(1);
            Assert.AreEqual(1, ServiceLocator.Instance.Resolve<int>());

            Services.ServiceLocator.Instance.Register<IFoo, Foo>();

            var reg = "REF";
            Services.ServiceLocator.Instance.Register(reg);
            Assert.AreEqual(reg, ServiceLocator.Instance.Resolve<IFoo>().Value);



        }

        public interface IFoo
        {
            string Value { get; }
        }

        public class Foo : IFoo
        {

            public Foo(string value)
            {
                _Value = value;
            }
            string _Value;
            public string Value
            {
                get
                {
                    return _Value;
                }
            }
        }


    }
}
