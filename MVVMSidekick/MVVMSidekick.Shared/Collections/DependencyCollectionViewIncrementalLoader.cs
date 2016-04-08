
#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MVVMSidekick.Collections
{
    public class DependencyCollectionViewIncrementalLoader : DependencyObject, ISupportIncrementalLoading
    {


        public bool HasMoreItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            throw new NotImplementedException();
        }
    }
}

#endif