using MVVMSidekickWPFDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMSidekickWPFDemo.Glue
{

    /// <summary>
    /// 更新密码命令类，由于行为在当前环境下不稳定，需要自己创建基本命令
    /// Update password command class, because the behavior is not stable in now-a-days, I have to create basic commands myself
    /// </summary>
    public class UpdatePasswordCommand : FrameworkElement, ICommand
    {

        /// <summary>
        /// 获取或设置登录实体
        /// Gets or sets the login entity
        /// </summary>
        public LoginEntity LoginEntity
        {
            get { return (LoginEntity)GetValue(LoginEntityProperty); }
            set { SetValue(LoginEntityProperty, value); }
        }

        /// <summary>
        /// LoginEntity的依赖属性，支持动画、样式、绑定等功能
        /// Dependency property for LoginEntity that enables animation, styling, binding, etc.
        /// </summary>
        public static readonly DependencyProperty LoginEntityProperty =
            DependencyProperty.Register("LoginEntity", typeof(LoginEntity), typeof(UpdatePasswordCommand), new PropertyMetadata(null));

        /// <summary>
        /// 当命令的可执行状态发生变化时触发的事件
        /// Event triggered when the command's executable state changes
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 确定命令是否可以执行
        /// Determines whether the command can execute
        /// </summary>
        /// <param name="parameter">命令参数 / Command parameter</param>
        /// <returns>始终返回true / Always returns true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 执行命令，更新登录实体的密码
        /// Executes the command, updates the login entity's password
        /// </summary>
        /// <param name="parameter">密码框控件 / Password box control</param>
        public void Execute(object parameter)
        {
            if (LoginEntity != null)
            {
                LoginEntity.Password = (parameter as PasswordBox)?.Password;
            }
        }
    }
}
