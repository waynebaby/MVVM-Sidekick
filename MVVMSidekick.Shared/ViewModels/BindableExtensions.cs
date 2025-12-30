using System;
using System.Runtime.CompilerServices;

namespace MVVMSidekick.ViewModels
{
#if !WEBASM
    /// <summary>
    /// <para>可绑定对象扩展方法静态类，为可释放对象提供视图模型生命周期管理的扩展方法</para>
    /// <para>Static class providing extension methods for bindable objects, offering viewmodel lifecycle management extension methods for disposable objects</para>
    /// </summary>
    /// <remarks>
    /// <para>此类提供将IDisposable对象注册到视图模型生命周期的扩展方法，包括在视图卸载时和视图与视图模型解绑时自动释放资源</para>
    /// <para>This class provides extension methods to register IDisposable objects to viewmodel lifecycle, including automatic resource disposal when view unloads and when view unbinds from viewmodel</para>
    /// </remarks>
    public static class BindableExtensions
    {



        /// <summary>
        /// <para>注册在目标VM绑定的视图Unload的时候Dispose对象，提供资源生命周期管理</para>
        /// <para>Register a dispose object that would dispose when target viewmodel's view unload, providing resource lifecycle management</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>任意IDisposable对象类型</para>
        /// <para>Any IDisposable object type</para>
        /// </typeparam>
        /// <param name="item">
        /// <para>注册的Disposeable对象实例</para>
        /// <para>Disposable object instance to be registered</para>
        /// </param>
        /// <param name="viewModel">
        /// <para>注册到的View Model实例</para>
        /// <para>The View Model instance to register to</para>
        /// </param>
        /// <param name="needCheckInFinalizer">
        /// <para>是否需要在终结器中检查资源释放状态</para>
        /// <para>Whether need to check resource disposal state in finalizer</para>
        /// </param>
        /// <param name="comment">
        /// <para>注释信息，用于调试和日志记录</para>
        /// <para>Comment information for debugging and logging</para>
        /// </param>
        /// <param name="caller">
        /// <para>调用者成员名称，由编译器自动填充</para>
        /// <para>Caller member name, automatically filled by compiler</para>
        /// </param>
        /// <param name="file">
        /// <para>调用文件路径，由编译器自动填充</para>
        /// <para>Caller file path, automatically filled by compiler</para>
        /// </param>
        /// <param name="line">
        /// <para>调用行号，由编译器自动填充</para>
        /// <para>Caller line number, automatically filled by compiler</para>
        /// </param>
        /// <returns>
        /// <para>返回原始对象实例，支持链式调用</para>
        /// <para>Returns the original object instance, supports method chaining</para>
        /// </returns>
        public static T DisposeWhenUnload<T>(this T item, IViewModelLifetime viewModel, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
        {

            viewModel.UnloadDisposeGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
            return item;


        }
        /// <summary>
        /// <para>注册在目标VM绑定的视图与VM解除绑定的时候Dispose对象，提供解绑时资源清理</para>
        /// <para>Register a dispose object that would dispose when target viewmodel's view unbind with the viewmodel, providing resource cleanup on unbinding</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>任意IDisposable对象类型</para>
        /// <para>Any IDisposable object type</para>
        /// </typeparam>
        /// <param name="item">
        /// <para>注册的Disposeable对象实例</para>
        /// <para>Disposable object instance to be registered</para>
        /// </param>
        /// <param name="viewModel">
        /// <para>注册到的View Model实例</para>
        /// <para>The View Model instance to register to</para>
        /// </param>
        /// <param name="needCheckInFinalizer">
        /// <para>是否需要在终结器中检查资源释放状态</para>
        /// <para>Whether need to check resource disposal state in finalizer</para>
        /// </param>
        /// <param name="comment">
        /// <para>注释信息，用于调试和日志记录</para>
        /// <para>Comment information for debugging and logging</para>
        /// </param>
        /// <param name="caller">
        /// <para>调用者成员名称，由编译器自动填充</para>
        /// <para>Caller member name, automatically filled by compiler</para>
        /// </param>
        /// <param name="file">
        /// <para>调用文件路径，由编译器自动填充</para>
        /// <para>Caller file path, automatically filled by compiler</para>
        /// </param>
        /// <param name="line">
        /// <para>调用行号，由编译器自动填充</para>
        /// <para>Caller line number, automatically filled by compiler</para>
        /// </param>
        /// <returns>
        /// <para>返回原始对象实例，支持链式调用</para>
        /// <para>Returns the original object instance, supports method chaining</para>
        /// </returns>
        public static T DisposeWhenUnbind<T>(this T item, IViewModelLifetime viewModel, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
        {

            viewModel.UnbindDisposeGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
            return item;


        }

    }
#endif
}
