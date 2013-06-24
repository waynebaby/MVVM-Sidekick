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
using MVVMSidekick.Patterns.ItemsAndSelection;

namespace Samples.ViewModels
{

    [DataContract]
    public class ListsAndItemsPattern_Model : ViewModelBase<ListsAndItemsPattern_Model>
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



        
        public ItemsAndSelectionGroup <string> ListGroup
        {
            get { return _ListGroupLocator(this).Value; }
            set { _ListGroupLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroup <string> ListGroup Setup
        protected Property<MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroup <string>> _ListGroup = new Property<MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroup <string>> { LocatorFunc = _ListGroupLocator };
        static Func<BindableBase, ValueContainer<MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroup <string>>> _ListGroupLocator = RegisterContainerLocator<MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroup <string>>("ListGroup", model => model.Initialize("ListGroup", ref model._ListGroup, ref _ListGroupLocator, _ListGroupDefaultValueFactory));
        static Func<MVVMSidekick.Patterns.ItemsAndSelection.ItemsAndSelectionGroup <string>> _ListGroupDefaultValueFactory = null;
        #endregion

        
        public ItemsAndSelectionGroup <string> ComboGroup
        {
            get { return _ComboGroupLocator(this).Value; }
            set { _ComboGroupLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ItemsAndSelectionGroup <string> ComboGroup Setup
        protected Property<ItemsAndSelectionGroup <string>> _ComboGroup = new Property<ItemsAndSelectionGroup <string>> { LocatorFunc = _ComboGroupLocator };
        static Func<BindableBase, ValueContainer<ItemsAndSelectionGroup <string>>> _ComboGroupLocator = RegisterContainerLocator<ItemsAndSelectionGroup <string>>("ComboGroup", model => model.Initialize("ComboGroup", ref model._ComboGroup, ref _ComboGroupLocator, _ComboGroupDefaultValueFactory));
        static Func<ItemsAndSelectionGroup <string>> _ComboGroupDefaultValueFactory = null;
        #endregion


        
        public ItemsAndSelectionGroup <string> ItemsGroup
        {
            get { return _ItemsGroupLocator(this).Value; }
            set { _ItemsGroupLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ItemsAndSelectionGroup <string> ItemsGroup Setup
        protected Property<ItemsAndSelectionGroup <string>> _ItemsGroup = new Property<ItemsAndSelectionGroup <string>> { LocatorFunc = _ItemsGroupLocator };
        static Func<BindableBase, ValueContainer<ItemsAndSelectionGroup <string>>> _ItemsGroupLocator = RegisterContainerLocator<ItemsAndSelectionGroup <string>>("ItemsGroup", model => model.Initialize("ItemsGroup", ref model._ItemsGroup, ref _ItemsGroupLocator, _ItemsGroupDefaultValueFactory));
        static Func<ItemsAndSelectionGroup <string>> _ItemsGroupDefaultValueFactory = null;
        #endregion



    }

}

