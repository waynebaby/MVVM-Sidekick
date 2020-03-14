
using System;
using System.Threading.Tasks;
using System.Threading;



namespace MVVMSidekick
{

    namespace Commands
    {
        /// <summary>
        /// Command被运行触发的事件数据类型
        /// </summary>
        public class EventCommandEventArgs : EventArgs
        {
            public EventCommandEventArgs()
            {


            }
            public Guid EventUniqueId { get; } = Guid.NewGuid();



            public Object Parameter { get; set; }
            /// <summary>
            /// Gets or sets the view model.
            /// </summary>
            /// <value>The view model.</value>
            public Object ViewModel { get; set; }
            /// <summary>
            /// Gets or sets the view sender.
            /// </summary>
            /// <value>The view sender.</value>
            public Object ViewSender { get; set; }
            /// <summary>
            /// Gets or sets the event arguments.
            /// </summary>
            /// <value>The event arguments.</value>
            public Object EventArgs { get; set; }
            /// <summary>
            /// Gets or sets the name of the event.
            /// </summary>
            /// <value>The name of the event.</value>
            public string EventName { get; set; }
            /// <summary>
            /// Gets or sets the type of the event handler.
            /// </summary>
            /// <value>The type of the event handler.</value>
            public Type EventHandlerType { get; set; }



            public TaskCompletionSource<EventCommandEventArgs> Completion { get; } =new TaskCompletionSource<EventCommandEventArgs>();
            public CancellationTokenSource Cancellation { get; } = new CancellationTokenSource();



            /// <summary>
            /// Creates the specified parameter.
            /// </summary>
            /// <param name="parameter">The parameter.</param>
            /// <param name="viewModel">The view model.</param>
            /// <param name="viewSender">The view sender.</param>
            /// <param name="eventArgs">The event arguments.</param>
            /// <param name="eventName">Name of the event.</param>
            /// <param name="eventHandlerType">Type of the event handler.</param>
            /// <returns>EventCommandEventArgs.</returns>
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
