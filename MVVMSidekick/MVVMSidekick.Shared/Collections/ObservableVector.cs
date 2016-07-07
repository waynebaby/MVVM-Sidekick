#if NETFX_CORE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MVVMSidekick.Collections
{



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





}
#endif
