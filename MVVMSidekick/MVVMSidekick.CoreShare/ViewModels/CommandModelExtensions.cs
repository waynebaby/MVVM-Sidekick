using MVVMSidekick.Commands;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// 可绑定的CommandVM 扩展方法集
    /// </summary>
    public static class CommandModelExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TReactiveCommand"></typeparam>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static async Task ExecuteAsync<TReactiveCommand, TResource>(this CommandModel<TReactiveCommand, TResource> command, object parameter)
            where TReactiveCommand : IReactiveCommand
        {
            await command.CommandCore.ExecuteAsync(parameter);
        }

        /// <summary>
        /// 根据ICommand实例创建CommandModel
        /// </summary>
        /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
        /// <typeparam name="TResource">附加资源类型</typeparam>
        /// <param name="command">ICommand实例</param>
        /// <param name="resource">资源实例</param>
        /// <returns>CommandModel实例</returns>
        public static CommandModel<TCommand, TResource> CreateCommandModel<TCommand, TResource>(this TCommand command, TResource resource)
            where TCommand : ICommand
        {
            return new CommandModel<TCommand, TResource>(command, resource);
        }

        /// <summary>
        /// 根据ICommand实例创建CommandModel
        /// </summary>
        /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
        /// <typeparam name="TResource">附加资源类型</typeparam>
        /// <param name="command">ICommand实例</param>
        /// <param name="resource">资源实例</param>
        /// <returns>CommandModel实例</returns>
        public static CommandModel CreateCommandModel (this ReactiveCommand command, object resource)
          
        {
            return new CommandModel(command, resource);
        }


        /// <summary>
        /// 据ICommand实例创建不具备/弱类型资源的CommandModel
        /// </summary>
        /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
        /// <param name="command">ICommand实例</param>
        /// <param name="resource">资源实例</param>
        /// <returns>CommandModel实例</returns>
        public static CommandModel<TCommand, object> CreateCommandModel<TCommand>(this TCommand command, object resource = null)
        where TCommand : ICommand
        {
            return new CommandModel<TCommand, object>(command, null);
        }

        /// <summary>
        /// 为CommandModel指定ViewModel
        /// </summary>
        /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
        /// <typeparam name="TResource">附加资源类型</typeparam>
        /// <param name="cmdModel">CommandModel具体实例</param>
        /// <param name="viewModel">ViewModel具体实例</param>
        /// <returns>CommandModel&lt;TCommand, TResource&gt;.</returns>
        public static CommandModel<TCommand, TResource> WithViewModel<TCommand, TResource>(this CommandModel<TCommand, TResource> cmdModel, BindableBase viewModel)
            where TCommand : ICommand
        {
            //cmdModel.
            var cmd2 = cmdModel.CommandCore as ICommandWithViewModel;
            if (cmd2 != null)
            {
                cmd2.ViewModel = viewModel;
            }
            return cmdModel;
        }
    }

}
