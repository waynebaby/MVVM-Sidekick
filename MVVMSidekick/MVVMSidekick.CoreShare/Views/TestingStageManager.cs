using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;



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
        private Dictionary<string, IStage> _currentStages = new Dictionary<string, IStage>();

        public IStage this[string beaconKey]
        {
            get
            {
                _currentStages.TryGetValue(beaconKey, out IStage stage);
                if (stage == null)
                {
                    stage = new TestingStage()
                    {
                        BeaconKey = beaconKey,
                        CanGoBack = true,
                        CanGoForward = true,
                        IsGoBackSupported = true,
                        IsGoForwardSupported = true,
                        Target = null
                    };
                    _currentStages[beaconKey] = stage;
                }

                return stage;
            }
        }

        public IView CurrentBindingView
        {
            get; set;
        }

        public IStage DefaultStage
        {
            get => this[""];

            set => _currentStages[""] = value;
        }

        public IViewModel ViewModel { get; set; }

        public void InitParent(Func<object> parentLocator)
        {

        }
    }
}
