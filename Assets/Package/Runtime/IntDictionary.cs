using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TSKT
{
    public class IntDictionary<T> : IEnumerable<KeyValuePair<T, int>>
    {
        readonly Dictionary<T, int> dictionary;

        public IntDictionary()
        {
            dictionary = new Dictionary<T, int>();
        }

        public IntDictionary(Dictionary<T, int> source)
        {
            dictionary = new Dictionary<T, int>(source);
        }

        public void Add(IntDictionary<T> src)
        {
            foreach (var it in src.dictionary)
            {
                this[it.Key] += it.Value;
            }
        }

        public void Sub(IntDictionary<T> src)
        {
            foreach (var it in src.dictionary)
            {
                this[it.Key] -= it.Value;
            }
        }

        public int this[T key]
        {
            get
            {
                if (dictionary.TryGetValue(key, out var value))
                {
                    return value;
                }
                return 0;
            }
            set
            {
                if (value == 0)
                {
                    dictionary.Remove(key);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public Dictionary<T, int>.KeyCollection Keys => dictionary.Keys;
        public Dictionary<T, int>.ValueCollection Values => dictionary.Values;

        public int Count => dictionary.Count;

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}
