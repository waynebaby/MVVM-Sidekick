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
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekickBlazorDemo.Pages.ViewModels
{


    public class ParameterDemo_Model : ViewModel<ParameterDemo_Model, ParameterDemo>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        public ParameterDemo_Model(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }


        public override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        public override void OnInitialized()
        {
            base.OnInitialized();
        }
        public string P1 { get => _P1Locator(this).Value; set => _P1Locator(this).SetValueAndTryNotify(value); }
        #region Property string P1 Setup        
        protected Property<string> _P1 = new Property<string>(_P1Locator);
        static Func<BindableBase, ValueContainer<string>> _P1Locator = RegisterContainerLocator(nameof(P1), m => m.Initialize(nameof(P1), ref m._P1, ref _P1Locator, () => default(string)));
        #endregion


        public string P2 { get => _P2Locator(this).Value; set => _P2Locator(this).SetValueAndTryNotify(value); }
        #region Property string P2 Setup        
        protected Property<string> _P2 = new Property<string>(_P2Locator);
        static Func<BindableBase, ValueContainer<string>> _P2Locator = RegisterContainerLocator(nameof(P2), m => m.Initialize(nameof(P2), ref m._P2, ref _P2Locator, () => default(string)));
        #endregion



        public string P3 { get => _P3Locator(this).Value; set => _P3Locator(this).SetValueAndTryNotify(value); }
        #region Property string P3 Setup        
        protected Property<string> _P3 = new Property<string>(_P3Locator);
        static Func<BindableBase, ValueContainer<string>> _P3Locator = RegisterContainerLocator(nameof(P3), m => m.Initialize(nameof(P3), ref m._P3, ref _P3Locator, () => default(string)));
        #endregion



        public string DifferentNamedProperty { get => _DifferentNamedPropertyLocator(this).Value; set => _DifferentNamedPropertyLocator(this).SetValueAndTryNotify(value); }
        #region Property string DifferentNamedProperty Setup        
        protected Property<string> _DifferentNamedProperty = new Property<string>(_DifferentNamedPropertyLocator);
        static Func<BindableBase, ValueContainer<string>> _DifferentNamedPropertyLocator = RegisterContainerLocator(nameof(DifferentNamedProperty), m => m.Initialize(nameof(DifferentNamedProperty), ref m._DifferentNamedProperty, ref _DifferentNamedPropertyLocator, () => default(string)));
        #endregion




        public long AssignableTypedProperty { get => _AssignableTypedPropertyLocator(this).Value; set => _AssignableTypedPropertyLocator(this).SetValueAndTryNotify(value); }
        #region Property long AssignableTypedProperty Setup        
        protected Property<long> _AssignableTypedProperty = new Property<long>(_AssignableTypedPropertyLocator);
        static Func<BindableBase, ValueContainer<long>> _AssignableTypedPropertyLocator = RegisterContainerLocator(nameof(AssignableTypedProperty), m => m.Initialize(nameof(AssignableTypedProperty), ref m._AssignableTypedProperty, ref _AssignableTypedPropertyLocator, () => default(long)));
        #endregion


    }

    #region ViewModelRegistry
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {

        internal static Action<MVVMSidekickOptions> ParameterDemoConfigEntry = AddConfigure(opt => opt.RegisterViewModel<ParameterDemo_Model>());
    }
    #endregion 
}

