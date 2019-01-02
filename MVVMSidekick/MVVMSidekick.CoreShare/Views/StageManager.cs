using System;
using System.Collections.Generic;
using MVVMSidekick.ViewModels;
using System.Windows;



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

    /// <summary>
    /// The abstract  for frame/contentcontrol. VM can access this class to Show other vm and vm's mapped view.
    /// </summary>
    public class StageManager : DependencyObject, IStageManager
    {


        static StageManager()
        {
            NavigatorBeaconsKey = nameof(NavigatorBeaconsKey);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StageManager"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public StageManager(IViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public StageManager()
        { }

        public IViewModel ViewModel { get; set; }

        /// <summary>
        /// This Key is a prefix for register keys. 
        /// The stage registeration store the String-Element-Mapping in view's Resource Dictionary(Resource property). 
        /// This can help not to overwrite the resources already defined.
        /// </summary>
        public static string NavigatorBeaconsKey;
        private WeakReference _CurrentBindingView = new WeakReference(null);
        /// <summary>
        /// Get the currently binded view of this stagemanager. A stagemanager is for a certain view. If viewmodel is not binded to a view, the whole thing cannot work.
        /// </summary>
        public IView CurrentBindingView
        {
            get => _CurrentBindingView.IsAlive ? _CurrentBindingView.Target as IView : null;
            set => _CurrentBindingView = new WeakReference(value);
        }






        /// <summary>
        /// Initializes the parent.
        /// </summary>
        /// <param name="parentLocator">The parent locator.</param>
        public void InitParent(Func<object> parentLocator)
        {
            _parentLocator = parentLocator;

        }

        private Func<object> _parentLocator;


        #region Attached Property

        /// <summary>
        /// Gets the beacon.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
#if WPF
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        [AttachedPropertyBrowsableForType(typeof(Frame))]
        [AttachedPropertyBrowsableForType(typeof(Window))]
#endif



        public static string GetBeacon(DependencyObject obj)
        {
            return (string)obj.GetValue(BeaconProperty);
        }

        /// <summary>Sets the beacon.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
#if WPF
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        [AttachedPropertyBrowsableForType(typeof(Frame))]
        [AttachedPropertyBrowsableForType(typeof(Window))]
#endif


        public static void SetBeacon(DependencyObject obj, string value)
        {
            if (!Utilities.Runtime.IsInDesignMode)
            {

                obj.SetValue(BeaconProperty, value);
            }
        }

        /// <summary>
        /// The beacon property
        /// </summary>
        public static readonly DependencyProperty BeaconProperty =
            DependencyProperty.RegisterAttached("Beacon", typeof(string), typeof(StageManager), new PropertyMetadata(null,
                   (o, p) =>
                   {
                       string name = (p.NewValue as string);
                       FrameworkElement target = o as FrameworkElement;

                       target.Loaded +=
                           (_1, _2)
                           =>
                           {
                               StageManager.RegisterTargetBeacon(name, target);
                           };
                   }

                   ));





        #endregion


        internal FrameworkElement LocateTargetContainer(IView view, ref string targetContainerName, IViewModel sourceVM)
        {


            targetContainerName = targetContainerName ?? "";
            FrameworkElement viewele = view.ViewObject as FrameworkElement;



            Dictionary<string, FrameworkElement> dic = GetOrCreateBeacons(sourceVM.StageManager.CurrentBindingView.ViewObject as FrameworkElement);
            dic.TryGetValue(targetContainerName, out FrameworkElement target);


            if (target == null)
            {
                target = _parentLocator() as FrameworkElement;
            }

            if (target == null)
            {
#if WPF
                if (string.IsNullOrWhiteSpace(targetContainerName) && view is IWindowView)
                {

                    target = sourceVM.StageManager.CurrentBindingView.ViewObject as FrameworkElement;

                }
                else
#endif
                {
                    ContentControl vieweleCt = viewele as ContentControl;
                    if (vieweleCt != null)
                    {
                        target = vieweleCt.Content as FrameworkElement;
                    }
                }

            }
            return target;
        }




        private static Dictionary<string, FrameworkElement> GetOrCreateBeacons(FrameworkElement view)
        {
            Dictionary<string, FrameworkElement> dic;
#if NETFX_CORE
            if (!view.Resources.ContainsKey(NavigatorBeaconsKey))
#else
            if (!view.Resources.Contains(NavigatorBeaconsKey))
#endif
            {
                dic = new Dictionary<string, FrameworkElement>();
                view.Resources.Add(NavigatorBeaconsKey, dic);
            }
            else
            {
                dic = view.Resources[NavigatorBeaconsKey] as Dictionary<string, FrameworkElement>;
            }

            return dic;
        }

        /// <summary>
        /// Registers the target beacon.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="target">The target.</param>
        public static void RegisterTargetBeacon(string name, FrameworkElement target)
        {
            if (!Utilities.Runtime.IsInDesignMode)
            {


                FrameworkElement view = LocateIView(target);

                Dictionary<string, FrameworkElement> beacons = GetOrCreateBeacons(view);
                beacons[name] = target;
            }

        }

        private static FrameworkElement LocateIView(FrameworkElement target)
        {
            FrameworkElement view = target;

            while (view != null)
            {
                //	var tryView = GetViewOfStage(target) as FrameworkElement;
                //	if (tryView != null)
                //	{
                //		return tryView;
                //	}

                FrameworkElement tryView = view.Parent as FrameworkElement;

                if (tryView != null)
                {
                    view = tryView;
                }
                else
                {
                    tryView = VisualTreeHelper.GetParent(view) as FrameworkElement;
                    view = tryView;
                }
                if (view is IView || view.GetViewDisguise() != null)
                {
                    break;
                }
            }
            return view;
        }







        /// <summary>
        /// Gets or sets the default stage.
        /// </summary>
        /// <value>
        /// The default stage.
        /// </value>
        public IStage DefaultStage => this[""];






        /// <summary>
        /// Gets the <see cref="Stage"/> with the specified beacon key.
        /// </summary>
        /// <value>
        /// The <see cref="Stage"/>.
        /// </value>
        /// <param name="beaconKey">The beacon key.</param>
        /// <returns></returns>
        public IStage this[string beaconKey]
        {
            get
            {
                FrameworkElement fr = LocateTargetContainer(CurrentBindingView, ref beaconKey, ViewModel);
                if (fr != null)
                {
                    return new Stage(fr, beaconKey, this);
                }
                else
                {
                    return null;
                }
            }
        }
    }

}