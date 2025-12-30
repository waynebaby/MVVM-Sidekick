/// <summary>
/// <para>模型属性测试，提供模型属性功能的单元测试</para>
/// <para>Model property test, providing unit tests for model property functionality</para>
/// </summary>

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
    /// <summary>
    /// <para>模型属性测试类，包含模型属性相关功能的单元测试</para>
    /// <para>Model property test class, containing unit tests for model property related functionality</para>
    /// </summary>
    [TestClass]
    public class ModelPropertyTest
    {
        /// <summary>
        /// <para>基础模型属性测试异步方法</para>
        /// <para>Basic model property test async method</para>
        /// </summary>
        /// <returns>任务等待器 / Task awaiter</returns>
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
