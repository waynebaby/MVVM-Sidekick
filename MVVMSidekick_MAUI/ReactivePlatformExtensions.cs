using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick_MAUI
{
    public static class ReactivePlatformExtensions
    {

        public static IDisposable SubscribeOnDispatcher<T>(this IObservable<T> source, IDispatcher dispatcher, Action<T> onNext)
        {
            return source.Subscribe(async e => await dispatcher.DispatchAsync(() => onNext(e)));
        }


        public static void DisposeWhenUnload(this IDisposable disposable, VisualElement element)
        {
            element.Unloaded += async (s, e) =>
            {
                await element.Dispatcher.DispatchAsync(() => disposable.Dispose());
            };


        }
    }
}
