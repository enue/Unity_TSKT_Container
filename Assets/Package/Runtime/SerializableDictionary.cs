using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    [System.Serializable]
    public struct SerializableDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        public K[] keys;
        public V[] values;

        public SerializableDictionary(Dictionary<K, V> source)
        {
            keys = source.Keys.ToArray();
            values = source.Values.ToArray();
        }

        public Dictionary<K, V> ToDictionary()
        {
            var result = new Dictionary<K, V>(keys.Length);
            for (int i = 0; i < keys.Length; ++i)
            {
                result.Add(keys[i], values[i]);
            }
            return result;
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            if (keys == null)
            {
                yield break;
            }
            for (int i = 0; i < keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (keys == null)
            {
                yield break;
            }
            for (int i = 0; i < keys.Length; ++i)
            {
                yield return new KeyValuePair<K, V>(keys[i], values[i]);
            }
        }
    }
}
