
#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MVVMSidekick.Collections
{
    public abstract class DependencyCollectionViewIncrementalLoaderBase : DependencyObject, ISupportIncrementalLoading
    {

        public DependencyCollectionViewIncrementalLoaderBase()
        {
            //Target = target;
        }


        internal protected DependencyCollectionView Target { get; set; }

        public bool HasMoreItems
        {
            get { return (bool)GetValue(HasMoreItemsProperty); }
            set { SetValue(HasMoreItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasMoreItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasMoreItemsProperty =
            DependencyProperty.Register("HasMoreItems", typeof(bool), typeof(DependencyCollectionViewIncrementalLoaderBase), new PropertyMetadata(false));




        protected abstract bool OnCheckIfHasMoreItems();

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var v = OnLoadMoreItemsAsync(count);
            HasMoreItems = OnCheckIfHasMoreItems();
            return v;
        }

        protected abstract IAsyncOperation<LoadMoreItemsResult> OnLoadMoreItemsAsync(uint count);

    }



    public class DependencyCollectionViewProxyIncrementalLoader : DependencyCollectionViewIncrementalLoaderBase
    {
        public DependencyCollectionViewProxyIncrementalLoader()
        {


            base.RegisterPropertyChangedCallback(InnerLoaderProperty,
                (o, e) =>
                {
                    var b = new Binding()
                    {
                        Path = new PropertyPath(nameof(HasMoreItems)),
                        Source = InnerLoader,
                    };

                    BindingOperations.SetBinding(this, HasMoreItemsProperty, b);
                });

        }


        public DependencyCollectionViewIncrementalLoaderBase InnerLoader
        {
            get { return (DependencyCollectionViewIncrementalLoaderBase)GetValue(InnerLoaderProperty); }
            set { SetValue(InnerLoaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InnerLoader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerLoaderProperty =
            DependencyProperty.Register("InnerLoader", typeof(DependencyCollectionViewIncrementalLoaderBase), typeof(DependencyCollectionViewProxyIncrementalLoader), new PropertyMetadata(null));


        sealed protected override bool OnCheckIfHasMoreItems()
        {
            return InnerLoader?.HasMoreItems ?? false;
        }

        protected override IAsyncOperation<LoadMoreItemsResult> OnLoadMoreItemsAsync(uint count)
        {
            return InnerLoader?.LoadMoreItemsAsync(count);
        }
    }


    public class DependencyCollectionViewDelegateIncrementalLoader : DependencyCollectionViewIncrementalLoaderBase
    {
        public DependencyCollectionViewDelegateIncrementalLoader(
            Func<DependencyCollectionView, DependencyCollectionViewDelegateIncrementalLoader, bool> hasMoreItems,
            Func<DependencyCollectionView, DependencyCollectionViewDelegateIncrementalLoader, int, Task<LoadMoreItemsResult>> loadMoreItemsAsync
            )
        {
            _OnCheckIfHasMoreItems = hasMoreItems;
            _OnLoadMoreItemsAsync = loadMoreItemsAsync;
        }


        Func<DependencyCollectionView, DependencyCollectionViewDelegateIncrementalLoader, bool> _OnCheckIfHasMoreItems;
        Func<DependencyCollectionView, DependencyCollectionViewDelegateIncrementalLoader, int, Task<LoadMoreItemsResult>> _OnLoadMoreItemsAsync;
        protected override bool OnCheckIfHasMoreItems()
        {
            return _OnCheckIfHasMoreItems?.Invoke(Target, this) ?? false;
        }

        protected override IAsyncOperation<LoadMoreItemsResult> OnLoadMoreItemsAsync(uint count)
        {
            return _OnLoadMoreItemsAsync?.Invoke(Target, this, (int)count)?.AsAsyncOperation();
        }
    }


}

#endif