using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick_MAUI
{
    /// <summary>
    /// 响应式平台扩展方法，为MAUI平台提供响应式编程支持
    /// Reactive platform extensions providing reactive programming support for MAUI platform
    /// </summary>
    public static class ReactivePlatformExtensions
    {

        /// <summary>
        /// 在指定调度器上订阅Observable并执行操作
        /// Subscribes to Observable on specified dispatcher and executes action
        /// </summary>
        /// <typeparam name="T">Observable元素类型 / Observable element type</typeparam>
        /// <param name="source">源Observable / Source Observable</param>
        /// <param name="dispatcher">调度器 / Dispatcher</param>
        /// <param name="onNext">下一个元素的处理动作 / Action for next element</param>
        /// <returns>可释放的订阅 / Disposable subscription</returns>
        public static IDisposable SubscribeOnDispatcher<T>(this IObservable<T> source, IDispatcher dispatcher, Action<T> onNext)
        {
            return source.Subscribe(async e => await dispatcher.DispatchAsync(() => onNext(e)));
        }

        /// <summary>
        /// 当视觉元素卸载时自动释放资源
        /// Automatically disposes resource when visual element is unloaded
        /// </summary>
        /// <param name="disposable">可释放对象 / Disposable object</param>
        /// <param name="element">视觉元素 / Visual element</param>
        public static void DisposeWhenUnload(this IDisposable disposable, VisualElement element)
        {
            element.Unloaded += async (s, e) =>
            {
                await element.Dispatcher.DispatchAsync(() => disposable.Dispose());
            };


        }
    }
}
