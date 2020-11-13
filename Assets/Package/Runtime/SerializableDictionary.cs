using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TSKT
{
    [System.Serializable]
    public struct SerializableDictionary<K, V>
    {
        [SerializeField]
        K[] keys;

        [SerializeField]
        V[] values;

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
    }
}
