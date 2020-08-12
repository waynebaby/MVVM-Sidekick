using System;
using System.Runtime.CompilerServices;

namespace MVVMSidekick.ViewModels
{
#if !BLAZOR
    public static class BindableExtensions
    {



        /// <summary>
        /// <para>注册在目标VM绑定的视图Unload的时候Dispose </para>
        /// <para>Register a dispose object that would dispose when target viewmodel's view unload</para>
        /// </summary>
        /// <typeparam name="T"><para>任意IDisposable对象类型</para><para>some IDisposable Type</para></typeparam>
        /// <param name="item"><para>注册的Disposeable对象</para><para>Disposable instance that would be registered</para></param>
        /// <param name="viewModel">注册到的View Model</param>
        /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">The comment.</param>
        /// <param name="caller">The caller.</param>
        /// <param name="file">The file.</param>
        /// <param name="line">The line.</param>
        /// <returns>T.</returns>
        public static T DisposeWhenUnload<T>(this T item, IViewModelLifetime viewModel, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
        {

            viewModel.UnloadDisposeGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
            return item;


        }
        /// <summary>
        /// <para>注册在目标VM绑定的视图与VM解除绑定的的时候Dispose </para>
        /// <para>Register a dispose object that would dispose when target viewmodel's view unbind with the viewmodel</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">The comment.</param>
        /// <param name="caller">The caller.</param>
        /// <param name="file">The file.</param>
        /// <param name="line">The line.</param>
        /// <returns>T.</returns>
        public static T DisposeWhenUnbind<T>(this T item, IViewModelLifetime viewModel, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
        {

            viewModel.UnbindDisposeGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
            return item;


        }

    }
#endif
}
