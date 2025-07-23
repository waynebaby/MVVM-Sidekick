using MVVMSidekick.Commands;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>ViewModel wrapper for ICommand. Usually contains a Command instance and a set of states for this instance.</para>
    /// <para>用于封装ICommand的ViewModel。一般包括一个Command实例和对应此实例的一组状态。</para>
    /// </summary>
    /// <typeparam name="TCommand">ICommand detailed type / ICommand 详细类型</typeparam>
    /// <typeparam name="TState">State type for the Command / 配合Command的状态类型</typeparam>
    public class CommandModel<TCommand, TState> : BindableBase<CommandModel<TCommand, TState>>, ICommandModel<TCommand, TState>, ICommandWithViewModel
        where TCommand : ICommand
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return State.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandModel{TCommand, TResource}"/> class.
        /// </summary>
        public CommandModel()
        { }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="commandCore">ICommand核心</param>
        /// <param name="resource">初始资源</param>
        public CommandModel(TCommand commandCore, TState state)
        {
            CommandCore = commandCore;
            commandCore.CanExecuteChanged += commandCore_CanExecuteChanged;
            State = state;
        }

        /// <summary>
        /// Handles the CanExecuteChanged event of the commandCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void commandCore_CanExecuteChanged(object sender, EventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, e);
                CanExecute(null);
            }

        }


        /// <summary>
        /// ICommand核心
        /// </summary>
        /// <value>The command core.</value>
        public TCommand CommandCore
        {
            get;
            private set;

        }

        public bool CanExecuteValue
        {
            get => CanExecute(null);
            set => _CanExecuteValueLocator(this).SetValueAndTryNotify(value);
        }
        #region Property bool CanExecuteValue Setup        
        protected Property<bool> _CanExecuteValue = new Property<bool>(_CanExecuteValueLocator);
        static Func<BindableBase, ValueContainer<bool>> _CanExecuteValueLocator = RegisterContainerLocator(nameof(CanExecuteValue), m => m.Initialize(nameof(CanExecuteValue), ref m._CanExecuteValue, ref _CanExecuteValueLocator, () => default(bool)));
        #endregion



        /// <summary>
        /// <para>Gets or sets the state for the command.</para>
        /// <para>获取或设置命令的状态。</para>
        /// </summary>
        /// <value>The state.</value>
        public TState State
        {
            get { return _StateLocator(this).Value; }
            set { _StateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property TState State Setup        
        protected Property<TState> _State = new Property<TState>(_StateLocator);
        static Func<BindableBase, ValueContainer<TState>> _StateLocator = RegisterContainerLocator<TState>(nameof(State), model => model.Initialize(nameof(State), ref model._State, ref _StateLocator, _StateDefaultValueFactory));
        static Func<TState> _StateDefaultValueFactory = () => default(TState);
        #endregion











        /// <summary>
        /// 判断是否可执行
        /// </summary>
        /// <param name="parameter">指定参数</param>
        /// <returns><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            if (IsInDesignMode)
            {
                return true;
            }
            var s = CommandCore.CanExecute(parameter);
            var cv = _CanExecuteValueLocator(this).Value;
            if (s != cv)
            {
                CanExecuteValue = s;
            }

            return s;
        }

        /// <summary>
        /// Occurs when [can execute changed].
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter">指定参数</param>
        public void Execute(object parameter)
        {
            var ec = CommandCore as EventCommandBase;
            var eargs = parameter as EventCommandEventArgs;
            if (ec != null && eargs != null)
            {
                ec.OnCommandExecute(eargs);
            }
            else
                CommandCore.Execute(parameter);
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public BindableBase ViewModel
        {
            get
            {
                var c = CommandCore as ICommandWithViewModel;
                if (c != null)
                {
                    return c.ViewModel;
                }
                return null;
            }
            set
            {
                var c = CommandCore as ICommandWithViewModel;
                if (c != null)
                {
                    c.ViewModel = value;
                }

            }
        }
    }

    /// <summary>
    /// <para>ViewModel wrapper for ICommand. Usually contains a Command instance and a set of states for this instance.</para>
    /// <para>用于封装ICommand的ViewModel。一般包括一个Command实例和对应此实例的一组状态。</para>
    /// </summary>
    /// <typeparam name="TCommand">ICommand detailed type / ICommand 详细类型</typeparam>
    public class CommandModel<TCommand> : CommandModel<TCommand, Object> where TCommand : ICommand
    {   /// <summary>
        /// Initializes a new instance of the <see cref="CommandModel{TCommand, TResource}"/> class.
        /// </summary>
        public CommandModel() : base()
        { }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="commandCore">ICommand核心</param>
        /// <param name="resource">初始资源</param>
        public CommandModel(TCommand commandCore, Object state) : base(commandCore, state)
        { }
    }


    /// <summary>
    /// <para>ViewModel wrapper for ReactiveCommand. Usually contains a Command instance and a set of states for this instance.</para>
    /// <para>用于封装ReactiveCommand的ViewModel。一般包括一个Command实例和对应此实例的一组状态。</para>
    /// </summary>
    /// <typeparam name="TCommand">ICommand detailed type / ICommand 详细类型</typeparam>
    public class CommandModel : CommandModel<ReactiveCommand, Object>
    {   /// <summary>
        /// Initializes a new instance of the <see cref="CommandModel{TCommand, TResource}"/> class.
        /// </summary>
        public CommandModel() : base()
        { }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="commandCore">ReactiveCommand</param>
        /// <param name="resource">初始资源</param>
        public CommandModel(ReactiveCommand commandCore, Object state) : base(commandCore, state)
        { }
    }
}
