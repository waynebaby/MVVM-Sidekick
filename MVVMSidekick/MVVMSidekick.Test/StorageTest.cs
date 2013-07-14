#if WINDOWS_PHONE_8
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

#else

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif
using System;
using System.Text;
using System.Collections.Generic;
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
#if WPF
        [TestMethod]
        public async Task TestDirectoryStorage()
        {

            var hub = MVVMSidekick.Storages.StorageHub<string, string>.CreateJsonDatacontractFileStorageHub(x => x);

            await hub.SaveAsync("aaa", "content");
            var p = Path.Combine(Environment.CurrentDirectory, "aaa");
            Assert.IsTrue (File.Exists (p));

            var newv=await hub.LoadAsync ("aaa" ,false  );
            Assert .AreEqual  (newv, "content");
            newv = await hub.LoadAsync("aaa", true);
            Assert.AreEqual(newv, "content");

        }
#elif WINDOWS_PHONE
        [TestMethod]
        public async Task TestIsolatedStorage()
        {

            var hub = MVVMSidekick.Storages.StorageHub<string, string>.CreateJsonDatacontractIsolatedStorageHub(x => x);

            await hub.SaveAsync("aaa", "content");
            var iso = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
            iso.FileExists("aaa");
            var newv = await hub.LoadAsync("aaa", false);
            Assert.AreEqual(newv, "content");
            newv = await hub.LoadAsync("aaa", true);
            Assert.AreEqual(newv, "content");

        }

#endif
    }
}
