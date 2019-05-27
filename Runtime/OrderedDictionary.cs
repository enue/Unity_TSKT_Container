using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    public class OrderedDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
        where K : IComparable
    {
        readonly List<K> keys;
        readonly List<V> values;

        public OrderedDictionary()
        {
            keys = new List<K>();
            values = new List<V>();
        }

        public OrderedDictionary(int capacity)
        {
            keys = new List<K>(capacity);
            values = new List<V>(capacity);
        }

        public OrderedDictionary(OrderedDictionary<K, V> src)
        {
            keys = src.keys.ToList();
            values = src.values.ToList();
        }

        public bool TryGetValue(K key, out V result)
        {
            var index = keys.BinarySearch(key);
            if (index >= 0)
            {
                result = values[index];
                return true;
            }

            result = default;
            return false;
        }

        public V this[K key]
        {
            get
            {
                if (TryGetValue(key, out var result))
                {
                    return result;
                }
                return default;
            }
            set
            {
                var index = keys.BinarySearch(key);
                if (index >= 0)
                {
                    values[index] = value;
                    return;
                }
                index = ~index;
                keys.Insert(index, key);
                values.Insert(index, value);
            }
        }

        public bool Remove(K key)
        {
            var index = keys.BinarySearch(key);
            if (index >= 0)
            {
                keys.RemoveAt(index);
                values.RemoveAt(index);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public int Count => keys.Count;

        public IReadOnlyCollection<K> Keys => keys;

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            for(int i=0; i<keys.Count; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < keys.Count; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }
    }
}