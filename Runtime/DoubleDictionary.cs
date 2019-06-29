using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TSKT
{
    public class DoubleDictionary<Key1, Key2, Value> : IEnumerable<(Key1 key1, Key2 key2, Value value)>
    {
        readonly Dictionary<Key1, Dictionary<Key2, Value>> dictionary = new Dictionary<Key1, Dictionary<Key2, Value>>();

        public bool TryGetValue(Key1 key1, Key2 key2, out Value result)
        {
            if (dictionary.TryGetValue(key1, out var dict))
            {
                return dict.TryGetValue(key2, out result);
            }

            result = default;
            return false;
        }

        public void Add(Key1 key1, Key2 key2, Value value)
        {
            if (!dictionary.TryGetValue(key1, out var dict))
            {
                dict = new Dictionary<Key2, Value>();
                dictionary.Add(key1, dict);
            }
            dict.Add(key2, value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public IEnumerator<(Key1 key1, Key2 key2, Value value)> GetEnumerator()
        {
            return GetEnumeratorInternal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumeratorInternal();
        }

        IEnumerator<(Key1, Key2, Value)> GetEnumeratorInternal()
        {
            foreach (var it in dictionary)
            {
                var key1 = it.Key;
                foreach (var pair in it.Value)
                {
                    var key2 = pair.Key;
                    var value = pair.Value;
                    yield return (key1, key2, value);
                }
            }
        }

        public IEnumerable<Value> Values
        {
            get
            {
                return dictionary.Values.SelectMany(_ => _.Values);
            }
        }

        public Value this[Key1 key1, Key2 key2]
        {
            get
            {
                return dictionary[key1][key2];
            }
            set
            {
                if (!dictionary.TryGetValue(key1, out var dict))
                {
                    dict = new Dictionary<Key2, Value>();
                    dictionary.Add(key1, dict);
                }
                dict[key2] = value;
            }
        }

        public bool ContainsKey(Key1 key1, Key2 key2)
        {
            if (!dictionary.TryGetValue(key1, out var dict))
            {
                return false;
            }
            return dict.ContainsKey(key2);
        }

        public Dictionary<Key1, Dictionary<Key2, Value>> ToDictionary()
        {
            var result = new Dictionary<Key1, Dictionary<Key2, Value>>();
            foreach(var it in dictionary)
            {
                result.Add(it.Key, new Dictionary<Key2, Value>(it.Value));
            }
            return result;
        }

        static public DoubleDictionary<Key1, Key2, Value> Create(Dictionary<Key1, Dictionary<Key2, Value>> original)
        {
            var result = new DoubleDictionary<Key1, Key2, Value>();
            foreach(var it in original)
            {
                foreach (var pair in it.Value)
                {
                    result.Add(it.Key, pair.Key, pair.Value);
                }
            }
            return result;
        }
    }
}
