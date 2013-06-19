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




    }

    public class LV1 : TreeViewItemModel<string>
    { 
    
    }


    public class LV2: TreeViewItemModel<string>
    {

    }




}
