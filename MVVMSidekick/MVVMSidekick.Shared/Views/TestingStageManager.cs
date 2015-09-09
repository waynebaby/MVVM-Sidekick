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
    public class TestingStageManager : IStageManager
    {

        Dictionary<string, IStage> _currentStages = new Dictionary<string, IStage>();

        public IStage this[string beaconKey]
        {
            get
            {
                IStage stage = null;
                _currentStages.TryGetValue(beaconKey, out stage);
                if (stage == null)
                {
                    stage = new TestingStage()
                    {
                        BeaconKey = beaconKey,
                        CanGoBack = true,
                        CanGoForward = true,
                        IsGoBackSupported = true,
                        IsGoForwardSupported = true,
                        Frame = null,
                        Target = null
                    };
                    _currentStages[beaconKey] = stage;
                }

                return stage;
            }
        }

        public IView CurrentBindingView
        {
            get
            {
                return null;
            }
        }

        public IStage DefaultStage
        {
            get
            {
                return this[""];
            }

            set
            {
                _currentStages[""] = value;
            }
        }

        public void InitParent(Func<DependencyObject> parentLocator)
        {
   
        }
    }
}
