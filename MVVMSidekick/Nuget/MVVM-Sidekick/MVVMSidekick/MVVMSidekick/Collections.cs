using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;
using Windows.Foundation.Collections;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif


namespace MVVMSidekick
{



    namespace Collections
    {



        public class DependencyObservableCollection<T> : DependencyObject , ICollection<T>, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
        {




            protected ObservableCollection<T> _core = new ObservableCollection<T>();
            public DependencyObservableCollection()
            {

                var countBinding = new Binding();
                countBinding.Path = new PropertyPath("Count");
                countBinding.Mode = BindingMode.OneWay;
                countBinding.Source = _core;
                BindingOperations.SetBinding(this, CountProperty, countBinding);

            }




            public void Add(T item)
            {
                _core.Add(item);
            }

            public void Clear()
            {
                _core.Clear();
            }

            public bool Contains(T item)
            {
                return _core.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _core.CopyTo(array, arrayIndex);
            }





            public virtual int Count
            {
                get { return (int)GetValue(CountProperty); }
                protected set { SetValue(CountProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty CountProperty =
                DependencyProperty.Register("Count", typeof(int), typeof(DependencyObservableCollection<T>), new PropertyMetadata(0));




            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(T item)
            {
                return _core.Remove(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _core.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _core.GetEnumerator();
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add { _core.CollectionChanged += value; }
                remove { _core.CollectionChanged -= value; }
            }

            public event PropertyChangedEventHandler PropertyChanged
            {
                add { ((INotifyPropertyChanged)_core).PropertyChanged += value; }
                remove { ((INotifyPropertyChanged)_core).PropertyChanged -= value; }
            }


            public int IndexOf(T item)
            {
                return _core.IndexOf(item);
            }

            public void Insert(int index, T item)
            {
                _core.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                _core.RemoveAt(index);
            }

            public T this[int index]
            {
                get
                {
                    return _core[index];
                }
                set
                {
                    _core[index] = value;
                }
            }
        }



#if NETFX_CORE

        //public 
        /// <summary>
        /// ObservableVector that raises events for IObservableCollection and IObservableVector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ObservableVector<T> : ObservableCollection<T>, Windows.Foundation.Collections.IObservableVector<object>
        {



            //// *** Constants ***

            //private const string PropertyNameCount = "Count";
            //private const string PropertyNameIndexer = "Item[]";

            //// *** Events ***

            //public event PropertyChangedEventHandler PropertyChanged;
            //public event NotifyCollectionChangedEventHandler CollectionChanged;

            // *** Constructors ***

            public ObservableVector()
                : base()
            {
            }

            public ObservableVector(IList<T> list)
                : base(list)
            {
            }

            // *** Protected Methods ***   

            protected override void ClearItems()
            {

                base.ClearItems();
                //OnPropertyChanged(PropertyNameCount);
                //OnPropertyChanged(PropertyNameIndexer);
                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                if (VectorChanged != null)
                {
                    VectorChanged(this, new VectorChangedEventArgs { CollectionChange = Windows.Foundation.Collections.CollectionChange.Reset });
                }
            }

            protected override void InsertItem(int index, T item)
            {
                base.InsertItem(index, item);
                //OnPropertyChanged(PropertyNameCount);
                //OnPropertyChanged(PropertyNameIndexer);
                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                if (VectorChanged != null)
                {
                    VectorChanged(this, new VectorChangedEventArgs { CollectionChange = Windows.Foundation.Collections.CollectionChange.ItemInserted, Index = (uint)index });
                }
            }

            protected override void RemoveItem(int index)
            {
                T oldItem = base[index];
                base.RemoveItem(index);
                //OnPropertyChanged(PropertyNameCount);
                //OnPropertyChanged(PropertyNameIndexer);
                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
                if (VectorChanged != null)
                {
                    VectorChanged(this, new VectorChangedEventArgs { CollectionChange = Windows.Foundation.Collections.CollectionChange.ItemRemoved, Index = (uint)index });
                }
            }

            protected override void SetItem(int index, T item)
            {
                T oldItem = base[index];
                base.SetItem(index, item);
                //OnPropertyChanged(PropertyNameIndexer);
                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
                if (VectorChanged != null)
                {
                    VectorChanged(this, new VectorChangedEventArgs { CollectionChange = Windows.Foundation.Collections.CollectionChange.ItemChanged, Index = (uint)index });
                }
            }

            protected void OnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            // *** Event Handlers ***

            //protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            //{
            //    if (PropertyChanged != null)
            //        PropertyChanged(this, e);
            //}

            //protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            //{
            //    if (CollectionChanged != null)
            //        CollectionChanged(this, e);
            //}

            public event Windows.Foundation.Collections.VectorChangedEventHandler<object> VectorChanged;



            #region IList<object> Members

            int IList<object>.IndexOf(object item)
            {
                return IndexOf((T)item);
            }

            void IList<object>.Insert(int index, object item)
            {
                Insert(index, (T)item);
            }



            object IList<object>.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (T)value;
                }
            }

            #endregion

            #region ICollection<object> Members

            void ICollection<object>.Add(object item)
            {
                Add((T)item);
            }


            bool ICollection<object>.Contains(object item)
            {
                return Contains((T)item);
            }

            void ICollection<object>.CopyTo(object[] array, int arrayIndex)
            {

                for (int i = 0; i < Count; i++)
                {
                    var idx = arrayIndex + i;
                    array[idx] = this[i];

                }
            }



            bool ICollection<object>.Remove(object item)
            {
                return Remove((T)item);
            }

            #endregion

            #region IEnumerable<object> Members

            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                return this.OfType<Object>().GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion

            #region ICollection<object> Members


            bool ICollection<Object>.IsReadOnly
            {
                get { return false; }
            }

            #endregion
        }




        /// <summary>
        /// Provides data for the changed events of a vector
        ///// </summary>
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

#endif
        public static class CollectionExtensions
        {
            public static KeyedObserableCollection<K, V> ToKeyedObserableCollection<K, V>(this IDictionary<K, V> items)
            {
                return new KeyedObserableCollection<K, V>(items);

            }


        }

        public class KeyedObserableCollection<K, V> : ObservableCollection<KeyValuePair<K, V>>
        {

            public KeyedObserableCollection(IDictionary<K, V> items)
            {
                if (items == null)
                {
                    throw new ArgumentException("items could not be null.");
                }
                var bak = items.ToList();
                _coreDictionary = items;
                items.Clear();
                foreach (var item in bak)
                {
                    base.Add(item);
                }
            }



            IDictionary<K, V> _coreDictionary;
            int _coreVersion;
            int _shadowVersion;
            private void IncVer()
            {
                _coreVersion++;
                if (_coreVersion >= 1024 * 1024 * 1024)
                {
                    _coreVersion = 0;
                }
            }



            protected override void ClearItems()
            {
                base.ClearItems();
                _coreDictionary.Clear();
                IncVer();
            }


            protected override void InsertItem(int index, KeyValuePair<K, V> item)
            {
                _coreDictionary.Add(item.Key, item.Value);
                base.InsertItem(index, item);
                IncVer();
            }

            protected override void SetItem(int index, KeyValuePair<K, V> item)
            {

                _coreDictionary.Add(item.Key, item.Value);
                RemoveFromDic(index);

                base.SetItem(index, item);
                IncVer();
            }

            private void RemoveFromDic(int index)
            {
                var rem = base[index];
                if (rem.Key != null)
                {
                    _coreDictionary.Remove(rem.Key);
                }
                IncVer();
            }

            protected override void RemoveItem(int index)
            {
                RemoveFromDic(index);
                base.RemoveItem(index);
                IncVer();
            }


#if SILVERLIGHT_5||NET40||WINDOWS_PHONE_7
            Dictionary<K, V> _shadowDictionary;
            public IDictionary<K, V> Items
            {
                get
                {
                    if (_shadowDictionary == null || _shadowVersion != _coreVersion)
                    {
                        _shadowDictionary = new Dictionary<K, V>(_coreDictionary);
                        _shadowVersion = _coreVersion;
                    }
                    return _shadowDictionary;

                }
            }

#else
            ReadOnlyDictionary<K, V> _shadowDictionary;
            public IDictionary<K, V> Items
            {
                get
                {
                    if (_shadowDictionary == null || _shadowVersion != _coreVersion)
                    {
                        _shadowDictionary = new ReadOnlyDictionary<K, V>(_coreDictionary);
                        _shadowVersion = _coreVersion;
                    }
                    return _shadowDictionary;

                }
            }


#endif



        }



#if NETFX_CORE

        namespace CollectionView
        {





            public class CollectionViewIncrementalLoader<T> : ISupportIncrementalLoading
            {
                private Func<CollectionView<T>, int, Task<IncrementalLoadResult<T>>> _loadMore;
                private Func<CollectionView<T>, bool> _hasMore;
                bool _hasNoMore = false;
                public CollectionViewIncrementalLoader(Func<CollectionView<T>, int, Task<IncrementalLoadResult<T>>> loadMore = null, Func<CollectionView<T>, bool> hasMore = null)
                {
                    var canlm = (loadMore != null);
                    var canhm = (hasMore != null);

                    if (canlm && canhm)
                    {

                        _loadMore = loadMore;
                        _hasMore = hasMore;
                    }
                    else
                    {
                        throw new InvalidOperationException("need both loadMore and hasMore have value ");
                    }
                }

                public CollectionView<T> CollectionView { get; set; }

                #region ISupportIncrementalLoading Members



                public bool HasMoreItems
                {
                    get
                    {
                        if (!_hasNoMore)
                        {
                            _hasNoMore = !_hasMore(CollectionView);
                        }
                        return !_hasNoMore;

                    }
                }

                async Task<LoadMoreItemsResult> InternalLoadMoreItemsAsync(uint count)
                {
                    var rval = await _loadMore(CollectionView, (int)count);

                    _hasNoMore = !rval.HaveMore;

                    foreach (var x in rval.NewItems)
                    {
                        CollectionView.Add(x);

                    }
                    return new LoadMoreItemsResult { Count = count };
                }

                #endregion

                #region ISupportIncrementalLoading Members


                public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
                {
                    return InternalLoadMoreItemsAsync(count).AsAsyncOperation();
                }

                #endregion
            }

            public struct IncrementalLoadResult<T>
            {
                public IList<T> NewItems { get; set; }
                public bool HaveMore { get; set; }
            }

            public class CollectionViewGroupCollectionItem : ICollectionViewGroup
            {

                public static CollectionViewGroupCollectionItem Create(object group, IObservableVector<object> items)
                {
                    return new CollectionViewGroupCollectionItem(group, items);
                }

                public CollectionViewGroupCollectionItem(object group, IObservableVector<object> items)
                {
                    Group = group;
                    GroupItems = items;
                }

                public object Group { get; private set; }
                public IObservableVector<object> GroupItems { get; private set; }



            }

            public abstract class CollectionViewGroupCollection<TItem> : ObservableVector<CollectionViewGroupCollectionItem>
            {
                public static CollectionViewGroupCollection<TItem, TGroupKey, TGroup> Create<TGroupKey, TGroup>(Func<TItem, TGroupKey> groupKeyGetter, Func<TItem, TGroup> groupFactory, Dictionary<TGroupKey, CollectionViewGroupCollectionItem> index = null)
                {
                    return new CollectionViewGroupCollection<TItem, TGroupKey, TGroup>(groupKeyGetter, groupFactory, index);

                }

                public abstract void AddItemToGroups(object item);


                public abstract void RemoveItemFromGroups(object item);
            }
            public class CollectionViewGroupCollection<TItem, TGroupKey, TGroup> : CollectionViewGroupCollection<TItem>
            {
                public CollectionViewGroupCollection(Func<TItem, TGroupKey> groupKeyGetter, Func<TItem, TGroup> groupFactory, Dictionary<TGroupKey, CollectionViewGroupCollectionItem> index = null)
                {
                    _groupKeyGetter = groupKeyGetter;
                    _groupFactory = groupFactory;

                    _index = index ?? new Dictionary<TGroupKey, CollectionViewGroupCollectionItem>();
                }
                Func<TItem, TGroupKey> _groupKeyGetter;

                Dictionary<TGroupKey, CollectionViewGroupCollectionItem> _index;
                private Func<TItem, TGroup> _groupFactory;


                public override void AddItemToGroups(object item)
                {
                    var itm = (TItem)item;
                    var key = _groupKeyGetter(itm);
                    CollectionViewGroupCollectionItem grp;
                    if (!_index.TryGetValue(key, out grp))
                    {
                        grp = CollectionViewGroupCollectionItem.Create(_groupFactory(itm), new ObservableVector<TItem>());
                        _index.Add(key, grp);
                        Add(grp);
                    }

                    grp.GroupItems.Add(item);

                }

                public override void RemoveItemFromGroups(object item)
                {
                    var key = _groupKeyGetter((TItem)item);
                    CollectionViewGroupCollectionItem grp;
                    if (_index.TryGetValue(key, out grp))
                    {
                        grp.GroupItems.Remove(item);
                        if (grp.GroupItems.Count == 0)
                        {
                            _index.Remove(key);
                            Remove(grp);
                        }
                    }

                }

            }


            public class CollectionView<T> : ObservableVector<T>, ICollectionView
            {
                public CollectionView(
                            IEnumerable<T> items = null,
                            CollectionViewIncrementalLoader<T> loader = null)
                    : base(items.ToArray())
                {

                    items = items ?? new T[0];

                    _loader = loader;
                    if (loader != null)
                    {
                        loader.CollectionView = this;
                    }

                }


                public CollectionView(
                    IEnumerable<T> items,
                CollectionViewGroupCollection<T> groupCollection)
                    : base(items.ToArray())
                {

                    _group = groupCollection;


                    if (_group != null && items != null)
                    {
                        foreach (var item in items)
                        {
                            _group.AddItemToGroups(item);
                        }
                    }
                }

                protected override void InsertItem(int index, T item)
                {
                    base.InsertItem(index, item);
                    if (_group != null)
                    {
                        _group.AddItemToGroups(item);
                    }
                }

                protected override void SetItem(int index, T item)
                {
                    var oldItem = base.Items[index];
                    if (_group != null)
                    {
                        _group.RemoveItemFromGroups(item);
                    }
                    base.SetItem(index, item);
                    if (_group != null)
                    {
                        _group.AddItemToGroups(item);
                    }
                }

                protected override void ClearItems()
                {
                    base.ClearItems();
                    if (_group != null)
                    {
                        _group.Clear();
                    }
                }

                protected override void RemoveItem(int index)
                {
                    var olditem = base.Items[index];
                    base.RemoveItem(index);

                    if (_group != null)
                    {
                        _group.RemoveItemFromGroups(olditem);
                    }
                }


                protected CollectionViewIncrementalLoader<T> _loader;

                Windows.Foundation.Collections.IObservableVector<object> ThisVector { get { return this; } }

                #region ICollectionView Members

                private CollectionViewGroupCollection<T> _group;
                public Windows.Foundation.Collections.IObservableVector<object> CollectionGroups
                {
                    get { return _group; }
                }

                public event EventHandler<object> CurrentChanged;

                public event CurrentChangingEventHandler CurrentChanging;

                public object CurrentItem
                {
                    get
                    {
                        if (Count > _CurrentPosition && _CurrentPosition >= 0)
                        {
                            return Items[_CurrentPosition];
                        }
                        return null;
                    }
                }

                int _CurrentPosition = 0;
                public int CurrentPosition
                {
                    get { return _CurrentPosition; }
                    set
                    {
                        _CurrentPosition = value;
                        base.OnPropertyChanged("CurrentPosition");
                        base.OnPropertyChanged("CurrentItem");
                    }
                }



                public bool HasMoreItems
                {
                    get
                    {
                        if (_loader == null)
                        {
                            return false;
                        }
                        else
                        {
                            return _loader.HasMoreItems;
                        }
                    }
                }

                public bool IsCurrentAfterLast
                {
                    get { return _CurrentPosition >= this.Count; }
                }

                public bool IsCurrentBeforeFirst
                {
                    get { return _CurrentPosition < 0; }
                }

                public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
                {
                    if (_loader != null)
                    {
                        return _loader.LoadMoreItemsAsync(count);
                    }
                    else
                    {
                        throw new InvalidOperationException("this instance does not support load More Items");
                    }
                }

                bool RaiseCurrentChangingAndReturnCanceled(object newValue)
                {
                    if (CurrentChanging != null)
                    {
                        var e = new CurrentChangingEventArgs(true);

                        CurrentChanging(this, e);
                        return e.Cancel;
                    }
                    return false;
                }
                void RaiseCurrentChanged(object newValue)
                {
                    if (CurrentChanged != null)
                    {
                        CurrentChanged(this, newValue);
                    }
                    base.OnPropertyChanged("CurrentItem");
                }


                public bool MoveCurrentToFirst()
                {
                    var newIndex = 0;
                    return MoveCurrentToPosition(newIndex);

                }



                public bool MoveCurrentToLast()
                {
                    var newIndex = Count - 1;
                    return MoveCurrentToPosition(newIndex);
                }

                public bool MoveCurrentToNext()
                {
                    var newIndex = CurrentPosition + 1;
                    return MoveCurrentToPosition(newIndex);
                }

                public bool MoveCurrentToPosition(int index)
                {

                    if (Count > 0 && index >= 0 && index < Count)
                    {

                        var newVal = Items[index];
                        if (RaiseCurrentChangingAndReturnCanceled(newVal))
                        {
                            CurrentPosition = index;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                public bool MoveCurrentToPrevious()
                {
                    var newIndex = CurrentPosition - 1;
                    return MoveCurrentToPosition(newIndex);
                }



                public bool MoveCurrentTo(object item)
                {
                    var index = IndexOf((T)item);
                    return MoveCurrentToPosition(index);
                }

                #endregion

            }

            //public class GroupedCollectionView<TGroup, TItem> : ObservableVector<TItem>
            //{
            //    public GroupedCollectionView(IEnumerable<TGroup> groups, Func<IEnumerable<TGroup>, IEnumerable<TItem>> itemSelector) :
            //        base(itemSelector(groups).ToArray())
            //    {

            //    }
            //}
        }

#endif

    }

}
