using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public struct ArrayBuilder<T> : IEnumerable<T>
    {
        readonly T[] array;
        public T[] Array
        {
            get
            {
                UnityEngine.Assertions.Assert.AreEqual(array.Length, Length);
                return array;
            }
        }

        public int Capacity => array.Length;
        public int Length { get; private set; }

        public ArrayBuilder(int count)
        {
            array = new T[count];
            Length = 0;
        }

        public void Add(T value)
        {
            array[Length] = value;
            ++Length;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return array[i];
            }
        }
    }
}

