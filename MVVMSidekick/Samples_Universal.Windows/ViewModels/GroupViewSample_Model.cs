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
using MVVMSidekick.Collections.CollectionView;
using MVVMSidekick.Collections;
using Windows.UI.Xaml.Data;

namespace Samples.ViewModels
{

    public enum CollectionViewType
    {
        Normal,
        Loading,
        Grouped
    }


    [DataContract]
    public class GroupViewSample_Model : ViewModelBase<GroupViewSample_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public GroupViewSample_Model()
        {

        }

        public void InitData(CollectionViewType type)
        {
            var rnd = new Random();
            var grps = Enumerable.Range(0, 100)
                .Select(i =>
                    new
                    {
                        Index = i,
                        Item = new TreeViewItemModel<string>
                        {
                            Value = "Item_" + i.ToString()
                        }
                    })
                .GroupBy(itm => (int)(itm.Index / 10))
                .Select(grp =>
                    new TreeViewItemModel<string>
                    {
                        Value = "Group " + grp.Key.ToString(),
                        Children = new ObservableCollection<ITreeItem<object, TreeViewItemState>>(
                                grp.Select(gi => gi.Item)
                            )
                    }).ToArray();

            var items =
                   grps.SelectMany(x => x.Children.Select(c =>
                   {
                       var mc = c as TreeViewItemModel<string>;
                       if (mc != null)
                       {
                           mc.Parent = x;
                       }

                       return mc;
                   }));




            switch (type)
            {
                case CollectionViewType.Normal:

                    CollectionView = new CollectionView<TreeViewItemModel<string>>(items);
                    break;
                case CollectionViewType.Loading:
                    var loader = new CollectionViewIncrementalLoader<TreeViewItemModel<string>>
                    (
                        async (cv, cnt) =>
                        {
                            return new IncrementalLoadResult<TreeViewItemModel<string>>
                            {
                                HaveMore = true,
                                NewItems = Enumerable.Range(0, cnt)
                                .Select(
                                  i => new TreeViewItemModel<string> { Value = "New_" + i.ToString(), Parent = grps[i % 10] }
                                )
                                .ToArray()

                            };

                        },
                         (cv) => true
                    );
                    CollectionView = new CollectionView<TreeViewItemModel<string>>(items, loader);
                    break;
                case CollectionViewType.Grouped:
                    var igrps = CollectionViewGroupCollection<TreeViewItemModel<string>>.Create(
                          x => x.Parent,
                          x => x.Parent);
                    CollectionView = new CollectionView<TreeViewItemModel<string>>(items, igrps);
                    break;
                default:
                    break;
            }



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


        protected override async Task OnBindedToView(IView view, IViewModel oldValue)
        {
            //InitData(CollectionViewType.Normal);
            await base.OnBindedToView(view, oldValue);


            // This method will be called when this VM is set to a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.

        }
        protected override Task OnBindedViewLoad(IView view)
        {
            if (IsInDesignMode)
                InitData(CollectionViewType.Normal);
            return base.OnBindedViewLoad(view);


        }
        protected override async Task OnUnbindedFromView(IView view, IViewModel newValue)
        {
            await base.OnUnbindedFromView(view, newValue);
            // This method will be called when this VM is removed from a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }



        public CollectionView<TreeViewItemModel<string>> CollectionView
        {
            get { return _CollectionViewLocator(this).Value; }
            set { _CollectionViewLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CollectionView< TreeViewItemModel<string>>  CollectionView Setup
        protected Property<CollectionView<TreeViewItemModel<string>>> _CollectionView = new Property<CollectionView<TreeViewItemModel<string>>> { LocatorFunc = _CollectionViewLocator };
        static Func<BindableBase, ValueContainer<CollectionView<TreeViewItemModel<string>>>> _CollectionViewLocator = RegisterContainerLocator<CollectionView<TreeViewItemModel<string>>>("CollectionView", model => model.Initialize("CollectionView", ref model._CollectionView, ref _CollectionViewLocator, _CollectionViewDefaultValueFactory));
        static Func<CollectionView<TreeViewItemModel<string>>> _CollectionViewDefaultValueFactory = null;
        #endregion

        //public ICollectionView CollectionView
        //{
        //    get { return _CollectionViewLocator(this).Value; }
        //    set { _CollectionViewLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property ICollectionView CollectionView Setup
        //protected Property<ICollectionView> _CollectionView = new Property<ICollectionView> { LocatorFunc = _CollectionViewLocator };
        //static Func<BindableBase, ValueContainer<ICollectionView>> _CollectionViewLocator = RegisterContainerLocator<ICollectionView>("CollectionView", model => model.Initialize("CollectionView", ref model._CollectionView, ref _CollectionViewLocator, _CollectionViewDefaultValueFactory));
        //static Func<ICollectionView> _CollectionViewDefaultValueFactory = null;
        //#endregion


        public CommandModel<ReactiveCommand, String> CommandShowingNormal
        {
            get { return _CommandShowingNormalLocator(this).Value; }
            set { _CommandShowingNormalLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowingNormal Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowingNormal = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowingNormalLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowingNormalLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandShowingNormal", model => model.Initialize("CommandShowingNormal", ref model._CommandShowingNormal, ref _CommandShowingNormalLocator, _CommandShowingNormalDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowingNormalDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                      vm.InitData(CollectionViewType.Normal);
                  })
                  .DisposeWith(vm);
                return cmd.CreateCommandModel("ShowingNormal");
            };
        #endregion




        public CommandModel<ReactiveCommand, String> CommandShowningGroup
        {
            get { return _CommandShowningGroupLocator(this).Value; }
            set { _CommandShowningGroupLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowningGroup Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowningGroup = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowningGroupLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowningGroupLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandShowningGroup", model => model.Initialize("CommandShowningGroup", ref model._CommandShowningGroup, ref _CommandShowningGroupLocator, _CommandShowningGroupDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowningGroupDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                      vm.InitData(CollectionViewType.Grouped);
                  })
                  .DisposeWith(vm);
                return cmd.CreateCommandModel("ShowningGroup");
            };
        #endregion


        public CommandModel<ReactiveCommand, String> CommandShowingLoading
        {
            get { return _CommandShowingLoadingLocator(this).Value; }
            set { _CommandShowingLoadingLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowingLoading Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowingLoading = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowingLoadingLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowingLoadingLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandShowingLoading", model => model.Initialize("CommandShowingLoading", ref model._CommandShowingLoading, ref _CommandShowingLoadingLocator, _CommandShowingLoadingDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowingLoadingDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                      vm.InitData(CollectionViewType.Loading);
                  })
                  .DisposeWith(vm);
                return cmd.CreateCommandModel("ShowingLoading");
            };
        #endregion

        
        
        public CommandModel<ReactiveCommand, String> CommandTestHit
        {
            get { return _CommandTestHitLocator(this).Value; }
            set { _CommandTestHitLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandTestHit Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandTestHit = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandTestHitLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandTestHitLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandTestHit", model => model.Initialize("CommandTestHit", ref model._CommandTestHit, ref _CommandTestHitLocator, _CommandTestHitDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandTestHitDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you wantS
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe (
                  async _=>
                  {
                      vm.Title = ((TreeViewItemModel<string>)_.EventArgs.Parameter).Value;
                  } )
                  .DisposeWith(vm); 
                return cmd.CreateCommandModel("TestHit");
            };
        #endregion

        


    }

}

