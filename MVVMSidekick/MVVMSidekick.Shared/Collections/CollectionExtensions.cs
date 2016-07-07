using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Collections
{
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
}
