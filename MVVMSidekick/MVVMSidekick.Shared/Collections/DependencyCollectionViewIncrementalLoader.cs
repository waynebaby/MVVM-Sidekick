
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

        public DependencyCollectionViewIncrementalLoaderBase(DependencyCollectionView target)
        {
            Target = target;
        }

        internal protected DependencyCollectionView Target { get; set; }

        public bool HasMoreItems
        {
            get
            {
                return OnCheckIfHasMoreItems();
            }
        }


        protected abstract bool OnCheckIfHasMoreItems();

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return OnLoadMoreItemsAsync(count);
        }

        protected abstract IAsyncOperation<LoadMoreItemsResult> OnLoadMoreItemsAsync(uint count);

    }


    public class DependencyCollectionViewDelegateIncrementalLoader : DependencyCollectionViewIncrementalLoaderBase
    {
        public DependencyCollectionViewDelegateIncrementalLoader(DependencyCollectionView target,
            Func<DependencyCollectionView, bool> hasMoreItems,
            Func<DependencyCollectionView, int, Task<LoadMoreItemsResult>> loadMoreItemsAsync
            ) : base(target)
        {
            _OnCheckIfHasMoreItems = hasMoreItems;
            _OnLoadMoreItemsAsync = loadMoreItemsAsync;
        }


        Func<DependencyCollectionView, bool> _OnCheckIfHasMoreItems;
        Func<DependencyCollectionView, int, Task<LoadMoreItemsResult>> _OnLoadMoreItemsAsync;
        protected override bool OnCheckIfHasMoreItems()
        {
            return _OnCheckIfHasMoreItems?.Invoke(Target) ?? false;
        }

        protected override IAsyncOperation<LoadMoreItemsResult> OnLoadMoreItemsAsync(uint count)
        {
            return _OnLoadMoreItemsAsync?.Invoke(Target, (int)count)?.AsAsyncOperation();
        }
    }


}

#endif