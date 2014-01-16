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
using MVVMSidekick.Patterns.Tree;
using MVVMSidekick.Patterns.ItemsAndSelection;
namespace Samples.ViewModels
{

    public class MultiLevelSelection_Model : ViewModelBase<MultiLevelSelection_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public MultiLevelSelection_Model()
        {
            if (IsInDesignMode)
            {

            }

            var rval = Enumerable.Range(0, 3)
                    .Select(
                        i =>
                        {
                            var item = new TreeItemModel<string, string> { State = "Item" + i.ToString() };
                            PopulateVNext(item);

                            foreach (TreeItemModel<string, string> itmlv2 in item.Children)
                            {
                                PopulateVNext(itmlv2);
                            }
                            return item;
                        }
                    );


            foreach (var item in rval)
            {
                TreeRoots.Add(item);
            }




            Level1Selection.GetValueContainer(x => x.SelectedItem)
                .GetNullObservable()
                .Where(_ => Level1Selection.SelectedItem != null)
                .Subscribe(
                _ =>
                {
                    Level2Selection.Items = new ObservableCollection<TreeItemModel<string, string>>(Level1Selection.SelectedItem.Children.Cast<TreeItemModel<string, string>>());
                }
                );


            Level2Selection.GetValueContainer(x => x.SelectedItem)
           .GetNullObservable()
           .Where(_ => Level2Selection.SelectedItem != null)
           .Subscribe(
           _ =>
           {
               Level3Selection.Items = new ObservableCollection<TreeItemModel<string, string>>(Level2Selection.SelectedItem.Children.Cast<TreeItemModel<string, string>>());
           }
           );


        }

        private static void PopulateVNext(TreeItemModel<string, string> item)
        {
            var lvn = Enumerable.Range(0, 3)
                 .Select(
                    j =>
                    {
                        var itemlv1 = new TreeItemModel<string, string> { State = item.State + j.ToString(), Parent = item };
                        return itemlv1;
                    }
                 );

            foreach (var itlv in lvn)
            {
                item.Children.Add(itlv);
            }
        }


        //propvm tab tab string tab Title

        #region Property TreeRoots
        public ObservableCollection<TreeItemModel<string, string>> TreeRoots
        {
            get { return _TreeRootsLocator(this).Value; }
            set { _TreeRootsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<TreeItemModel <string,string>> TreeRoots Setup
        protected Property<ObservableCollection<TreeItemModel<string, string>>> _TreeRoots = new Property<ObservableCollection<TreeItemModel<string, string>>> { LocatorFunc = _TreeRootsLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<TreeItemModel<string, string>>>> _TreeRootsLocator = RegisterContainerLocator<ObservableCollection<TreeItemModel<string, string>>>("TreeRoots", model => model.Initialize("TreeRoots", ref model._TreeRoots, ref _TreeRootsLocator, _TreeRootsDefaultValueFactory));
        static Func<ObservableCollection<TreeItemModel<string, string>>> _TreeRootsDefaultValueFactory = () => { return new ObservableCollection<TreeItemModel<string, string>>(); };
        #endregion
        #endregion





        #region Property Level1Selection
        public ItemsAndSelectionGroup<TreeItemModel<string, string>> Level1Selection
        {
            get { return _Level1SelectionLocator(this).Value; }
            set { _Level1SelectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ItemsAndSelectionGroup <TreeItemModel<string,string>> Level1Selection Setup
        protected Property<ItemsAndSelectionGroup<TreeItemModel<string, string>>> _Level1Selection = new Property<ItemsAndSelectionGroup<TreeItemModel<string, string>>> { LocatorFunc = _Level1SelectionLocator };
        static Func<BindableBase, ValueContainer<ItemsAndSelectionGroup<TreeItemModel<string, string>>>> _Level1SelectionLocator = RegisterContainerLocator<ItemsAndSelectionGroup<TreeItemModel<string, string>>>("Level1Selection", model => model.Initialize("Level1Selection", ref model._Level1Selection, ref _Level1SelectionLocator, _Level1SelectionDefaultValueFactory));
        static Func<BindableBase, ItemsAndSelectionGroup<TreeItemModel<string, string>>> _Level1SelectionDefaultValueFactory =
            model =>
            { return new ItemsAndSelectionGroup<TreeItemModel<string, string>> { Items = CastToCurrentType(model).TreeRoots }; };
        #endregion
        #endregion



        #region Property Level2Selection
        public ItemsAndSelectionGroup<TreeItemModel<string, string>> Level2Selection
        {
            get { return _Level2SelectionLocator(this).Value; }
            set { _Level2SelectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ItemsAndSelectionGroup<TreeItemModel<string,string>> Level2Selection Setup
        protected Property<ItemsAndSelectionGroup<TreeItemModel<string, string>>> _Level2Selection = new Property<ItemsAndSelectionGroup<TreeItemModel<string, string>>> { LocatorFunc = _Level2SelectionLocator };
        static Func<BindableBase, ValueContainer<ItemsAndSelectionGroup<TreeItemModel<string, string>>>> _Level2SelectionLocator = RegisterContainerLocator<ItemsAndSelectionGroup<TreeItemModel<string, string>>>("Level2Selection", model => model.Initialize("Level2Selection", ref model._Level2Selection, ref _Level2SelectionLocator, _Level2SelectionDefaultValueFactory));
        static Func<ItemsAndSelectionGroup<TreeItemModel<string, string>>> _Level2SelectionDefaultValueFactory =
            () => { return new ItemsAndSelectionGroup<TreeItemModel<string, string>>(); };
        #endregion
        #endregion



        #region Property Level3Selection
        public ItemsAndSelectionGroup<TreeItemModel<string, string>> Level3Selection
        {
            get { return _Level3SelectionLocator(this).Value; }
            set { _Level3SelectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ItemsAndSelectionGroup<TreeItemModel<string,string>> Level3Selection Setup
        protected Property<ItemsAndSelectionGroup<TreeItemModel<string, string>>> _Level3Selection = new Property<ItemsAndSelectionGroup<TreeItemModel<string, string>>> { LocatorFunc = _Level3SelectionLocator };
        static Func<BindableBase, ValueContainer<ItemsAndSelectionGroup<TreeItemModel<string, string>>>> _Level3SelectionLocator = RegisterContainerLocator<ItemsAndSelectionGroup<TreeItemModel<string, string>>>("Level3Selection", model => model.Initialize("Level3Selection", ref model._Level3Selection, ref _Level3SelectionLocator, _Level3SelectionDefaultValueFactory));
        static Func<ItemsAndSelectionGroup<TreeItemModel<string, string>>> _Level3SelectionDefaultValueFactory =
            () => { return new ItemsAndSelectionGroup<TreeItemModel<string, string>>(); };
        #endregion
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

}

