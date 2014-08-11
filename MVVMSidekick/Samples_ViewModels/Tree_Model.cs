using MVVMSidekick;
using MVVMSidekick.Patterns.Tree;
using MVVMSidekick.Reactive;
using MVVMSidekick.Utilities;
using MVVMSidekick.ViewModels;
using Samples.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMSidekick.Collections;
namespace Samples.ViewModels
{

    public class Tree_Model : ViewModelBase<Tree_Model>
    {

        public Tree_Model()
        {

            var n = new LV1
                {
                    Value = "A",


                };
            n.Children.Add(new LV2() { Value ="B"});


            RootNodes.Add (n);
            RootNodes.Add(n);
            RootNodes.Add(n);
            RootNodes.Add(n);
            RootNodes.Add(n);
        }

        public ObservableCollection<TreeViewItemModel<string>> RootNodes
        {
            get { return _RootNodesLocator(this).Value; }
            set { _RootNodesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<TreeViewItemModel<string >>  RootNodes Setup
        protected Property<ObservableCollection<TreeViewItemModel<string>>> _RootNodes = new Property<ObservableCollection<TreeViewItemModel<string>>> { LocatorFunc = _RootNodesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<TreeViewItemModel<string>>>> _RootNodesLocator = RegisterContainerLocator<ObservableCollection<TreeViewItemModel<string>>>("RootNodes", model => model.Initialize("RootNodes", ref model._RootNodes, ref _RootNodesLocator, _RootNodesDefaultValueFactory));
        static Func<ObservableCollection<TreeViewItemModel<string>>> _RootNodesDefaultValueFactory = () => new ObservableCollection<TreeViewItemModel<string>>();
        #endregion

        #region Life Time Event Handling
    
        ///// <summary>
        ///// This will be invoked by view when this viewmodel instance is set to view's ViewModel property. 
        ///// </summary>
        ///// <param name="view">Set target</param>
        ///// <param name="oldValue">Value before set.</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        //{
        //    return base.OnBindedToView(view, oldValue);
        //}

        ///// <summary>
        ///// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.
        ///// </summary>
        ///// <param name="view">Overwrite target view.</param>
        ///// <param name="newValue">The value replacing </param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
        //{
        //    return base.OnUnbindedFromView(view, newValue);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose infomation</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await TaskExHelper.Yield();
        //}

        #endregion
    }

    public class LV1 : TreeViewItemModel<string>
    { 
    
    }


    public class LV2: TreeViewItemModel<string>
    {

    }




}
