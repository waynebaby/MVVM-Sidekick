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
    public class UpdatePasswordCommand : FrameworkElement, ICommand
    {





        public LoginEntity LoginEntity
        {
            get { return (LoginEntity)GetValue(LoginEntityProperty); }
            set { SetValue(LoginEntityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoginEntity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginEntityProperty =
            DependencyProperty.Register("LoginEntity", typeof(LoginEntity), typeof(UpdatePasswordCommand), new PropertyMetadata(null));





        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (LoginEntity != null)
            {
                LoginEntity.Password = (parameter as PasswordBox)?.Password;
            }
        }
    }
}
