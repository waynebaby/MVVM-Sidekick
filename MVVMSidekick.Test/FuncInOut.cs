using System;
using System.Text;
using System.Collections.Generic;
#if WINDOWS_PHONE_8||NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif

namespace MVVMSidekick.Test
{
	/// <summary>
	/// Summary description for FuncInOut
	/// </summary>
	[TestClass]
	public class FuncInOut
	{
		public FuncInOut()
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
		public void TestInOut()
		{
			//
			// TODO: Add test logic here
			//
			Func<Bar> a = ()=>new Bar ();
			Func<Foo> b = a;

			var c = b();

			Assert.IsNotNull(c);

		}
		public class Foo
		{
		}

		public class Bar : Foo
		{

		}
	}



}

