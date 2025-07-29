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
    /// <para>可绑定CommandModel的扩展方法集合</para>
    /// <para>Extension methods for bindable Command ViewModels</para>
    /// </summary>
    public static class CommandModelExtensions
    {
        /// <summary>
        /// <para>异步执行Reactive命令</para>
        /// <para>Executes the reactive command asynchronously</para>
        /// </summary>
        /// <typeparam name="TReactiveCommand">
        /// <para>Reactive命令类型</para>
        /// <para>The type of the reactive command</para>
        /// </typeparam>
        /// <typeparam name="TResource">
        /// <para>资源类型</para>
        /// <para>The type of the resource</para>
        /// </typeparam>
        /// <param name="command">
        /// <para>命令模型实例</para>
        /// <para>The command model instance</para>
        /// </param>
        /// <param name="parameter">
        /// <para>命令参数</para>
        /// <para>The command parameter</para>
        /// </param>
        /// <returns>
        /// <para>异步任务</para>
        /// <para>Asynchronous task</para>
        /// </returns>
        public static async Task ExecuteAsync<TReactiveCommand, TResource>(this CommandModel<TReactiveCommand, TResource> command, object parameter)
            where TReactiveCommand : IReactiveCommand
        {
            await command.CommandCore.ExecuteAsync(parameter);
        }

        /// <summary>
        /// <para>根据ICommand实例创建CommandModel</para>
        /// <para>Creates a CommandModel from an ICommand instance</para>
        /// </summary>
        /// <typeparam name="TCommand">
        /// <para>ICommand实例的具体类型</para>
        /// <para>The specific type of the ICommand instance</para>
        /// </typeparam>
        /// <typeparam name="TResource">
        /// <para>附加资源类型</para>
        /// <para>The type of additional resource</para>
        /// </typeparam>
        /// <param name="command">
        /// <para>ICommand实例</para>
        /// <para>The ICommand instance</para>
        /// </param>
        /// <param name="resource">
        /// <para>资源实例</para>
        /// <para>The resource instance</para>
        /// </param>
        /// <returns>
        /// <para>CommandModel实例</para>
        /// <para>The CommandModel instance</para>
        /// </returns>
        public static CommandModel<TCommand, TResource> CreateCommandModel<TCommand, TResource>(this TCommand command, TResource resource)
            where TCommand : ICommand
        {
            return new CommandModel<TCommand, TResource>(command, resource);
        }

        /// <summary>
        /// <para>根据ReactiveCommand实例创建CommandModel</para>
        /// <para>Creates a CommandModel from a ReactiveCommand instance</para>
        /// </summary>
        /// <param name="command">
        /// <para>ReactiveCommand实例</para>
        /// <para>The ReactiveCommand instance</para>
        /// </param>
        /// <param name="resource">
        /// <para>资源实例</para>
        /// <para>The resource instance</para>
        /// </param>
        /// <returns>
        /// <para>CommandModel实例</para>
        /// <para>The CommandModel instance</para>
        /// </returns>
        public static CommandModel CreateCommandModel (this ReactiveCommand command, object resource)
          
        {
            return new CommandModel(command, resource);
        }

        /// <summary>
        /// <para>根据ICommand实例创建不具备强类型资源的CommandModel</para>
        /// <para>Creates a CommandModel without strongly typed resource from an ICommand instance</para>
        /// </summary>
        /// <typeparam name="TCommand">
        /// <para>ICommand实例的具体类型</para>
        /// <para>The specific type of the ICommand instance</para>
        /// </typeparam>
        /// <param name="command">
        /// <para>ICommand实例</para>
        /// <para>The ICommand instance</para>
        /// </param>
        /// <param name="resource">
        /// <para>资源实例（可选）</para>
        /// <para>The resource instance (optional)</para>
        /// </param>
        /// <returns>
        /// <para>CommandModel实例</para>
        /// <para>The CommandModel instance</para>
        /// </returns>
        public static CommandModel<TCommand, object> CreateCommandModel<TCommand>(this TCommand command, object resource = null)
        where TCommand : ICommand
        {
            return new CommandModel<TCommand, object>(command, null);
        }

        /// <summary>
        /// <para>为CommandModel指定ViewModel</para>
        /// <para>Specifies a ViewModel for the CommandModel</para>
        /// </summary>
        /// <typeparam name="TCommand">
        /// <para>ICommand实例的具体类型</para>
        /// <para>The specific type of the ICommand instance</para>
        /// </typeparam>
        /// <typeparam name="TResource">
        /// <para>附加资源类型</para>
        /// <para>The type of additional resource</para>
        /// </typeparam>
        /// <param name="cmdModel">
        /// <para>CommandModel具体实例</para>
        /// <para>The specific CommandModel instance</para>
        /// </param>
        /// <param name="viewModel">
        /// <para>ViewModel具体实例</para>
        /// <para>The specific ViewModel instance</para>
        /// </param>
        /// <returns>
        /// <para>配置了ViewModel的CommandModel实例</para>
        /// <para>The CommandModel instance configured with ViewModel</para>
        /// </returns>
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
