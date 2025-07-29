using MVVMSidekick.Commands;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>ICommand 的 ViewModel 包装器。通常包含一个 Command 实例和该实例的一组状态</para>
    /// <para>ViewModel wrapper for ICommand. Usually contains a Command instance and a set of states for this instance.</para>
    /// <para>用于封装ICommand的ViewModel。一般包括一个Command实例和对应此实例的一组状态。</para>
    /// </summary>
    /// <typeparam name="TCommand">ICommand 详细类型 / ICommand detailed type</typeparam>
    /// <typeparam name="TState">配合 Command 的状态类型 / State type for the Command</typeparam>
    public class CommandModel<TCommand, TState> : BindableBase<CommandModel<TCommand, TState>>, ICommandModel<TCommand, TState>, ICommandWithViewModel
        where TCommand : ICommand
    {
        /// <summary>
        /// 返回表示此实例的 <see cref="System.String" />
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>表示此实例的 <see cref="System.String" /> / A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return State.ToString();
        }

        /// <summary>
        /// 初始化 <see cref="CommandModel{TCommand, TResource}"/> 类的新实例
        /// Initializes a new instance of the <see cref="CommandModel{TCommand, TResource}"/> class.
        /// </summary>
        public CommandModel()
        { }
        /// <summary>
        /// 构造器
        /// Constructor
        /// </summary>
        /// <param name="commandCore">ICommand核心 / ICommand core</param>
        /// <param name="state">初始状态 / Initial state</param>
        public CommandModel(TCommand commandCore, TState state)
        {
            CommandCore = commandCore;
            commandCore.CanExecuteChanged += commandCore_CanExecuteChanged;
            State = state;
        }

        /// <summary>
        /// 处理 commandCore 控件的 CanExecuteChanged 事件
        /// Handles the CanExecuteChanged event of the commandCore control.
        /// </summary>
        /// <param name="sender">事件源 / The source of the event.</param>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/> 实例 / The <see cref="EventArgs"/> instance containing the event data.</param>
        void commandCore_CanExecuteChanged(object sender, EventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, e);
                CanExecute(null);
            }

        }


        /// <summary>
        /// <para>ICommand核心实例</para>
        /// <para>Core ICommand instance</para>
        /// </summary>
        /// <value>
        /// <para>CommandCore 属性的值</para>
        /// <para>The value of CommandCore property</para>
        /// </value>
        public TCommand CommandCore
        {
            get;
            private set;

        }

        /// <summary>
        /// <para>获取或设置命令是否可执行的值</para>
        /// <para>Gets or sets the value indicating whether the command can execute</para>
        /// </summary>
        /// <value>
        /// <para>如果命令可执行则为 true；否则为 false</para>
        /// <para>true if the command can execute; otherwise, false</para>
        /// </value>
        public bool CanExecuteValue
        {
            get => CanExecute(null);
            set => _CanExecuteValueLocator(this).SetValueAndTryNotify(value);
        }
        #region Property bool CanExecuteValue Setup
        /// <summary>
        /// <para>CanExecuteValue 属性的内部 Property 实例</para>
        /// <para>Internal Property instance for CanExecuteValue property</para>
        /// </summary>        
        protected Property<bool> _CanExecuteValue = new Property<bool>(_CanExecuteValueLocator);
        /// <summary>
        /// <para>CanExecuteValue 属性的值容器定位器</para>
        /// <para>Value container locator for CanExecuteValue property</para>
        /// </summary>
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
        /// <summary>
        /// <para>State 属性的内部 Property 实例</para>
        /// <para>Internal Property instance for State property</para>
        /// </summary>        
        protected Property<TState> _State = new Property<TState>(_StateLocator);
        /// <summary>
        /// <para>State 属性的值容器定位器</para>
        /// <para>Value container locator for State property</para>
        /// </summary>
        static Func<BindableBase, ValueContainer<TState>> _StateLocator = RegisterContainerLocator<TState>(nameof(State), model => model.Initialize(nameof(State), ref model._State, ref _StateLocator, _StateDefaultValueFactory));
        /// <summary>
        /// <para>State 属性的默认值工厂</para>
        /// <para>Default value factory for State property</para>
        /// </summary>
        static Func<TState> _StateDefaultValueFactory = () => default(TState);
        #endregion











        /// <summary>
        /// <para>判断是否可执行命令</para>
        /// <para>Determines whether the command can execute</para>
        /// </summary>
        /// <param name="parameter">
        /// <para>命令参数</para>
        /// <para>Command parameter</para>
        /// </param>
        /// <returns>
        /// <para>如果此实例可以执行指定参数的命令，则为 <c>true</c>；否则为 <c>false</c></para>
        /// <para><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c></para>
        /// </returns>
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
        /// <para>当命令的执行状态发生变化时发生</para>
        /// <para>Occurs when changes occur that affect whether or not the command should execute</para>
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// <para>执行命令</para>
        /// <para>Executes the command</para>
        /// </summary>
        /// <param name="parameter">
        /// <para>命令参数</para>
        /// <para>Command parameter</para>
        /// </param>
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
        /// <para>获取或设置与此命令关联的视图模型</para>
        /// <para>Gets or sets the view model associated with this command</para>
        /// </summary>
        /// <value>
        /// <para>关联的视图模型实例</para>
        /// <para>The associated view model instance</para>
        /// </value>
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
    /// <para>ICommand 的 ViewModel 包装器，使用默认状态类型 Object</para>
    /// <para>ViewModel wrapper for ICommand with default state type Object</para>
    /// </summary>
    /// <typeparam name="TCommand">
    /// <para>ICommand 详细类型</para>
    /// <para>ICommand detailed type</para>
    /// </typeparam>
    public class CommandModel<TCommand> : CommandModel<TCommand, Object> where TCommand : ICommand
    {   
        /// <summary>
        /// <para>初始化 <see cref="CommandModel{TCommand}"/> 类的新实例</para>
        /// <para>Initializes a new instance of the <see cref="CommandModel{TCommand}"/> class</para>
        /// </summary>
        public CommandModel() : base()
        { }
        
        /// <summary>
        /// <para>使用指定的命令核心和状态初始化 <see cref="CommandModel{TCommand}"/> 类的新实例</para>
        /// <para>Initializes a new instance of the <see cref="CommandModel{TCommand}"/> class with specified command core and state</para>
        /// </summary>
        /// <param name="commandCore">
        /// <para>ICommand核心实例</para>
        /// <para>ICommand core instance</para>
        /// </param>
        /// <param name="state">
        /// <para>初始状态对象</para>
        /// <para>Initial state object</para>
        /// </param>
        public CommandModel(TCommand commandCore, Object state) : base(commandCore, state)
        { }
    }


    /// <summary>
    /// <para>ReactiveCommand 的 ViewModel 包装器</para>
    /// <para>ViewModel wrapper for ReactiveCommand</para>
    /// </summary>
    public class CommandModel : CommandModel<ReactiveCommand, Object>
    {   
        /// <summary>
        /// <para>初始化 <see cref="CommandModel"/> 类的新实例</para>
        /// <para>Initializes a new instance of the <see cref="CommandModel"/> class</para>
        /// </summary>
        public CommandModel() : base()
        { }
        
        /// <summary>
        /// <para>使用指定的 ReactiveCommand 和状态初始化 <see cref="CommandModel"/> 类的新实例</para>
        /// <para>Initializes a new instance of the <see cref="CommandModel"/> class with specified ReactiveCommand and state</para>
        /// </summary>
        /// <param name="commandCore">
        /// <para>ReactiveCommand 核心实例</para>
        /// <para>ReactiveCommand core instance</para>
        /// </param>
        /// <param name="state">
        /// <para>初始状态对象</para>
        /// <para>Initial state object</para>
        /// </param>
        public CommandModel(ReactiveCommand commandCore, Object state) : base(commandCore, state)
        { }
    }
}
