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

        public NonAllocDictionary(K[] sortedKeys, V[] values)
        {
            keys = sortedKeys;
            this.values = values;
        }

        public bool TryGetValue(K key, out V result)
        {
            if (keys == null)
            {
                result = default;
                return false;
            }
            var index = Array.BinarySearch(keys, key);
            if (index >= 0)
            {
                result = values[index];
                return true;
            }

            result = default;
            return false;
        }

        public V TryGetValue(K key, V defaultValue = default)
        {
            if (TryGetValue(key, out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public V this[K key]
        {
            get
            {
                var index = System.Array.BinarySearch(keys, key);
                return values[index];
            }
        }

        public int Count
        {
            get
            {
                return keys.Length;
            }
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            for(int i=0; i<keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }
    }
}