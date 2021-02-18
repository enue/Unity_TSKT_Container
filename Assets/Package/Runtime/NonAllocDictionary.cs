using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    public readonly struct NonAllocDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        readonly K[] keys;
        readonly V[] values;
        readonly IComparer<K> comparer;

        public NonAllocDictionary(K[] sortedKeys, V[] values)
        {
            keys = sortedKeys;
            this.values = values;
            comparer = null;
        }

        public NonAllocDictionary(K[] sortedKeys, V[] values, IComparer<K> comparer)
        {
            keys = sortedKeys;
            this.values = values;
            this.comparer = comparer;
        }

        readonly public bool TryGetValue(K key, out V result)
        {
            if (keys == null)
            {
                result = default;
                return false;
            }
            int index;
            if (comparer == null)
            {
                index = Array.BinarySearch(keys, key);
            }
            else
            {
                index = Array.BinarySearch(keys, key, comparer);
            }
            if (index >= 0)
            {
                result = values[index];
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

        readonly public V this[K key]
        {
            get
            {
                int index;
                if (comparer == null)
                {
                    index = Array.BinarySearch(keys, key);
                }
                else
                {
                    index = Array.BinarySearch(keys, key, comparer);
                }
                return values[index];
            }
        }

        readonly public int Count => keys.Length;

        readonly IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            for(int i=0; i<keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }
    }
}