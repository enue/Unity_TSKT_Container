using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace TSKT
{
    public struct NativeQueue<T> : IDisposable, IEnumerable<T>
        where T : struct
    {
        const int DefaultCapacity = 4;
        const int MaxArrayLength = 0X7FEFFFFF;
        NativeArray<T> array;
        public int Count { get; private set; }
        public int head;
        readonly Allocator allocator;

        public NativeQueue(Allocator allocator, int capacity = DefaultCapacity)
        {
            array = new NativeArray<T>(capacity, allocator, NativeArrayOptions.UninitializedMemory);
            Count = 0;
            head = 0;
            this.allocator = allocator;
        }

        public void Enqueue(T item)
        {
            EnsureCapacity(Count + 1);
            array[(Count + head) % array.Length] = item;
            ++Count;
        }

        public T Dequeue()
        {
            var result = array[head];
            ++head;
            if (head >= array.Length)
            {
                head = 0;
            }
            --Count;
            return result;
        }

        public void Clear()
        {
            head = 0;
            Count = 0;
        }

        public void Dispose()
        {
            array.Dispose();
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (Count > 0)
            {
                yield return Dequeue();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            while (Count > 0)
            {
                yield return Dequeue();
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
                for (int i = 0; i < Count; ++i)
                {
                    newArray[i] = array[(i + head) % array.Length];
                }

                array.Dispose();
                array = newArray;
                head = 0;
            }
        }
    }
}
