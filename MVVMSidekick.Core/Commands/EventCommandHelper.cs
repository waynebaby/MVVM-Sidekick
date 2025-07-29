


using MVVMSidekick.ViewModels;

namespace MVVMSidekick
{

    namespace Commands
    {
        
        /// <summary>
        /// 事件Command的助手类
        /// Helper class for Event Commands
        /// </summary>
        public static class EventCommandHelper
        {
            /// <summary>
            /// 为一个事件Command制定一个VM
            /// Associates a ViewModel with an Event Command
            /// </summary>
            /// <typeparam name="TCommand">事件Command具体类型 / Concrete type of the Event Command</typeparam>
            /// <param name="cmd">事件Command实例 / Event Command instance</param>
            /// <param name="viewModel">VM实例 / ViewModel instance</param>
            /// <returns>事件Command实例本身 / The Event Command instance itself</returns>
            public static TCommand WithViewModel<TCommand>(this TCommand cmd, BindableBase viewModel)
                where TCommand : EventCommandBase
            {
                cmd.ViewModel = viewModel;
                return cmd;
            }

        }


    }

}
