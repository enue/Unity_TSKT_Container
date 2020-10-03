using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    [System.Serializable]
    public class SerializableOrderedDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        [SerializeField]
        List<K> keys = new List<K>();

        [SerializeField]
        List<V> values = new List<V>();

        public IReadOnlyCollection<K> Keys => keys;
        public IReadOnlyCollection<V> Values => values;
        public int Count => keys.Count;

        public SerializableOrderedDictionary()
        {
        }

        public SerializableOrderedDictionary(List<K> sortedKeys, List<V> values)
        {
            keys.AddRange(sortedKeys);
            this.values.AddRange(values);
        }

        int GetIndex(K key)
        {
            return keys.BinarySearch(key);
        }

        public bool TryGetValue(K key, out V result)
        {
            var index = GetIndex(key);
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
                var index = GetIndex(key);
                if (index < 0)
                {
                    throw new KeyNotFoundException();
                }
                return values[index];
            }
            set
            {
                var index = GetIndex(key);
                if (index < 0)
                {
                    index = ~index;
                    keys.Insert(index, key);
                    values.Insert(index, value);
                }
                else
                {
                    values[index] = value;
                }
            }
        }

        public void Add(K key, V value)
        {
            var index = GetIndex(key);
            if (index >= 0)
            {
                throw new ArgumentException();
            }
            index = ~index;
            keys.Insert(index, key);
            values.Insert(index, value);
        }

        public bool Remove(K key)
        {
            var index = GetIndex(key);
            if (index < 0)
            {
                return false;
            }
            keys.RemoveAt(index);
            values.RemoveAt(index);
            return true;
        }
        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public bool ContainsKey(K key)
        {
            return GetIndex(key) >= 0;
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
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