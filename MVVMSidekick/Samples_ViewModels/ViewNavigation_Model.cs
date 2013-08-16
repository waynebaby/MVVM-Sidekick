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
    public class ViewNavigation_Model : ViewModelBase<ViewNavigation_Model>
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
        static Func<BindableBase, String> _TitleDefaultValueFactory = m => m.GetType().Name;
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


        public int CurrentIndex
        {
            get { return _CurrentIndexLocator(this).Value; }
            set { _CurrentIndexLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int CurrentIndex Setup
        protected Property<int> _CurrentIndex = new Property<int> { LocatorFunc = _CurrentIndexLocator };
        static Func<BindableBase, ValueContainer<int>> _CurrentIndexLocator = RegisterContainerLocator<int>("CurrentIndex", model => model.Initialize("CurrentIndex", ref model._CurrentIndex, ref _CurrentIndexLocator, _CurrentIndexDefaultValueFactory));
        static Func<int> _CurrentIndexDefaultValueFactory = null;
        #endregion


        static string[] ColorNames = new string[]        {
            "Red",
            "Green",
            "Blue",
            "DarkBlue",
            "Gray"        
        };

        public CommandModel<ReactiveCommand, String> CommandNavigateNext
        {
            get { return _CommandNavigateNextLocator(this).Value; }
            set { _CommandNavigateNextLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavigateNext Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavigateNext = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavigateNextLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavigateNextLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandNavigateNext", model => model.Initialize("CommandNavigateNext", ref model._CommandNavigateNext, ref _CommandNavigateNextLocator, _CommandNavigateNextDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavigateNextDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                // Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                      vm.CurrentIndex++;
                      var newvm = new ViewNavigation_Child_Model() { Index = vm.CurrentIndex, ColorName = ColorNames[vm.CurrentIndex % ColorNames.Length] };
                      await vm.StageManager["Children"].Show(newvm);

                  })
                  .DisposeWith(vm);
                return cmd.CreateCommandModel("NavigateNext");
            };
        #endregion




    }

}

