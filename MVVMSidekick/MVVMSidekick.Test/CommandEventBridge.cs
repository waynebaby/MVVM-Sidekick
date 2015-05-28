using System;
#if NETFX_CORE

using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif WINDOWS_PHONE_8
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Windows.Controls;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Windows.Controls;
#endif
using MVVMSidekick.Collections;
using System.Collections.Generic;
using MVVMSidekick.Patterns;
using MVVMSidekick.Patterns.ItemsAndSelection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using MVVMSidekick.EventRouting;


namespace MVVMSidekick.Test
{
    public class AnyObjectWithEvent
    {
        public event EventHandler<string> SampleEvent;

        public void RaiseEvent(string data)
        {
            if (SampleEvent != null)
            {
                SampleEvent(this, data);
            }

        }




    }

    [TestClass]
    public class CommandEventBridge
    {


        [TestMethod]
        public void TestBindingNormalEvent()
        {
            var eobj = new AnyObjectWithEvent();
            string data = "";

            string expected = "Mydata";

            var bind = MVVMSidekick.Utilities.EventHandlerHelper.BindEvent(eobj, "SampleEvent", (o, e,en,eht) => {
				data = (string)e;
			});
            eobj.RaiseEvent(expected);
            Assert.AreEqual(data, expected);

            bind.Dispose();
            eobj.RaiseEvent("");
            Assert.AreEqual(data, expected);

        }


#if NETFX_CORE

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void TestBindingRTEvent()
        {
            int count = 0;

            var eobj2 = new TextBox ();
            var bind2 = MVVMSidekick.Utilities.EventHandlerHelper.BindEvent(eobj2, "TextChanged",
                (o, e,en,eht) => {
                    count++;
                });
     

        }


#endif
    }
}
