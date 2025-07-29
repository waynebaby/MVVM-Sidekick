
using System;
using System.Threading.Tasks;
using System.Threading;



namespace MVVMSidekick
{

    namespace Commands
    {
        /// <summary>
        /// Command被运行触发的事件数据类型
        /// Event data type triggered when a Command is executed
        /// </summary>
        public class EventCommandEventArgs : EventArgs
        {
            /// <summary>
            /// 初始化EventCommandEventArgs实例
            /// Initializes a new instance of EventCommandEventArgs
            /// </summary>
            public EventCommandEventArgs()
            {


            }
            
            /// <summary>
            /// 获取事件的唯一标识符
            /// Gets the unique identifier of the event
            /// </summary>
            public Guid EventUniqueId { get; } = Guid.NewGuid();

            /// <summary>
            /// 获取或设置命令参数
            /// Gets or sets the command parameter
            /// </summary>
            public Object Parameter { get; set; }
            /// <summary>
            /// 获取或设置视图模型
            /// Gets or sets the view model.
            /// </summary>
            /// <value>The view model.</value>
            public Object ViewModel { get; set; }
            
            /// <summary>
            /// 获取或设置视图发送者
            /// Gets or sets the view sender.
            /// </summary>
            /// <value>The view sender.</value>
            public Object ViewSender { get; set; }
            
            /// <summary>
            /// 获取或设置事件参数
            /// Gets or sets the event arguments.
            /// </summary>
            /// <value>The event arguments.</value>
            public Object EventArgs { get; set; }
            
            /// <summary>
            /// 获取或设置事件名称
            /// Gets or sets the name of the event.
            /// </summary>
            /// <value>The name of the event.</value>
            public string EventName { get; set; }
            
            /// <summary>
            /// 获取或设置事件处理程序的类型
            /// Gets or sets the type of the event handler.
            /// </summary>
            /// <value>The type of the event handler.</value>
            public Type EventHandlerType { get; set; }

            /// <summary>
            /// 获取任务完成源，用于异步操作完成通知
            /// Gets the task completion source for async operation completion notification
            /// </summary>
            public TaskCompletionSource<EventCommandEventArgs> Completion { get; } =new TaskCompletionSource<EventCommandEventArgs>();
            
            /// <summary>
            /// 获取取消令牌源，用于操作取消控制
            /// Gets the cancellation token source for operation cancellation control
            /// </summary>
            public CancellationTokenSource Cancellation { get; } = new CancellationTokenSource();



            /// <summary>
            /// 创建EventCommandEventArgs实例的静态工厂方法
            /// Static factory method to create EventCommandEventArgs instance
            /// </summary>
            /// <param name="parameter">命令参数 / The parameter</param>
            /// <param name="viewModel">视图模型 / The view model</param>
            /// <param name="viewSender">视图发送者 / The view sender</param>
            /// <param name="eventArgs">事件参数 / The event arguments</param>
            /// <param name="eventName">事件名称 / Name of the event</param>
            /// <param name="eventHandlerType">事件处理程序类型 / Type of the event handler</param>
            /// <returns>EventCommandEventArgs实例 / EventCommandEventArgs instance</returns>
            public static EventCommandEventArgs Create(
                Object parameter = null,
                Object viewModel = null,
                object viewSender = null,
                object eventArgs = null,
                string eventName = null,
                Type eventHandlerType = null
                )
            {
                return new EventCommandEventArgs { Parameter = parameter, ViewModel = viewModel, ViewSender = viewSender, EventArgs = eventArgs, EventHandlerType = eventHandlerType, EventName = eventName };
            }


        }


    }

}
