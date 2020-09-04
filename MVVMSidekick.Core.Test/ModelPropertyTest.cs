using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System;
using System.Threading;

namespace MVVMSidekick.Core.Test
{
    [TestClass]
    public class ModelPropertyTest
    {
        [TestMethod]
        public async Task BasicModelPropertyTestAsync()
        {
            var m = new TestTargetModel();

            var hasChanged = new TaskCompletionSource<bool>();
            m.IsNotificationActivated = true;
            await Task.Delay(200);
            m.ListenValueChangedEvents(x => x.IntPropery1)
                 .Subscribe(e =>
                 {
                     hasChanged.TrySetResult(true);
                 });


            Assert.AreEqual(m.IntPropery1, 8);
            m.IntPropery1 = 5;

            Assert.AreEqual(m.IntPropery1, 5);

            Assert.AreEqual(m.GetValueContainer(x => x.IntPropery1).Value, 5);



            var vt = await Task.WhenAny(Task.Delay(3000).ContinueWith(x => Task.Delay(3000)).ContinueWith(x => false), hasChanged.Task);

            Assert.IsTrue(await vt);
        }
    }
}
