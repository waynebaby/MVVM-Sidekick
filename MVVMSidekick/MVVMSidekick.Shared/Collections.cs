// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Collections.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.Dynamic;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
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

#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
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



        /// <summary>
        /// Class DependencyObservableCollection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class DependencyObservableCollection<T> : DependencyObject, ICollection<T>, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
        {




            /// <summary>
            /// The _core
            /// </summary>
            protected ObservableCollection<T> _core = new ObservableCollection<T>();
            /// <summary>
            /// Initializes a new instance of the <see cref="DependencyObservableCollection{T}"/> class.
            /// </summary>
            public DependencyObservableCollection()
            {

                var countBinding = new Binding();
                countBinding.Path = new PropertyPath("Count");
                countBinding.Mode = BindingMode.OneWay;
                countBinding.Source = _core;
                BindingOperations.SetBinding(this, CountProperty, countBinding);

            }




            /// <summary>
            /// Adds the specified item.
            /// </summary>
            /// <param name="item">The item.</param>
            public void Add(T item)
            {
                _core.Add(item);
            }

            /// <summary>
            /// Clears this instance.
            /// </summary>
            public void Clear()
            {
                _core.Clear();
            }

            /// <summary>
            /// Determines whether [contains] [the specified item].
            /// </summary>
            /// <param name="item">The item.</param>
            /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
            public bool Contains(T item)
            {
                return _core.Contains(item);
            }

            /// <summary>
            /// Copies to.
            /// </summary>
            /// <param name="array">The array.</param>
            /// <param name="arrayIndex">Index of the array.</param>
            public void CopyTo(T[] array, int arrayIndex)
            {
                _core.CopyTo(array, arrayIndex);
            }





            /// <summary>
            /// Gets or sets the count.
            /// </summary>
            /// <value>The count.</value>
            public virtual int Count
            {
                get { return (int)GetValue(CountProperty); }
                protected set { SetValue(CountProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
            /// <summary>
            /// The count property
            /// </summary>
            public static readonly DependencyProperty CountProperty =
                DependencyProperty.Register("Count", typeof(int), typeof(DependencyObservableCollection<T>), new PropertyMetadata(0));




            /// <summary>
            /// Gets a value indicating whether this instance is read only.
            /// </summary>
            /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
            public bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// Removes the specified item.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            public bool Remove(T item)
            {
                return _core.Remove(item);
            }

            /// <summary>
            /// Gets the enumerator.
            /// </summary>
            /// <returns>IEnumerator&lt;T&gt;.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                return _core.GetEnumerator();
            }

            /// <summary>
            /// Gets the enumerator.
            /// </summary>
            /// <returns>IEnumerator.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _core.GetEnumerator();
            }

            /// <summary>
            /// Occurs when [collection changed].
            /// </summary>
            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add { _core.CollectionChanged += value; }
                remove { _core.CollectionChanged -= value; }
            }

            /// <summary>
            /// Occurs when [property changed].
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged
            {
                add { ((INotifyPropertyChanged)_core).PropertyChanged += value; }
                remove { ((INotifyPropertyChanged)_core).PropertyChanged -= value; }
            }


            /// <summary>
            /// Indexes the of.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <returns>System.Int32.</returns>
            public int IndexOf(T item)
            {
                return _core.IndexOf(item);
            }

            /// <summary>
            /// Inserts the specified index.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="item">The item.</param>
            public void Insert(int index, T item)
            {
                _core.Insert(index, item);
            }

            /// <summary>
            /// Removes at.
            /// </summary>
            /// <param name="index">The index.</param>
            public void RemoveAt(int index)
            {
                _core.RemoveAt(index);
            }

            /// <summary>Gets or sets the item at the specified index.</summary>
            /// <value>The item.</value>
            /// <param name="index">The index.</param>
            /// <returns>T.</returns>
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

			/// <summary>
			/// Initializes a new instance of the <see cref="ObservableVector{T}"/> class.
			/// </summary>
			public ObservableVector()
				: base()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ObservableVector{T}"/> class.
			/// </summary>
			/// <param name="list">The list.</param>
			public ObservableVector(IList<T> list)
				: base(list)
			{
			}

			// *** Protected Methods ***   


			/// <summary>
			/// Clear Items
			/// </summary>
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

			/// <summary>
			/// Inserts the item.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="item">The item.</param>
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

			/// <summary>
			/// Removes the item.
			/// </summary>
			/// <param name="index">The index.</param>
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

			/// <summary>
			/// Sets the item.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="item">The item.</param>
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

			/// <summary>
			/// Called when [property changed].
			/// </summary>
			/// <param name="propertyName">Name of the property.</param>
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

			/// <summary>
			/// Occurs when [vector changed].
			/// </summary>
			public event Windows.Foundation.Collections.VectorChangedEventHandler<object> VectorChanged;



        #region IList<object> Members

			/// <summary>
			/// Indexes the of.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns></returns>
			int IList<object>.IndexOf(object item)
			{
				return IndexOf((T)item);
			}

			/// <summary>
			/// Inserts the specified index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="item">The item.</param>
			void IList<object>.Insert(int index, object item)
			{
				Insert(index, (T)item);
			}



			/// <summary>
			/// Gets or sets the <see cref="System.Object"/> at the specified index.
			/// </summary>
			/// <value>
			/// The <see cref="System.Object"/>.
			/// </value>
			/// <param name="index">The index.</param>
			/// <returns></returns>
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

			/// <summary>
			/// Adds the specified item.
			/// </summary>
			/// <param name="item">The item.</param>
			void ICollection<object>.Add(object item)
			{
				Add((T)item);
			}


			/// <summary>
			/// Determines whether [contains] [the specified item].
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns></returns>
			bool ICollection<object>.Contains(object item)
			{
				return Contains((T)item);
			}

			/// <summary>
			/// Copies to.
			/// </summary>
			/// <param name="array">The array.</param>
			/// <param name="arrayIndex">Index of the array.</param>
			void ICollection<object>.CopyTo(object[] array, int arrayIndex)
			{

				for (int i = 0; i < Count; i++)
				{
					var idx = arrayIndex + i;
					array[idx] = this[i];

				}
			}



			/// <summary>
			/// Removes the specified item.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns></returns>
			bool ICollection<object>.Remove(object item)
			{
				return Remove((T)item);
			}

        #endregion

        #region IEnumerable<object> Members

			/// <summary>
			/// Gets the enumerator.
			/// </summary>
			/// <returns></returns>
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				return this.OfType<Object>().GetEnumerator();
			}

        #endregion

        #region IEnumerable Members

			/// <summary>
			/// Gets the enumerator.
			/// </summary>
			/// <returns></returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

        #endregion

        #region ICollection<object> Members


			/// <summary>
			/// Gets a value indicating whether this instance is read only.
			/// </summary>
			/// <value>
			/// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
			/// </value>
			public bool IsReadOnly
			{
				get { return false; }
			}

        #endregion
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

