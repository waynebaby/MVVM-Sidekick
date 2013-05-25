using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace $rootnamespace$.ViewModels
{

    [DataContract]
    public class $safeitemrootname$ : ViewModelBase<$safeitemrootname$>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<BindableBase ,String> _TitleDefaultValueFactory = m=>m.GetType().Name  ;
        #endregion

    
        protected override async Task OnBindedToView(IView view, IViewModel oldValue)
        {
            await base.OnBindedToView(view, oldValue);
            // This method will be called when this VM is set to a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }

        protected override async Task OnUnbindedFromView(IView view, IViewModel newValue)
        {
            await base.OnUnbindedFromView(view, newValue);
            // This method will be called when this VM is removed from a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }
      


    }
	
}

