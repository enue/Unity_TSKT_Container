using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TSKT
{
    public class FloatDictionary<T> : IEnumerable<KeyValuePair<T, float>>
        where T : System.IComparable
    {
        readonly OrderedDictionary<T, float> dictionary = new OrderedDictionary<T, float>();
        float defaultValue;

        public FloatDictionary(float defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public void Mul(FloatDictionary<T> src)
        {
            foreach (var it in src.dictionary)
            {
                this[it.Key] *= it.Value;
            }

            defaultValue *= src.defaultValue;
        }

        public float this[T key]
        {
            get
            {
                if (dictionary.TryGetValue(key, out var value))
                {
                    return value;
                }
                return defaultValue;
            }
            set
            {
                if (value == defaultValue)
                {
                    dictionary.Remove(key);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
        }

        public int Count => dictionary.Count;

        public IEnumerator<KeyValuePair<T, float>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}
