

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
using MVVMSidekick.Services;



#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;

using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
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
    public abstract class ViewDisguiseBase<TViewElement, TViewDisguise> : DependencyObject, IViewDisguise
        where TViewElement : FrameworkElement
        where TViewDisguise : ViewDisguiseBase<TViewElement, TViewDisguise>
    {
        public ViewDisguiseBase(TViewElement assocatedObject)
        {
            _assocatedObject = assocatedObject;
        }
        public TViewElement AssocatedObject { get => _assocatedObject; }
        TViewElement _assocatedObject;

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public IViewModel ViewModel
        {
            get
            {
                var vm = GetValue(ViewModelProperty) as IViewModel;
                var content = this.GetContentAndCreateIfNull();
                if (vm == null)
                {

                    vm = content.DataContext as IViewModel;
                    SetValue(ViewModelProperty, vm);

                }
                else
                {
                    IView view = this;


                    if (!Object.ReferenceEquals(content.DataContext, vm))
                    {

                        content.DataContext = vm;
                    }
                }
                return vm;
            }
            set
            {
                SetValue(ViewModelProperty, value);
                var c = this.GetContentAndCreateIfNull();
                if (!Object.ReferenceEquals(c.DataContext, value))
                {
                    c.DataContext = value;
                }

            }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IViewModel), typeof(TViewDisguise), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));


        public abstract object ViewContentObject { get; set; }
        public abstract object Parent { get; }

        public object ViewObject => _assocatedObject;
    }
}
