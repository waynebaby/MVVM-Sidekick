/// <summary>
/// <para>MVVM-Sidekick Visual Studio扩展包文件</para>
/// <para>MVVM-Sidekick Visual Studio extension package file</para>
/// </summary>
/// <remarks>
/// <para>此文件包含Visual Studio扩展包的实现，为MVVM-Sidekick框架提供IDE集成支持</para>
/// <para>This file contains the Visual Studio extension package implementation that provides IDE integration support for the MVVM-Sidekick framework</para>
/// <para>实现IVsPackage接口并通过托管包框架(MPF)注册到Visual Studio shell</para>
/// <para>Implements the IVsPackage interface and registers with Visual Studio shell through Managed Package Framework (MPF)</para>
/// </remarks>

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace MVVM_Sidekick_Extensions
{
    /// <summary>
    /// <para>MVVM-Sidekick扩展包类，实现Visual Studio扩展包功能</para>
    /// <para>MVVM-Sidekick extension package class that implements Visual Studio extension package functionality</para>
    /// </summary>
    /// <remarks>
    /// <para>这是实现由此程序集公开的包的类</para>
    /// <para>This is the class that implements the package exposed by this assembly</para>
    /// <para>Visual Studio包的最低要求是实现IVsPackage接口并向shell注册自身</para>
    /// <para>The minimum requirement for a class to be considered a valid package for Visual Studio is to implement the IVsPackage interface and register itself with the shell</para>
    /// <para>此包使用托管包框架(MPF)中定义的帮助器类来实现：从提供IVsPackage接口实现的Package类派生，并使用框架中定义的注册特性向shell注册自身及其组件</para>
    /// <para>This package uses the helper classes defined inside the Managed Package Framework (MPF) to do it: it derives from the Package class that provides the implementation of the IVsPackage interface and uses the registration attributes defined in the framework to register itself and its components with the shell</para>
    /// <para>这些特性告诉pkgdef创建工具将什么数据放入.pkgdef文件</para>
    /// <para>These attributes tell the pkgdef creation utility what data to put into .pkgdef file</para>
    /// <para>要加载到VS中，包必须在.vsixmanifest文件中通过&lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt;引用</para>
    /// <para>To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file</para>
    /// </remarks>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(MVVM_Sidekick_ExtensionsPackage.PackageGuidString)]
    public sealed class MVVM_Sidekick_ExtensionsPackage : AsyncPackage
    {
        /// <summary>
        /// <para>MVVM_Sidekick_ExtensionsPackage的GUID字符串</para>
        /// <para>MVVM_Sidekick_ExtensionsPackage GUID string</para>
        /// </summary>
        public const string PackageGuidString = "750dc2dd-1c99-4878-9b9a-2284cbe4bbe3";

        #region Package Members

        /// <summary>
        /// <para>包的初始化方法；此方法在包被放置后立即调用，因此这是您可以放置所有依赖于VisualStudio提供的服务的初始化代码的地方</para>
        /// <para>Initialization of the package; this method is called right after the package is sited, so this is the place where you can put all the initialization code that rely on services provided by VisualStudio</para>
        /// </summary>
        /// <param name="cancellationToken">
        /// <para>用于监视初始化取消的取消令牌，当VS关闭时可能发生</para>
        /// <para>A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down</para>
        /// </param>
        /// <param name="progress">
        /// <para>进度更新的提供程序</para>
        /// <para>A provider for progress updates</para>
        /// </param>
        /// <returns>
        /// <para>表示包初始化异步工作的任务，如果没有则返回已完成的任务。不要从此方法返回null</para>
        /// <para>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method</para>
        /// </returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            
        }

        #endregion
    }
}
