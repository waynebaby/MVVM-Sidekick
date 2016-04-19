#if NETFX_CORE

using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MVVMSidekick.Collections
{
    public interface IDependencyCollectionViewGroupingManager
    {
        bool TryAddItemToGroup(object item);
        bool TryRemoveItemFromGroup(object item);
    }

    public abstract class DependencyCollectionViewGroupingManagerBase : DependencyObject, IDependencyCollectionViewGroupingManager
    {


      
        public DependencyCollectionViewGroupingManagerBase(DependencyCollectionView collectionView)
        {
            CollectionView = collectionView;

            var binding = new Binding
            {
                Path = new PropertyPath( nameof (collectionView.IncrementalLoader)),
                Source= CollectionView
            };

            BindingOperations.SetBinding(this, IncrementalLoaderProperty, binding);
        }

        public DependencyCollectionView CollectionView
        {
            get { return (DependencyCollectionView)GetValue(CollectionViewProperty); }
            protected set { SetValue(CollectionViewProperty, value); }
        }

        public static readonly DependencyProperty CollectionViewProperty =
            DependencyProperty.Register(nameof(CollectionView), typeof(DependencyCollectionView), typeof(DependencyCollectionViewGroupingManagerBase), new PropertyMetadata(0));


        protected DependencyCollectionViewIncrementalLoaderBase IncrementalLoader
        {
            get { return (DependencyCollectionViewIncrementalLoaderBase)GetValue(IncrementalLoaderProperty); }
            set { SetValue(IncrementalLoaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IncrementalLoader.  This enables animation, styling, binding, etc...
        protected static readonly DependencyProperty IncrementalLoaderProperty =
            DependencyProperty.Register(nameof(IncrementalLoader), typeof(DependencyCollectionViewIncrementalLoaderBase), typeof(DependencyCollectionViewGroupingManagerBase), new PropertyMetadata(null));




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


        public int GroupCount
        {
            get { return (int)GetValue(GroupCountProperty); }
            set { SetValue(GroupCountProperty, value); }
        }

        public static readonly DependencyProperty GroupCountProperty =
            DependencyProperty.Register(
                nameof(GroupCount),
                typeof(int),
                typeof(DependencyCollectionViewGroupingManagerBase),
                new PropertyMetadata(3,
                    (o, e) =>
                    {
                        var gm = o as DependencyCollectionViewGroupingManagerBase;
                        var count = (int)e.NewValue;
                        var view = gm.CollectionView;
                        InitCollectionViewGroup(view, count);
                    }
                ));

        private static void InitCollectionViewGroup(DependencyCollectionView view, int count)
        {
            if (view.CollectionGroups?.Count != count)
            {
                view.CollectionGroups.Clear();
                for (int i = 0; i < count; i++)
                {
                    var g = new DependencyCollectionViewGroup(i, view);
               
                    view.CollectionGroups.Add(g);
                }
            }
        }
    }
}
#endif