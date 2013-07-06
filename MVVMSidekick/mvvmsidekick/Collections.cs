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
using MVVMSidekick.EventRouting ;
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
                _coreDictionary = items;
                foreach (var item in items)
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


    }

}
