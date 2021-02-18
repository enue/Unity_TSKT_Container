using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public struct ArrayBuilder<T> : IEnumerable<T>
    {
        readonly T[] array;
        readonly public T[] Array
        {
            get
            {
                Debug.Assert(Length == array.Length);
                return array;
            }
        }

        readonly public int Capacity => array.Length;
        public int Length { readonly get; private set; }

        public ArrayBuilder(int count)
        {
            array = new T[count];
            Length = 0;
        }

        public void Add(in T value)
        {
            array[Length] = value;
            ++Length;
        }

        readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return array[i];
            }
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return array[i];
            }
        }
    }
}

