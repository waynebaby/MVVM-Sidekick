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

namespace Samples.ViewModels
{

    [DataContract]
    public class ViewNavigation_Child_Model : ViewModelBase<ViewNavigation_Child_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
		public ViewNavigation_Child_Model()
		{
			Title = BindableInstanceId;
		
		}
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<BindableBase, String> _TitleDefaultValueFactory = m => m.GetType().Name;
        #endregion

        
        public int Index
        {
            get { return _IndexLocator(this).Value; }
            set { _IndexLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int Index Setup
        protected Property<int> _Index = new Property<int> { LocatorFunc = _IndexLocator };
        static Func<BindableBase, ValueContainer<int>> _IndexLocator = RegisterContainerLocator<int>("Index", model => model.Initialize("Index", ref model._Index, ref _IndexLocator, _IndexDefaultValueFactory));
        static Func<int> _IndexDefaultValueFactory = null;
        #endregion

        
        public String ColorName
        {
            get { return _ColorNameLocator(this).Value; }
            set { _ColorNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String ColorName Setup
        protected Property<String> _ColorName = new Property<String> { LocatorFunc = _ColorNameLocator };
        static Func<BindableBase, ValueContainer<String>> _ColorNameLocator = RegisterContainerLocator<String>("ColorName", model => model.Initialize("ColorName", ref model._ColorName, ref _ColorNameLocator, _ColorNameDefaultValueFactory));
        static Func<String> _ColorNameDefaultValueFactory = null;
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

        
        public CommandModel<ReactiveCommand, String> CommandNavigateBack
        {
            get { return _CommandNavigateBackLocator(this).Value; }
            set { _CommandNavigateBackLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavigateBack Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavigateBack = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavigateBackLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavigateBackLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandNavigateBack", model => model.Initialize("CommandNavigateBack", ref model._CommandNavigateBack, ref _CommandNavigateBackLocator, _CommandNavigateBackDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavigateBackDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                
                          if (  vm.StageManager.DefaultStage.CanGoBack  )
                          {
                              vm.StageManager.DefaultStage.Frame.GoBack();
                          }
           

                  })
                  .DisposeWith(vm); 
                return cmd.CreateCommandModel("NavigateBack");
            };
        #endregion


    }

}

