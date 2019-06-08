using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    public class OrderedMultiDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
        where K : IComparable<K>
    {
        readonly SortedList<K, List<V>> list = new SortedList<K, List<V>>();

        public void Add(K key, V value)
        {
            if (!list.TryGetValue(key, out var values))
            {
                values = new List<V>();
                list.Add(key, values);
            }
            values.Add(value);
        }

        public bool TryGetValue(K key, out List<V> result)
        {
            return list.TryGetValue(key, out result);
        }

        public List<V> this[K key]
        {
            get
            {
                if (!TryGetValue(key, out var result))
                {
                    result = new List<V>();
                    list.Add(key, result);
                }
                return result;
            }
        }

        public bool Remove(K key)
        {
            return list.Remove(key);
        }

        public void Clear()
        {
            list.Clear();
        }

        public IEnumerable<KeyValuePair<K, V>> GetRange(K begin, K end)
        {
            var beginIndex = BinarySearch(begin);
            if (beginIndex < 0)
            {
                beginIndex = ~beginIndex;
            }
            var endIndex = BinarySearch(end);
            if (endIndex < 0)
            {
                endIndex = ~endIndex - 1;
            }

            var count = endIndex - beginIndex + 1;
            for(int i=0; i<count; ++i)
            {
                var key = list.Keys[i + beginIndex];
                var values = list.Values[i + beginIndex];
                foreach(var it in values)
                {
                    yield return new KeyValuePair<K, V>(key, it);
                }
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach(var it in list)
            {
                foreach(var value in it.Value)
                {
                    yield return new KeyValuePair<K, V>(it.Key, value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var it in list)
            {
                foreach (var value in it.Value)
                {
                    yield return new KeyValuePair<K, V>(it.Key, value);
                }
            }
        }

        int BinarySearch(K k)
        {
            var keys = list.Keys;
 
            int low = 0;
            int high = keys.Count - 1;
            while (low <= high)
            {
                int median = low + ((high - low) >> 1);

                var c = keys[median].CompareTo(k);
                if (c == 0)
                {
                    return median;
                }
                if (c < 0)
                {
                    low = median + 1;
                }
                else
                {
                    high = median - 1;
                }
            }

            return ~low;
        }
    }
}