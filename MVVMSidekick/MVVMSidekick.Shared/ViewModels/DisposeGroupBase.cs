// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Reactive.Linq;
#if NETFX_CORE


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
using System.Windows.Threading;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#endif






namespace MVVMSidekick
{

    namespace ViewModels
    {
        using MVVMSidekick.Common;
        /// <summary>
        /// Class DisposeGroupBase.
        /// </summary>
        [DataContract]
        public abstract class DisposeGroupBase : InstanceCounableBase, IDisposeGroup
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DisposeGroupBase"/> class.
            /// </summary>
            public DisposeGroupBase()
            {
                CreateDisposeList();

            }

            /// <summary>
            /// Creates the dispose list.
            /// </summary>
            private void CreateDisposeList()
            {
                _disposeInfoList = new Lazy<List<DisposeEntry>>(() => new List<DisposeEntry>(), true);

            }

            /// <summary>
            /// Called when [deserializing].
            /// </summary>
            /// <param name="context">The context.</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2238:ImplementSerializationMethodsCorrectly"), OnDeserializing]
            public void OnDeserializing(System.Runtime.Serialization.StreamingContext context)
            {
                OnDeserializingActions();
            }

            /// <summary>
            /// Called when [deserializing actions].
            /// </summary>
            protected virtual void OnDeserializingActions()
            {

                CreateDisposeList();
            }


            #region Disposing Logic/Disposing相关逻辑
            /// <summary>
            /// Finalizes an instance of the <see cref="DisposeGroupBase"/> class.
            /// </summary>
            ~DisposeGroupBase()
            {
                Dispose(false);
            }



            /// <summary>
            /// <para>Logic actions need to be executed when the instance is disposing</para>
            /// <para>销毁对象时 需要执行的操作</para>
            /// </summary>
            private Lazy<List<DisposeEntry>> _disposeInfoList;

            /// <summary>
            /// Gets the dispose information list.
            /// </summary>
            /// <value>The dispose information list.</value>
            public IList<DisposeEntry> DisposeInfoList { get { return _disposeInfoList.Value; } }

            //protected static Func<DisposeGroupBase, List<DisposeInfo>> _locateDisposeInfos =
            //    m =>
            //    {
            //        if (m._disposeInfoList == null)
            //        {
            //            Interlocked.CompareExchange(ref m._disposeInfoList, new List<DisposeInfo>(), null);

            //        }
            //        return m._disposeInfoList;

            //    };

            /// <summary>
            /// <para>Register logic actions need to be executed when the instance is disposing</para>
            /// <para>注册一个销毁对象时需要执行的操作</para>
            /// </summary>
            /// <param name="newAction">Disposing action/销毁操作</param>
            /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
            /// <param name="comment">The comment.</param>
            /// <param name="caller">The caller.</param>
            /// <param name="file">The file.</param>
            /// <param name="line">The line.</param>
            public void AddDisposeAction(Action newAction, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
            {

                var di = new DisposeEntry
                {
                    CallingCodeContext = CallingCodeContext.Create(comment, caller, file, line),
                    Action = newAction,
                    IsNeedCheckOnFinalizer = needCheckInFinalizer

                };
                _disposeInfoList.Value.Add(di);

            }


            /// <summary>
            /// <para>Register an object that need to be disposed when the instance is disposing</para>
            /// <para>销毁对象时 需要一起销毁的对象</para>
            /// </summary>
            /// <param name="item">disposable object/需要一起销毁的对象</param>
            /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
            /// <param name="comment">The comment.</param>
            /// <param name="caller">The caller.</param>
            /// <param name="file">The file.</param>
            /// <param name="line">The line.</param>
            public void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
            {
                AddDisposeAction(() => item.Dispose(), needCheckInFinalizer, comment, caller, file, line);
            }




            /// <summary>
            /// Disposes this instance.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// <para>Do all the dispose </para>
            /// <para>销毁，尝试运行所有注册的销毁操作</para>
            /// </summary>
            /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
            protected virtual void Dispose(bool disposing)
            {
                var disposeList = Interlocked.Exchange(ref _disposeInfoList, new Lazy<List<DisposeEntry>>(() => new List<DisposeEntry>(), true));
                if (disposeList != null && disposeList.IsValueCreated)
                {
                    var l = disposeList.Value
                        .Select
                        (
                            info =>
                            {
                                var ea = DisposeEventArgs.Create(info);
                                //Exception gotex = null;
                                try
                                {
                                    if (DisposingEntry != null)
                                    {
                                        DisposingEntry(this, ea);
                                    }
                                    if (disposing || info.IsNeedCheckOnFinalizer)
                                    {
                                        info.Action();
                                    }


                                }
                                catch (Exception ex)
                                {
                                    info.Exception = ex;

                                }
                                finally
                                {
                                    if (DisposedEntry != null)
                                    {
                                        DisposedEntry(this, ea);
                                    }
                                }

                                return info;
                            }

                        )
                        .Where(x => x.Exception != null)
                        .ToArray();
                    if (l.Length > 0)
                    {
                        OnDisposeExceptions(l);
                    }
                }



            }




            /// <summary>
            /// <para>If dispose actions got exceptions, will handled here. </para>
            /// <para>处理Dispose 时产生的Exception</para>
            /// </summary>
            /// <param name="disposeInfoWithExceptions"><para>The exception and dispose infomation</para>
            /// <para>需要处理的异常信息</para></param>

            protected virtual void OnDisposeExceptions(IList<DisposeEntry> disposeInfoWithExceptions)
            {

            }


            #endregion


            /// <summary>
            /// Occurs when [disposing entry].
            /// </summary>
            public event EventHandler<DisposeEventArgs> DisposingEntry;

            /// <summary>
            /// Occurs when [disposed entry].
            /// </summary>
            public event EventHandler<DisposeEventArgs> DisposedEntry;
        }





    }

}
