using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace MVVMSidekick.Test
{
    /// <summary>
    /// Summary description for StorageTest
    /// </summary>
    [TestClass]
    public class StorageTest
    {
        public StorageTest()
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
        public async Task TestStorage()
        {

            var hub = MVVMSidekick.Storages.StorageHub<string, String>.CreateJsonDatacontractFileStorageHub(x => x);
            await hub.SaveAsync("aaa", "content");
            var p = Path.Combine(Environment.CurrentDirectory, "aaa");
            Assert.IsTrue (File.Exists (p));

            var newv=await hub.LoadAsync ("aaa" ,false  );
            Assert .AreEqual  (newv, "content");
            newv = await hub.LoadAsync("aaa", true);
            Assert.AreEqual(newv, "content");

        }
    }
}
