#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    public readonly struct NonAllocDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        readonly ReadOnlyMemory<K> keys;
        readonly ReadOnlyMemory<V> values;
        readonly IComparer<K> comparer;

        public NonAllocDictionary(ReadOnlyMemory<K> sortedKeys, ReadOnlyMemory<V> values, IComparer<K>? comparer = null)
        {
            keys = sortedKeys;
            this.values = values;
            this.comparer = comparer ?? Comparer<K>.Default;
        }


        readonly public bool TryGetValue(K key, out V result)
        {
            var index = keys.Span.BinarySearch(key, comparer);
            if (index >= 0)
            {
                result = values.Span[index];
                return true;
            }

            result = default;
            return false;
        }

        readonly public V TryGetValue(K key, V defaultValue = default)
        {
            if (TryGetValue(key, out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public readonly V this[K key]
        {
            get
            {
                var index = keys.Span.BinarySearch(key, comparer);
                return values.Span[index];
            }
        }

        readonly public int Count => keys.Length;

        readonly IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys.Span[i], values.Span[i]);
            }
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys.Span[i], values.Span[i]);
            }
        }
    }
}