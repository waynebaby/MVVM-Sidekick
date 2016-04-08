#if NETFX_CORE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;

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
            ((INotifyPropertyChanged)_coreCollection).PropertyChanged += DependencyObservableVector_PropertyChanged;

        }

        private void DependencyObservableVector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Count = _coreCollection.Count;
        }

        private void _coreCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (VectorChanged != null)
            {

                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        {
                            var changeType = CollectionChange.ItemInserted;
                            RaiseNewItemsChangedEvents(e, changeType);
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        {
                            var changeType = CollectionChange.ItemChanged;
                            RaiseNewItemsChangedEvents(e, changeType);
                            RaiseOldItemsChangedEvents(e, changeType);
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        {
                            var changeType = CollectionChange.ItemRemoved;
                            RaiseOldItemsChangedEvents(e, changeType);
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        {
                            var changeType = CollectionChange.ItemChanged;
                            RaiseNewItemsChangedEvents(e, changeType);
                            RaiseOldItemsChangedEvents(e, changeType);
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        {
                            var changeType = CollectionChange.ItemChanged;
                            VectorChanged(this, new VectorChangedEventArgs { CollectionChange = changeType });
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void RaiseOldItemsChangedEvents(System.Collections.Specialized.NotifyCollectionChangedEventArgs e, CollectionChange changeType)
        {
            for (uint i = (uint)e.OldStartingIndex; i < e.OldStartingIndex + e.OldItems.Count; i++)
            {

                var v = new VectorChangedEventArgs()
                {
                    CollectionChange = changeType,
                    Index = i,
                };
                VectorChanged(this, v);
            }
        }

        private void RaiseNewItemsChangedEvents(System.Collections.Specialized.NotifyCollectionChangedEventArgs e, CollectionChange changeType)
        {
            for (uint i = (uint)e.NewStartingIndex; i < e.NewStartingIndex + e.NewItems.Count; i++)
            {
                var v = new VectorChangedEventArgs()
                {
                    CollectionChange = changeType,
                    Index = i,
                };
                VectorChanged(this, v);
            }
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
            private set { SetValue(CountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register(nameof(Count), typeof(int), typeof(Subclass), new PropertyMetadata(0));

        public bool IsReadOnly
        {
            get { return false; }
        }

      

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


    public class DependencyObservableVector : DependencyObservableVector<object, DependencyObservableVector>
    {

    }



    /// <summary>
    /// Provides data for the changed events of a vector
    ///</summary>
    public class VectorChangedEventArgs : Windows.Foundation.Collections.IVectorChangedEventArgs
    {
        #region IVectorChangedEventArgs Members
        /// <summary>
        /// Describes the change that caused the change
        /// </summary>
        public Windows.Foundation.Collections.CollectionChange CollectionChange
        {
            get;
            set;
        }
        /// <summary>
        /// The index of the item changed
        /// </summary>
        public uint Index
        {
            get;
            set;
        }
        #endregion
    }
}



#endif