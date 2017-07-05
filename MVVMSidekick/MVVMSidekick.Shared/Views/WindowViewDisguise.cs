


#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
using System.Windows;
using MVVMSidekick.ViewModels;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
						   using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif

namespace MVVMSidekick.Views
{

#if WPF
    public class WindowViewDisguise : ViewDisguiseBase<Window, WindowViewDisguise>
    {
        public WindowViewDisguise(Window assocatedObject) : base(assocatedObject)
        {
            assocatedObject.Loaded += ViewHelper.ViewLoadCallBack;
            assocatedObject.Unloaded += ViewHelper.ViewUnloadCallBack;

        }


        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ViewModelProperty)
            {
                var vm = e.NewValue as IViewModel;
                if (vm != null)
                {
                    vm.IsDisposingWhenUnloadRequired = true;
                }
            }
            base.OnPropertyChanged(e);
        }
        public override ViewType ViewType => Views.ViewType.Page;

        public override object ViewContentObject
        {
            get { return base.AssocatedObject.Content; }
            set { AssocatedObject.Content = value; }
        }


        public override object Parent
        {
            get
            {

                return this.AssocatedObject.Parent;

            }
        }
    }
#endif
}
