using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace TSKT
{
    public struct NativeList<T> : IDisposable, IEnumerable<T>
        where T : struct
    {
        const int DefaultCapacity = 4;
        const int MaxArrayLength = 0X7FEFFFFF;
        NativeArray<T> array;
        public int Count { get; private set; }
        readonly Allocator allocator;

        public NativeList(Allocator allocator, int capacity = DefaultCapacity)
        {
            array = new NativeArray<T>(capacity, allocator, NativeArrayOptions.UninitializedMemory);
            Count = 0;
            this.allocator = allocator;
        }

        public void Add(T item)
        {
            EnsureCapacity(Count + 1);
            array[Count] = item;
            ++Count;
        }

        public T this[int index]
        {
            get
            {
                UnityEngine.Assertions.Assert.IsTrue(index >= 0);
                UnityEngine.Assertions.Assert.IsTrue(index < Count);
                return array[index];
            }
            set
            {
                UnityEngine.Assertions.Assert.IsTrue(index >= 0);
                UnityEngine.Assertions.Assert.IsTrue(index < Count);
                array[index] = value;
            }
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Dispose()
        {
            array.Dispose();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(int i=0; i<Count; ++i)
            {
                yield return array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i<Count; ++i)
            {
                yield return array[i];
            }
        }

        // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/Collections/Generic/List.cs#L411
        void EnsureCapacity(int min)
        {
            if (array.Length < min)
            {
                int newCapacity = (array.Length == 0) ? DefaultCapacity : array.Length * 2;
                // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
                // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                if ((uint)newCapacity > MaxArrayLength)
                {
                    newCapacity = MaxArrayLength;
                }
                if (newCapacity < min)
                {
                    newCapacity = min;
                }

                var newArray = new NativeArray<T>(newCapacity, allocator, NativeArrayOptions.UninitializedMemory);
                for (int i = 0; i < array.Length; ++i)
                {
                    newArray[i] = array[i];
                }

                array.Dispose();
                array = newArray;
            }
        }

        public T[] ToArray()
        {
            if (array.Length == Count)
            {
                return array.ToArray();
            }

            var result = new T[Count];
            for(int i=0; i<Count; ++i)
            {
                result[i] = array[i];
            }
            return result;
        }
    }
}
