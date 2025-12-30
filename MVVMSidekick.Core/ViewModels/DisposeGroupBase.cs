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






namespace MVVMSidekick
{

    namespace ViewModels
    {
        using MVVMSidekick.Common;
        /// <summary>
        /// <para>资源释放组基类，提供统一的资源释放管理功能</para>
        /// <para>Base class for dispose group, provides unified resource disposal management</para>
        /// </summary>
        [DataContract]
        public abstract class DisposeGroupBase : InstanceCountableBase, IDisposeGroup
        {
            /// <summary>
            /// 初始化 <see cref="DisposeGroupBase"/> 类的新实例
            /// Initializes a new instance of the <see cref="DisposeGroupBase"/> class.
            /// </summary>
            public DisposeGroupBase()
            {
                CreateDisposeList();

            }

            /// <summary>
            /// <para>创建内部的释放列表</para>
            /// <para>Creates the internal dispose list</para>
            /// </summary>
            private void CreateDisposeList()
            {
                _disposeInfoList = new Lazy<List<DisposeEntry>>(() => new List<DisposeEntry>(), true);

            }

            /// <summary>
            /// <para>在反序列化时调用，用于重新初始化对象状态</para>
            /// <para>Called during deserialization to reinitialize object state</para>
            /// </summary>
            /// <param name="context">
            /// <para>序列化上下文</para>
            /// <para>The serialization context</para>
            /// </param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2238:ImplementSerializationMethodsCorrectly"), OnDeserializing]
            public void OnDeserializing(System.Runtime.Serialization.StreamingContext context)
            {
                OnDeserializingActions();
            }

            /// <summary>
            /// <para>执行反序列化时的动作</para>
            /// <para>Executes actions during deserialization</para>
            /// </summary>
            protected virtual void OnDeserializingActions()
            {

                CreateDisposeList();
            }


            #region Disposing Logic/Disposing相关逻辑
            /// <summary>
            /// <para>DisposeGroupBase 类的终结器</para>
            /// <para>Finalizer for the DisposeGroupBase class</para>
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
            /// <para>获取释放信息列表</para>
            /// <para>Gets the dispose information list</para>
            /// </summary>
            /// <value>
            /// <para>释放信息列表</para>
            /// <para>The dispose information list</para>
            /// </value>
            public IList<DisposeEntry> DisposeInfoList { get { return _disposeInfoList.Value; } }

            //protected static Func<DisposeGroupBase, List<DisposeEntry>> _locateDisposeInfos =
            //    m =>
            //    {
            //        if (m._disposeInfoList == null)
            //        {
            //            Interlocked.CompareExchange(ref m._disposeInfoList, new List<DisposeEntry>(), null);

            //        }
            //        return m._disposeInfoList;

            //    };

            /// <summary>
            /// <para>注册一个在实例销毁时需要执行的逻辑操作</para>
            /// <para>Registers a logic action that needs to be executed when the instance is disposing</para>
            /// </summary>
            /// <param name="newAction">
            /// <para>销毁操作</para>
            /// <para>Disposing action</para>
            /// </param>
            /// <param name="needCheckInFinalizer">
            /// <para>是否需要在终结器中检查</para>
            /// <para>Whether to check in finalizer</para>
            /// </param>
            /// <param name="comment">
            /// <para>注释</para>
            /// <para>The comment</para>
            /// </param>
            /// <param name="caller">
            /// <para>调用者名称</para>
            /// <para>The caller name</para>
            /// </param>
            /// <param name="file">
            /// <para>文件路径</para>
            /// <para>The file path</para>
            /// </param>
            /// <param name="line">
            /// <para>行号</para>
            /// <para>The line number</para>
            /// </param>
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
            /// <para>注册一个需要在实例销毁时一起释放的对象</para>
            /// <para>Registers an object that needs to be disposed when the instance is disposing</para>
            /// </summary>
            /// <param name="item">
            /// <para>需要一起销毁的可释放对象</para>
            /// <para>Disposable object that needs to be disposed together</para>
            /// </param>
            /// <param name="needCheckInFinalizer">
            /// <para>是否需要在终结器中检查</para>
            /// <para>Whether to check in finalizer</para>
            /// </param>
            /// <param name="comment">
            /// <para>注释</para>
            /// <para>The comment</para>
            /// </param>
            /// <param name="caller">
            /// <para>调用者名称</para>
            /// <para>The caller name</para>
            /// </param>
            /// <param name="file">
            /// <para>文件路径</para>
            /// <para>The file path</para>
            /// </param>
            /// <param name="line">
            /// <para>行号</para>
            /// <para>The line number</para>
            /// </param>
            public void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
            {
                AddDisposeAction(() => item.Dispose(), needCheckInFinalizer, comment, caller, file, line);
            }




            /// <summary>
            /// <para>释放此实例及其管理的所有资源</para>
            /// <para>Disposes this instance and all resources it manages</para>
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// <para>执行所有的释放操作，尝试运行所有注册的销毁操作</para>
            /// <para>Performs all dispose operations, attempts to run all registered disposal actions</para>
            /// </summary>
            /// <param name="disposing">
            /// <para>为 <c>true</c> 时释放托管和非托管资源；为 <c>false</c> 时仅释放非托管资源</para>
            /// <para><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</para>
            /// </param>
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
                                    if (DisposeEntryDisposing != null)
                                    {
                                        DisposeEntryDisposing(this, ea);
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
                                    if (DisposeEntryDisposed != null)
                                    {
                                        DisposeEntryDisposed(this, ea);
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
            /// <para>处理释放操作时产生的异常</para>
            /// <para>Handles exceptions that occur during dispose operations</para>
            /// </summary>
            /// <param name="disposeInfoWithExceptions">
            /// <para>包含异常信息的释放条目集合</para>
            /// <para>Collection of dispose entries with exception information</para>
            /// </param>
            protected virtual void OnDisposeExceptions(IList<DisposeEntry> disposeInfoWithExceptions)
            {

            }


            #endregion


            /// <summary>
            /// <para>在释放条目开始释放时发生</para>
            /// <para>Occurs when a dispose entry starts disposing</para>
            /// </summary>
            public event EventHandler<DisposeEventArgs> DisposeEntryDisposing;

            /// <summary>
            /// <para>在释放条目完成释放时发生</para>
            /// <para>Occurs when a dispose entry finishes disposing</para>
            /// </summary>
            public event EventHandler<DisposeEventArgs> DisposeEntryDisposed;
        }





    }

}
