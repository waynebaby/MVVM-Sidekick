using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekickBlazorDemo.Pages.ViewModels
{

    /// <summary>
    /// 登录演示页面的视图模型
    /// View model for the LoginDemo page
    /// </summary>
    public class LoginDemo_Model : ViewModel<LoginDemo_Model, LoginDemo>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        /// <summary>
        /// 初始化登录演示视图模型的新实例
        /// Initializes a new instance of the LoginDemo view model
        /// </summary>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        public LoginDemo_Model(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 获取或设置正确的用户名
        /// Gets or sets the correct username
        /// </summary>
        public string RightUserName { get => _RightUserNameLocator(this).Value; set => _RightUserNameLocator(this).SetValueAndTryNotify(value); }
        #region Property string RightUserName Setup        
        protected Property<string> _RightUserName = new Property<string>(_RightUserNameLocator);
        static Func<BindableBase, ValueContainer<string>> _RightUserNameLocator = RegisterContainerLocator(nameof(RightUserName), m => m.Initialize(nameof(RightUserName), ref m._RightUserName, ref _RightUserNameLocator, () => "waynebaby"));
        #endregion

        /// <summary>
        /// 获取或设置正确的密码
        /// Gets or sets the correct password
        /// </summary>
        public string RightPassword { get => _RightPasswordLocator(this).Value; set => _RightPasswordLocator(this).SetValueAndTryNotify(value); }
        #region Property string RightPassword Setup        
        protected Property<string> _RightPassword = new Property<string>(_RightPasswordLocator);
        static Func<BindableBase, ValueContainer<string>> _RightPasswordLocator = RegisterContainerLocator(nameof(RightPassword), m => m.Initialize(nameof(RightPassword), ref m._RightPassword, ref _RightPasswordLocator, () => "loveMVVMSidekick4ever"));
        #endregion

        /// <summary>
        /// 获取或设置当前登录状态
        /// Gets or sets the current login state
        /// </summary>
        public LoginState CurrentLoginState { get => _CurrentLoginStateLocator(this).Value; set => _CurrentLoginStateLocator(this).SetValueAndTryNotify(value); }
        #region Property LoginState CurrentLoginState Setup        
        protected Property<LoginState> _CurrentLoginState = new Property<LoginState>(_CurrentLoginStateLocator);
        static Func<BindableBase, ValueContainer<LoginState>> _CurrentLoginStateLocator = RegisterContainerLocator(nameof(CurrentLoginState), m => m.Initialize(nameof(CurrentLoginState), ref m._CurrentLoginState, ref _CurrentLoginStateLocator, () => default(LoginState)));
        #endregion

        /// <summary>
        /// 获取或设置登录实体
        /// Gets or sets the login entity
        /// </summary>
        public LoginEntity LoginEntity { get => _LoginEntityLocator(this).Value; set => _LoginEntityLocator(this).SetValueAndTryNotify(value); }
        #region Property LoginEntity LoginEntity Setup        
        protected Property<LoginEntity> _LoginEntity = new Property<LoginEntity>(_LoginEntityLocator);
        static Func<BindableBase, ValueContainer<LoginEntity>> _LoginEntityLocator = RegisterContainerLocator(nameof(LoginEntity), m => m.Initialize(nameof(LoginEntity), ref m._LoginEntity, ref _LoginEntityLocator, () => new LoginEntity()));
        #endregion

        /// <summary>
        /// 获取或设置恢复实体
        /// Gets or sets the recovery entity
        /// </summary>
        public RecoveryEntity RecoveryEntity { get => _RecoveryEntityLocator(this).Value; set => _RecoveryEntityLocator(this).SetValueAndTryNotify(value); }
        #region Property RecoveryEntity RecoveryEntity Setup        
        protected Property<RecoveryEntity> _RecoveryEntity = new Property<RecoveryEntity>(_RecoveryEntityLocator);
        static Func<BindableBase, ValueContainer<RecoveryEntity>> _RecoveryEntityLocator = RegisterContainerLocator(nameof(RecoveryEntity), m => m.Initialize(nameof(RecoveryEntity), ref m._RecoveryEntity, ref _RecoveryEntityLocator, () => new RecoveryEntity()));
        #endregion


        public string Message { get => _MessageLocator(this).Value; set => _MessageLocator(this).SetValueAndTryNotify(value); }
        #region Property string Message Setup        
        protected Property<string> _Message = new Property<string>(_MessageLocator);
        static Func<BindableBase, ValueContainer<string>> _MessageLocator = RegisterContainerLocator(nameof(Message), m => m.Initialize(nameof(Message), ref m._Message, ref _MessageLocator, () => default(string)));
        #endregion


        public int CountDownOfSMSResend
        {
            get => _CountDownOfSMSResendLocator(this).Value;
            set => _CountDownOfSMSResendLocator(this).SetValueAndTryNotify(value);
        }
        #region Property int CountDownOfSMSResend Setup        
        protected Property<int> _CountDownOfSMSResend = new Property<int>(_CountDownOfSMSResendLocator);
        static Func<BindableBase, ValueContainer<int>> _CountDownOfSMSResendLocator = RegisterContainerLocator(nameof(CountDownOfSMSResend), m => m.Initialize(nameof(CountDownOfSMSResend), ref m._CountDownOfSMSResend, ref _CountDownOfSMSResendLocator, () => default(int)));
        #endregion


        public CommandModel CommandLoginIn => _CommandLoginInLocator(this).Value;
        #region Property CommandModel CommandLoginIn Setup                
        protected Property<CommandModel> _CommandLoginIn = new Property<CommandModel>(_CommandLoginInLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandLoginInLocator = RegisterContainerLocator(nameof(CommandLoginIn), m => m.Initialize(nameof(CommandLoginIn), ref m._CommandLoginIn, ref _CommandLoginInLocator,
              model =>
              {
                  object state = nameof(CommandLoginIn);
                  var commandId = nameof(CommandLoginIn);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {

                              await Task.Delay(2000);
                              if (vm.LoginEntity.UserName == vm.RightUserName && vm.LoginEntity.Password == vm.RightPassword)
                              {
                                  vm.CurrentLoginState = LoginState.LoginSuccess;
                                  vm.Message = "Login success!";
                              }
                              else
                              {

                                  vm.CurrentLoginState = LoginState.LoginError;
                                  vm.Message = "Login failed. Do you need help to recovery you password?";
                              }
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion



        public CommandModel CommandStartRecover => _CommandStartRecoverLocator(this).Value;
        #region Property CommandModel CommandStartRecover Setup                
        protected Property<CommandModel> _CommandStartRecover = new Property<CommandModel>(_CommandStartRecoverLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandStartRecoverLocator = RegisterContainerLocator(nameof(CommandStartRecover), m => m.Initialize(nameof(CommandStartRecover), ref m._CommandStartRecover, ref _CommandStartRecoverLocator,
              model =>
              {
                  object state = nameof(CommandStartRecover);
                  var commandId = nameof(CommandStartRecover);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              vm.CurrentLoginState = LoginState.PasswordRecovery;
                              await Task.Delay(100);
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion



        public CommandModel CommandRecoverySMS => _CommandRecoverySMSLocator(this).Value;
        #region Property CommandModel CommandRecoverySMS Setup                
        protected Property<CommandModel> _CommandRecoverySMS = new Property<CommandModel>(_CommandRecoverySMSLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandRecoverySMSLocator = RegisterContainerLocator(nameof(CommandRecoverySMS), m => m.Initialize(nameof(CommandRecoverySMS), ref m._CommandRecoverySMS, ref _CommandRecoverySMSLocator,
              model =>
              {
                  object state = nameof(CommandRecoverySMS);
                  var commandId = nameof(CommandRecoverySMS);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyFunctionTask(
                        vm,
                        async (e, cancelToken) =>
                        {
                            //Todo: Add RecoverySMS logic here  
                            await Task.Delay(2000);
                            return e;
                        })
                        .DoExecuteUIActionTask(vm, async (t, c) =>
                        {

                            var e = await t.Task;
                            for (int i = 10; i > 0; i--)
                            {
                                vm.CountDownOfSMSResend = i;
                                await Task.Delay(1000);
                            }

                            vm.CountDownOfSMSResend = 0;


                        })
                      .Subscribe()
                      .DisposeWith(vm);


                  var cmdmdl = cmd.CreateCommandModel(state);
                  cmd.ListenCanExecuteObservable(vm.ListenValueChangedEvents(_ => _.CountDownOfSMSResend).Select(_ =>
                    vm.CountDownOfSMSResend == 0));

                  return cmdmdl;
              }));
        #endregion



        public CommandModel CommandRecoverySMSCheck => _CommandRecoverySMSCheckLocator(this).Value;
        #region Property CommandModel CommandRecoverySMSCheck Setup                
        protected Property<CommandModel> _CommandRecoverySMSCheck = new Property<CommandModel>(_CommandRecoverySMSCheckLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandRecoverySMSCheckLocator = RegisterContainerLocator(nameof(CommandRecoverySMSCheck), m => m.Initialize(nameof(CommandRecoverySMSCheck), ref m._CommandRecoverySMSCheck, ref _CommandRecoverySMSCheckLocator,
              model =>
              {
                  object state = nameof(CommandRecoverySMSCheck);
                  var commandId = nameof(CommandRecoverySMSCheck);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              //Todo: Add RecoverySMSCheck logic here  
                              await Task.Delay(2000);
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion


        public CommandModel CommandBackToIndex => _CommandBackToIndexLocator(this).Value;
        #region Property CommandModel CommandBackToIndex Setup                
        protected Property<CommandModel> _CommandBackToIndex = new Property<CommandModel>(_CommandBackToIndexLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandBackToIndexLocator = RegisterContainerLocator(nameof(CommandBackToIndex), m => m.Initialize(nameof(CommandBackToIndex), ref m._CommandBackToIndex, ref _CommandBackToIndexLocator,
              model =>
              {
                  object state = nameof(CommandBackToIndex);
                  var commandId = nameof(CommandBackToIndex);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              //Todo: Add BackToIndex logic here  
                              try
                              {

                                  await vm.StageManager.DefaultStage.Show<Index_Model>();
                              }
                              catch (Exception ex)
                              {
                                  Debug.WriteLine(ex);
                                  throw;
                              }
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion

        /// <summary>
        /// 定义登录状态的枚举
        /// Enumeration that defines login states
        /// </summary>
        public enum LoginState
        {
            /// <summary>登录状态 / Login state</summary>
            Login,
            /// <summary>登录错误状态 / Login error state</summary>
            LoginError,
            /// <summary>登录成功状态 / Login success state</summary>
            LoginSuccess,
            /// <summary>密码恢复状态 / Password recovery state</summary>
            PasswordRecovery,
            /// <summary>密码恢复错误状态 / Password recovery error state</summary>
            PasswordRecoveryError,
            /// <summary>密码恢复成功状态 / Password recovery success state</summary>
            PasswordRecoverySuccess,
        }

    }

    /// <summary>
    /// 登录实体，包含用户名和密码的验证逻辑
    /// Login entity that contains validation logic for username and password
    /// </summary>
    public class LoginEntity : Bindable<LoginEntity>
    {
        /// <summary>
        /// 初始化登录实体的新实例，配置验证规则
        /// Initializes a new instance of login entity with validation rules
        /// </summary>
        public LoginEntity()
        {
            this.ValidateOnChange(_ => _.UserName, _ => _.Password)
                .FocusOnProperty(
                    x => x.Password,
                    x => x
                        .Required()
                        .StringLength(6, 10))
                .FocusOnProperty(
                    x => x.UserName,
                    x => x
                        .Required()
                        .StringLength(8, 22))
                .MismatchThenMessage(
                    m =>
                        m.UserName?.Length + m.Password?.Length > 25,
                    () =>
                        "the total Length Shoud Bigger Than 25");

        }

        /// <summary>
        /// 获取或设置用户名
        /// Gets or sets the username
        /// </summary>
        public string UserName { get => _UserNameLocator(this).Value; set => _UserNameLocator(this).SetValueAndTryNotify(value); }
        #region Property string  UserName Setup        
        protected Property<string> _UserName = new Property<string>(_UserNameLocator);
        static Func<BindableBase, ValueContainer<string>> _UserNameLocator = RegisterContainerLocator(nameof(UserName), m => m.Initialize(nameof(UserName), ref m._UserName, ref _UserNameLocator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置密码
        /// Gets or sets the password
        /// </summary>
        public string Password { get => _PasswordLocator(this).Value; set => _PasswordLocator(this).SetValueAndTryNotify(value); }
        #region Property string Password Setup        
        protected Property<string> _Password = new Property<string>(_PasswordLocator);
        static Func<BindableBase, ValueContainer<string>> _PasswordLocator = RegisterContainerLocator(nameof(Password), m => m.Initialize(nameof(Password), ref m._Password, ref _PasswordLocator, () => default(string)));
        #endregion


        //Use propvm + tab +tab  to create a new property of bindable here
    }
    
    /// <summary>
    /// 恢复实体，包含恢复密码的手机号和短信验证码
    /// Recovery entity that contains phone number and SMS code for password recovery
    /// </summary>
    public class RecoveryEntity : Bindable<RecoveryEntity>
    {
        /// <summary>
        /// 初始化恢复实体的新实例，配置验证规则
        /// Initializes a new instance of recovery entity with validation rules
        /// </summary>
        public RecoveryEntity()
        {
            this.ValidateOnChange(x => x.UserPhone).Required().StringLength(6, 20);
            this.ValidateOnChange(x => x.SMSCode).Required().StringLength(6, 20);

        }

        /// <summary>
        /// 获取或设置用户手机号
        /// Gets or sets the user phone number
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Length shoule between 6-20")]
        [Phone]
        public string UserPhone { get => _UserPhoneLocator(this).Value; set => _UserPhoneLocator(this).SetValueAndTryNotify(value); }
        #region Property string UserPhone Setup        
        protected Property<string> _UserPhone = new Property<string>(_UserPhoneLocator);
        static Func<BindableBase, ValueContainer<string>> _UserPhoneLocator = RegisterContainerLocator(nameof(UserPhone), m => m.Initialize(nameof(UserPhone), ref m._UserPhone, ref _UserPhoneLocator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置短信验证码
        /// Gets or sets the SMS verification code
        /// </summary>
        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "5 digital numbers")]
        public string SMSCode { get => _SMSCodeLocator(this).Value; set => _SMSCodeLocator(this).SetValueAndTryNotify(value); }
        #region Property string SMSCode Setup        
        protected Property<string> _SMSCode = new Property<string>(_SMSCodeLocator);
        static Func<BindableBase, ValueContainer<string>> _SMSCodeLocator = RegisterContainerLocator(nameof(SMSCode), m => m.Initialize(nameof(SMSCode), ref m._SMSCode, ref _SMSCodeLocator, () => default(string)));
        #endregion

    }

    #region ViewModelRegistry
    /// <summary>
    /// 视图模型注册表的内部分部类
    /// Internal partial class for view model registry
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 登录演示配置条目的静态操作
        /// Static action for LoginDemo configuration entry
        /// </summary>
        internal static Action<MVVMSidekickOptions> LoginDemoConfigEntry = AddConfigure(opt => opt.RegisterViewModel<LoginDemo_Model>());
    }
    #endregion 
}

