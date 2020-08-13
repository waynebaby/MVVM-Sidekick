using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MVVMSidekickWPFDemo.Glue
{


    /// <summary>
    /// because the behavior is not stable in now-a-days, i have to create basic commands myself
    /// </summary>
    public class UpdateVisualStateCommand : FrameworkElement, ICommand
    {



        public FrameworkElement TargetObject
        {
            get { return (FrameworkElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(UpdateVisualStateCommand), new PropertyMetadata(null));




        public string ViewStateString
        {
            get { return (string)GetValue(ViewStateStringProperty); }
            set { SetValue(ViewStateStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewStateString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewStateStringProperty =
            DependencyProperty.Register("ViewStateString", typeof(string), typeof(UpdateVisualStateCommand), new PropertyMetadata("", (o, a) =>
            {
                var obj = o as UpdateVisualStateCommand;
                if (Comparer<string>.Default.Compare(a.NewValue as string, a.OldValue as string) != 0)
                {
                    obj?.ViewStateChanged?.Invoke(o, new RoutedEventArgs());
                }
            }));



        public event EventHandler<RoutedEventArgs> ViewStateChanged;





        public event EventHandler CanExecuteChanged;

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
