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

namespace $rootnamespace$.ViewModels
{


    public  class $fileinputname$_ViewModel :ViewModel<$fileinputname$_ViewModel, $fileinputname$>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        public $fileinputname$_ViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => default(string)));
        #endregion
    }

    #region ViewModelRegistry
    internal partial class ViewModelRegistry : MVVMSidekickViewModelRegistryBase
    {

        internal static Action<MVVMSidekickOptions> $fileinputname$ConfigEntry = AddConfigure(opt => opt.RegisterViewModel<$fileinputname$_ViewModel> ());
    }
    #endregion 
}

