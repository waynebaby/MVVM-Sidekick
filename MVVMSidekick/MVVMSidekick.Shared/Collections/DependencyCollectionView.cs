#if NETFX_CORE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace MVVMSidekick.Collections
{
    public abstract class DependencyObservableVector<TValue, Subclass> : DependencyObject, IObservableVector<TValue>
        where Subclass : DependencyObservableVector<TValue, Subclass>
    {
        protected ObservableCollection<TValue> _coreCollection;
        public DependencyObservableVector()
        {
            _coreCollection = new ObservableCollection<TValue>();
            _coreCollection.CollectionChanged += _coreCollection_CollectionChanged;

        }

        private void _coreCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        public TValue this[int index]
        {
            get
            {
                return _coreCollection[index];
            }
            set
            {
                _coreCollection[index] = value;
            }
        }


        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            //set { SetValue(CountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register(nameof(Count), typeof(int), typeof(Subclass), new PropertyMetadata(0));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            //set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(Subclass), new PropertyMetadata(false));


        public event VectorChangedEventHandler<TValue> VectorChanged;

        public void Add(TValue item)
        {
            _coreCollection.Add(item);
        }

        public void Clear()
        {
            _coreCollection.Clear();
        }

        public bool Contains(TValue item)
        {
            return _coreCollection.Contains(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            _coreCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _coreCollection.GetEnumerator();
        }

        public int IndexOf(TValue item)
        {
            return _coreCollection.IndexOf(item);
        }

        public void Insert(int index, TValue item)
        {
            _coreCollection.Insert(index, item);
        }

        public bool Remove(TValue item)
        {
            return _coreCollection.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _coreCollection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _coreCollection.GetEnumerator();
        }
    }

    public class DependencyCollectionView : DependencyObservableVector<Object, DependencyCollectionView>, ICollectionView
    {
        public IObservableVector<object> CollectionGroups
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object CurrentItem
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CurrentPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasMoreItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsCurrentAfterLast
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsCurrentBeforeFirst
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<object> CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentTo(object item)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToFirst()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToLast()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToNext()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToPosition(int index)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToPrevious()
        {
            throw new NotImplementedException();
        }
    }
}

#endif

