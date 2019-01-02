#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MVVMSidekick.Collections
{

    public class DependencyCollectionViewGroup : DependencyObject, ICollectionViewGroup
    {
        public DependencyCollectionViewGroup(object groupObject, DependencyCollectionView parentView)
        {
            Group = groupObject;
            var groupItems = new DependencyCollectionView();
            GroupItems = groupItems;
            ParentView = parentView;
            var loader = new DependencyCollectionViewProxyIncrementalLoader()
            {
                InnerLoader = parentView.IncrementalLoader
            };
            
            groupItems.IncrementalLoader = loader;



        }
        public DependencyCollectionView ParentView { get; set; }

        public object Group
        {
            get { return (object)GetValue(GroupProperty); }
            protected set { SetValue(GroupProperty, value); }
        }
        
        public static readonly DependencyProperty GroupProperty =
            DependencyProperty.Register(nameof(Group), typeof(object), typeof(DependencyCollectionViewGroup), new PropertyMetadata(null));


        public IObservableVector<object> GroupItems
        {
            get { return (IObservableVector<object>)GetValue(GroupItemsProperty); }
            set { SetValue(GroupItemsProperty, value); }
        }


        public static readonly DependencyProperty GroupItemsProperty =
            DependencyProperty.Register(nameof(GroupItems), typeof(IObservableVector<object>), typeof(DependencyCollectionViewGroup), new PropertyMetadata(null));

    }


    public abstract class SelfServiceDependencyCollectionViewGroupBase : DependencyCollectionViewGroup, IDependencyCollectionViewGroupingManager
    {

        public SelfServiceDependencyCollectionViewGroupBase(object groupObject, DependencyCollectionView parentView)
            : base(groupObject, parentView)
        {

        }

        public bool TryAddItemToGroup(object item)
        {
            return OnAddingItemToGroup(item);
        }

        public bool TryRemoveItemFromGroup(object item)
        {
            return OnRemovingingItemFromGroup(item);
        }

        protected abstract bool OnAddingItemToGroup(object item);
        protected abstract bool OnRemovingingItemFromGroup(object item);


    }



    public class SelfServiceDependencyDelegateCollectionViewGroup : SelfServiceDependencyCollectionViewGroupBase
    {

        public SelfServiceDependencyDelegateCollectionViewGroup(object groupObject, DependencyCollectionView parentView, Func<ICollectionViewGroup, object, bool> tryAddItemToGroup, Func<ICollectionViewGroup, object, bool> tryRemoveItemFromGroup)
      : base(groupObject, parentView)
        {
            _OnAddingItemToGroup = tryAddItemToGroup;
            _OnRemovingingItemRemoveGroup = tryRemoveItemFromGroup;
        }

        Func<ICollectionViewGroup, object, bool> _OnAddingItemToGroup;
        protected override bool OnAddingItemToGroup(object item)
        {
            return _OnAddingItemToGroup?.Invoke(this, item) ?? false;
        }

        Func<ICollectionViewGroup, object, bool> _OnRemovingingItemRemoveGroup;

        protected override bool OnRemovingingItemFromGroup(object item)
        {
            return _OnRemovingingItemRemoveGroup?.Invoke(this, item) ?? false;
        }
    }
}
#endif