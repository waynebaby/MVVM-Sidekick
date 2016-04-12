#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MVVMSidekick.Collections
{
    public abstract class DependencyCollectionViewGroupBase : DependencyObject, ICollectionViewGroup
    {

        public DependencyCollectionViewGroupBase()
        {
            Group = new object();
            GroupItems = new DependencyObservableVector();
        }

        public object Group
        {
            get { return (object)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }



        public static readonly DependencyProperty GroupProperty =
            DependencyProperty.Register(nameof(Group), typeof(object), typeof(DependencyCollectionViewGroupBase), new PropertyMetadata(null));


        public IObservableVector<object> GroupItems
        {
            get { return (IObservableVector<object>)GetValue(GroupItemsProperty); }
            set { SetValue(GroupItemsProperty, value); }
        }

        public static readonly DependencyProperty GroupItemsProperty =
            DependencyProperty.Register(nameof(GroupItems), typeof(IObservableVector<object>), typeof(DependencyCollectionViewGroupBase), new PropertyMetadata(null));


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


    public class DependencyDelegateCollectionViewGroup : DependencyCollectionViewGroupBase
    {
        public DependencyDelegateCollectionViewGroup(Func<ICollectionViewGroup, object, bool> tryAddItemToGroup, Func<ICollectionViewGroup, object, bool> tryRemoveItemFromGroup)
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