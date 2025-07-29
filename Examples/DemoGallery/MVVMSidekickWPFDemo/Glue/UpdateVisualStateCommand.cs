using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MVVMSidekickWPFDemo.Glue
{

    /// <summary>
    /// 更新视觉状态命令类，由于行为在当前环境下不稳定，需要自己创建基本命令
    /// Update visual state command class, because the behavior is not stable in now-a-days, I have to create basic commands myself
    /// </summary>
    public class UpdateVisualStateCommand : FrameworkElement, ICommand
    {

        /// <summary>
        /// 获取或设置目标对象
        /// Gets or sets the target object
        /// </summary>
        public FrameworkElement TargetObject
        {
            get { return (FrameworkElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        /// <summary>
        /// TargetObject的依赖属性，支持动画、样式、绑定等功能
        /// Dependency property for TargetObject that enables animation, styling, binding, etc.
        /// </summary>
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(UpdateVisualStateCommand), new PropertyMetadata(null));

        /// <summary>
        /// 获取或设置视图状态字符串
        /// Gets or sets the view state string
        /// </summary>
        public string ViewStateString
        {
            get { return (string)GetValue(ViewStateStringProperty); }
            set { SetValue(ViewStateStringProperty, value); }
        }

        /// <summary>
        /// ViewStateString的依赖属性，支持动画、样式、绑定等功能，并在值变化时触发事件
        /// Dependency property for ViewStateString that enables animation, styling, binding, etc., and triggers events on value changes
        /// </summary>
        public static readonly DependencyProperty ViewStateStringProperty =
            DependencyProperty.Register("ViewStateString", typeof(string), typeof(UpdateVisualStateCommand), new PropertyMetadata("", (o, a) =>
            {
                var obj = o as UpdateVisualStateCommand;
                if (Comparer<string>.Default.Compare(a.NewValue as string, a.OldValue as string) != 0)
                {
                    obj?.ViewStateChanged?.Invoke(o, new RoutedEventArgs());
                }
            }));

        /// <summary>
        /// 视图状态变化时触发的事件
        /// Event triggered when view state changes
        /// </summary>
        public event EventHandler<RoutedEventArgs> ViewStateChanged;

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

        public void Execute(object parameter)
        {
            if (ViewStateString != null)
            {
      
                VisualStateManager.GoToElementState(TargetObject, ViewStateString, true);
            }
        }
    }
}