#endif
        /// <summary>
        /// <para> The extension method for collections </para>
        /// <para>集合类型的扩展方法</para>
        /// </summary>
        public static class CollectionExtensions
        {
            /// <summary>
            /// <para>Transform to a dictionary with INotifyCollectionChanged</para>
            /// <para>生成一个带有集合变化通知的字典</para>
            /// </summary>
            /// <typeparam name="K">The type of the t group.</typeparam>
            /// <typeparam name="V"><para>Value Type</para><para>值类型</para></typeparam>
            /// <param name="items"><para>Source Dictionary</para><para>来源字典</para><para></para></param>
            /// <returns>KeyedObservableCollection&lt;K, V&gt;.</returns>
            public static KeyedObservableCollection<K, V> ToKeyedObservableCollection<K, V>(this IDictionary<K, V> items)
            {
                return new KeyedObservableCollection<K, V>(items);

            }


        }

        public class ValueWithIndex<TValue>
        {
            public int Index { get; set; }
            public TValue Value { get; set; }
        }



        /// <summary>
        /// Class KeyedObservableCollection.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public class KeyedObservableCollection<TKey, TValue> : ObservableCollection<KeyValuePair<TKey, TValue>>
        {     

            /// <summary>
            /// Initializes a new instance of the <see cref="KeyedObservableCollection{K, V}"/> class.
            /// </summary>
            /// <param name="items">The items.</param>
            /// <exception cref="System.ArgumentException">items could not be null.</exception>
            public KeyedObservableCollection(IDictionary<TKey, TValue> items)
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



            /// <summary>
            /// The _core dictionary
            /// </summary>
            IDictionary<TKey, TValue> _coreDictionary;
            /// <summary>
            /// The _core version
            /// </summary>
            int _coreVersion;
            /// <summary>
            /// The _shadow version
            /// </summary>
            int _shadowVersion;
            /// <summary>
            /// Incs the ver.
            /// </summary>
            private void IncVer()
            {
                _coreVersion++;
                if (_coreVersion >= 1024 * 1024 * 1024)
                {
                    _coreVersion = 0;
                }
            }



            /// <summary>
            /// Clears the items.
            /// </summary>
            protected override void ClearItems()
            {
                base.ClearItems();
                _coreDictionary.Clear();
                IncVer();
            }


            /// <summary>
            /// Inserts the item.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="item">The item.</param>
            protected override void InsertItem(int index, KeyValuePair<TKey, TValue> item)
            {

                base.InsertItem(index, item);
                _coreDictionary.Add(item.Key, item.Value);

                IncVer();
            }

            /// <summary>
            /// Sets the item.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="item">The item.</param>
            protected override void SetItem(int index, KeyValuePair<TKey, TValue> item)
            {

                _coreDictionary.Add(item.Key, item.Value);
                RemoveFromDic(index);

                base.SetItem(index, item);
                IncVer();
            }

            /// <summary>
            /// Removes from dic.
            /// </summary>
            /// <param name="index">The index.</param>
            private void RemoveFromDic(int index)
            {
                var rem = base[index];
                if (rem.Key != null)
                {
                    _coreDictionary.Remove(rem.Key);
                }
                IncVer();
            }

            /// <summary>
            /// Removes the item.
            /// </summary>
            /// <param name="index">The index.</param>
            protected override void RemoveItem(int index)
            {
                RemoveFromDic(index);
                base.RemoveItem(index);
                IncVer();
            }

            public void AddOrUpdateByKey(KeyValuePair<TKey, TValue> item)
            {

            
                ValueWithIndex<TValue> v = null;
                if (this.DictionaryItems.TryGetValue(item.Key, out v))
                {
                    this._coreDictionary[item.Key] = item.Value;
                    base.SetItem(v.Index, item);

                }
                else
                {
                    this._coreDictionary[item.Key] = item.Value;
                    base.Add(item);
                }
                IncVer();
            }

            public void RemoveByKey(TKey key)
            {

                //this._coreDictionary.Remove(key);
                ValueWithIndex<TValue> v = null;
                if (this.DictionaryItems.TryGetValue(key, out v))
                {
                    _coreDictionary.Remove(key);
                    base.RemoveAt(v.Index);

                }
                else
                {
                    _coreDictionary.Remove(key);                    
                }
                IncVer();
            }


#if SILVERLIGHT_5 || NET40 || WINDOWS_PHONE_7
            Dictionary<TKey, ValueWithIndex<TValue>> _shadowDictionary;


            /// <summary>Gets the dictionary items.</summary>
            /// <value>The dictionary items.</value>
            public IDictionary<TKey, ValueWithIndex<TValue>> DictionaryItems
            {
                get
                {
                    if (_shadowDictionary == null || _shadowVersion != _coreVersion)
                    {
                        _shadowDictionary = new Dictionary<TKey, ValueWithIndex<TValue>>(this.Select((o, i) =>
                   new { index = i, kvp = o }).ToDictionary(x => x.kvp.Key, x => new ValueWithIndex<TValue> { Index = x.index, Value = x.kvp.Value })

                        );
                        _shadowVersion = _coreVersion;
                    }
                    return _shadowDictionary;

                }
            }

#else
            /// <summary>
            /// The _shadow dictionary
            /// </summary>
            ReadOnlyDictionary<TKey, ValueWithIndex<TValue>> _shadowDictionary;
            /// <summary>
            /// Gets the dictionary items.
            /// </summary>
            /// <value>The dictionary items.</value>
            public IDictionary<TKey, ValueWithIndex<TValue>> DictionaryItems
            {
                get
                {
                    if (_shadowDictionary == null || _shadowVersion != _coreVersion)
                    {
                        _shadowDictionary = new ReadOnlyDictionary<TKey, ValueWithIndex<TValue>>(this.Select((o, i) =>
                       new { index = i, kvp = o }).ToDictionary(x => x.kvp.Key, x => new ValueWithIndex<TValue> { Index = x.index, Value = x.kvp.Value })

                            );
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




			/// <summary>
			/// <para>ICollectionView generic implmention</para>
			/// <para>ICollectionView 的泛型实现</para>
			/// </summary>
			/// <typeparam name="T"><para>Content Type</para><para>内容类型</para> </typeparam>
			public class CollectionView<T> : ObservableVector<T>, ICollectionView
			{
				/// <summary>
				/// <para>Constructor of Collection View</para>
				/// <para>构造函数</para>
				/// </summary>
				/// <param name="items"><para>Initialing Items</para><para>初始内容集合</para></param>
				/// <param name="loader"><para>increanatal loader</para><para>自增加载器</para></param>
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

				/// <summary>
				/// <para>Constructor of Collection View</para>
				/// <para>构造函数</para>
				/// </summary>
				/// <param name="items"><para>Initialing Items</para><para>初始内容集合</para></param>
				/// <param name="groupCollection"><para>Initaling Groups</para><para>初始分组</para></param>
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
				/// <summary>
				/// <para>Insert Item</para><para>插入</para>
				/// </summary>
				/// <param name="index"><para>targeting index</para><para>目标索引</para></param>
				/// <param name="item"><para> Item inserting</para><para>插入项</para></param>
				protected override void InsertItem(int index, T item)
				{
					base.InsertItem(index, item);
					if (_group != null)
					{
						_group.AddItemToGroups(item);
					}
				}

				/// <summary>
				/// 	  Set Item
				/// </summary>
				/// <param name="index">index</param>
				/// <param name="item">item </param>
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
				/// <summary>
				/// <para>Clear Items</para>
				/// <para>清除内容</para>
				/// </summary>
				protected override void ClearItems()
				{
					base.ClearItems();
					if (_group != null)
					{
						_group.Clear();
					}
				}
				/// <summary>
				/// <para>Clear Items</para>
				/// <para>清除内容</para>
				/// </summary>
				/// <param name="index"></param>
				protected override void RemoveItem(int index)
				{
					var olditem = base.Items[index];
					base.RemoveItem(index);

					if (_group != null)
					{
						_group.RemoveItemFromGroups(olditem);
					}
				}

				/// <summary>
				/// <para>Incremental Loader</para>
				/// <para>自增读取器</para>
				/// </summary>
				protected CollectionViewIncrementalLoader<T> _loader;

				Windows.Foundation.Collections.IObservableVector<object> ThisVector { get { return this; } }

        #region ICollectionView Members

				private CollectionViewGroupCollection<T> _group;
				/// <summary>
				/// <para>Collection Groups</para>
				/// <para>集合中的分组</para>
				/// </summary>
				public Windows.Foundation.Collections.IObservableVector<object> CollectionGroups
				{
					get { return _group; }
				}

				/// <summary>
				/// <para>Fired when current Item has changed</para>
				/// <para>当前项变化后触发</para>
				/// </summary>
				public event EventHandler<object> CurrentChanged;
				/// <summary>
				/// <para>Fired when current Item is changing</para>
				/// <para>当前项变化前触发</para>
				/// </summary>
				public event CurrentChangingEventHandler CurrentChanging;


				/// <summary>
				/// <para>Current Item </para>
				/// <para>当前项</para>
				/// </summary>
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
				/// <summary>
				/// <para>Current Item Index</para><para>当前项的索引</para>
				/// </summary>
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


				/// <summary>
				/// <para>Has more items can loaded by loader</para><para>是否还有更多的数据</para>
				/// </summary>
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


				/// <summary>
				/// <para>Is Current Item is beyond  bound of collection</para><para>当前项目是否已经超出集合最大范围</para>
				/// </summary>
				public bool IsCurrentAfterLast
				{
					get { return _CurrentPosition >= this.Count; }
				}
				/// <summary>
				/// <para>Is Current Item is before  bound of collection</para><para>当前项目是否已经在集合之前</para>
				/// </summary>

				public bool IsCurrentBeforeFirst
				{
					get { return _CurrentPosition < 0; }
				}

				/// <summary>
				/// <para>Load More Items</para><para>加载更多的项</para>
				/// </summary>
				/// <param name="count"><para>count of items</para><para>个数</para></param>
				/// <returns></returns>
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


				/// <summary>Moves the current to first.</summary>
				/// <returns></returns>
				public bool MoveCurrentToFirst()
				{
					var newIndex = 0;
					return MoveCurrentToPosition(newIndex);

				}



				/// <summary>Moves the current to last.</summary>
				/// <returns></returns>
				public bool MoveCurrentToLast()
				{
					var newIndex = Count - 1;
					return MoveCurrentToPosition(newIndex);
				}

				/// <summary>Moves the current to next.</summary>
				/// <returns></returns>
				public bool MoveCurrentToNext()
				{
					var newIndex = CurrentPosition + 1;
					return MoveCurrentToPosition(newIndex);
				}

				/// <summary>Moves the current to position.</summary>
				/// <param name="index">The index.</param>
				/// <returns></returns>
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

				/// <summary>Moves the current to previous.</summary>
				/// <returns></returns>
				public bool MoveCurrentToPrevious()
				{
					var newIndex = CurrentPosition - 1;
					return MoveCurrentToPosition(newIndex);
				}



				/// <summary>Moves the current to.</summary>
				/// <param name="item">The item.</param>
				/// <returns></returns>
				public bool MoveCurrentTo(object item)
				{
					var index = IndexOf((T)item);
					return MoveCurrentToPosition(index);
				}

        #endregion

			}

			/// <summary>
			/// 
			/// </summary>
			/// <typeparam name="T"></typeparam>
			public class CollectionViewIncrementalLoader<T> : ISupportIncrementalLoading
			{
				private Func<CollectionView<T>, int, Task<IncrementalLoadResult<T>>> _loadMore;
				private Func<CollectionView<T>, bool> _hasMore;
				bool _hasNoMore = false;
				/// <summary>
				/// 
				/// </summary>
				/// <param name="loadMore"></param>
				/// <param name="hasMore"></param>
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

				/// <summary>
				/// Collection View
				/// </summary>

				public CollectionView<T> CollectionView { get; set; }

        #region ISupportIncrementalLoading Members



				/// <summary>Gets a value indicating whether this instance has more items.</summary>
				/// <value>
				/// <c>true</c> if this instance has more items; otherwise, <c>false</c>.
				/// </value>
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

				/// <summary>Internals the load more items asynchronous.</summary>
				/// <param name="count">The count.</param>
				/// <returns></returns>
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


				/// <summary>Loads the more items asynchronous.</summary>
				/// <param name="count">The count.</param>
				/// <returns></returns>
				public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
				{
					return InternalLoadMoreItemsAsync(count).AsAsyncOperation();
				}

        #endregion
			}

			/// <summary>
			/// 	  IncrementalLoadResult
			/// </summary>
			/// <typeparam name="T"></typeparam>
			public struct IncrementalLoadResult<T>
			{
				/// <summary>
				/// NewItems
				/// </summary>
				public IList<T> NewItems { get; set; }
				/// <summary>
				/// HaveMore
				/// </summary>
				public bool HaveMore { get; set; }
			}

			/// <summary>
			/// 
			/// </summary>
			public class CollectionViewGroupCollectionItem : ICollectionViewGroup
			{

				/// <summary>
				/// Crewate a new 	 CollectionViewGroupCollectionItem
				/// </summary>
				/// <param name="group"></param>
				/// <param name="items"></param>
				/// <returns></returns>
				public static CollectionViewGroupCollectionItem Create(object group, IObservableVector<object> items)
				{
					return new CollectionViewGroupCollectionItem(group, items);
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="CollectionViewGroupCollectionItem"/> class.
				/// </summary>
				/// <param name="group">The group.</param>
				/// <param name="items">The items.</param>
				public CollectionViewGroupCollectionItem(object group, IObservableVector<object> items)
				{
					Group = group;
					GroupItems = items;
				}

				/// <summary>Gets the group.</summary>
				/// <value>The group.</value>
				public object Group { get; private set; }
				/// <summary>Gets the group items.</summary>
				/// <value>The group items.</value>
				public IObservableVector<object> GroupItems { get; private set; }



			}

			/// <summary>
			/// 
			/// </summary>
			/// <typeparam name="TItem">The type of the item.</typeparam>
			public abstract class CollectionViewGroupCollection<TItem> : ObservableVector<CollectionViewGroupCollectionItem>
			{
				/// <summary>Creates the specified group key getter.</summary>
				/// <typeparam name="TGroupKey">The type of the group key.</typeparam>
				/// <typeparam name="TGroup">The type of the group.</typeparam>
				/// <param name="groupKeyGetter">The group key getter.</param>
				/// <param name="groupFactory">The group factory.</param>
				/// <param name="index">The index.</param>
				/// <returns></returns>
				public static CollectionViewGroupCollection<TItem, TGroupKey, TGroup> Create<TGroupKey, TGroup>(Func<TItem, TGroupKey> groupKeyGetter, Func<TItem, TGroup> groupFactory, Dictionary<TGroupKey, CollectionViewGroupCollectionItem> index = null)
				{
					return new CollectionViewGroupCollection<TItem, TGroupKey, TGroup>(groupKeyGetter, groupFactory, index);

				}

				/// <summary>Adds the item to groups.</summary>
				/// <param name="item">The item.</param>
				public abstract void AddItemToGroups(object item);


				/// <summary>Removes the item from groups.</summary>
				/// <param name="item">The item.</param>
				public abstract void RemoveItemFromGroups(object item);
			}
			/// <summary>
			/// 
			/// </summary>
			/// <typeparam name="TItem">The type of the item.</typeparam>
			/// <typeparam name="TGroupKey">The type of the group key.</typeparam>
			/// <typeparam name="TGroup">The type of the group.</typeparam>
			public class CollectionViewGroupCollection<TItem, TGroupKey, TGroup> : CollectionViewGroupCollection<TItem>
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="CollectionViewGroupCollection{TItem, TGroupKey, TGroup}"/> class.
				/// </summary>
				/// <param name="groupKeyGetter">The group key getter.</param>
				/// <param name="groupFactory">The group factory.</param>
				/// <param name="index">The index.</param>
				public CollectionViewGroupCollection(Func<TItem, TGroupKey> groupKeyGetter, Func<TItem, TGroup> groupFactory, Dictionary<TGroupKey, CollectionViewGroupCollectionItem> index = null)
				{
					_groupKeyGetter = groupKeyGetter;
					_groupFactory = groupFactory;

					_index = index ?? new Dictionary<TGroupKey, CollectionViewGroupCollectionItem>();
				}
				Func<TItem, TGroupKey> _groupKeyGetter;

				Dictionary<TGroupKey, CollectionViewGroupCollectionItem> _index;
				private Func<TItem, TGroup> _groupFactory;


				/// <summary>Adds the item to groups.</summary>
				/// <param name="item">The item.</param>
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

				/// <summary>Removes the item from groups.</summary>
				/// <param name="item">The item.</param>
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
