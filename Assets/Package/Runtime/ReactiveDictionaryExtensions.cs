using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if TSKT_CONTAINER_SUPPORT_UNIRX
using UniRx;

namespace TSKT
{
    public static class ReactiveDictionaryExtentions
    {
        public static ReadOnlyReactiveProperty<V> ObserveValue<K, V>(this ReactiveDictionary<K, V> dict, K key, V defaultValue = default)
        {
            var comparer = EqualityComparer<K>.Default;
            var hashCode = comparer.GetHashCode(key);
            if (!dict.TryGetValue(key, out var current))
            {
                current = defaultValue;
            }
            return dict.ObserveReplace().Where(_ => comparer.GetHashCode(_.Key) == hashCode && comparer.Equals(_.Key, key)).Select(_ => _.NewValue)
                .Merge(
                    dict.ObserveAdd().Where(_ => comparer.GetHashCode(_.Key) == hashCode && comparer.Equals(_.Key, key)).Select(_ => _.Value),
                    dict.ObserveRemove().Where(_ => comparer.GetHashCode(_.Key) == hashCode && comparer.Equals(_.Key, key)).Select(_ => defaultValue))
                .ToReadOnlyReactiveProperty(current);
        }
    }
}
#endif
