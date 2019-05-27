using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace TSKT
{
    public struct NativeBitArray : IDisposable, IEnumerable<bool>
    {
        NativeArray<int> array;
        public int Length { get; }

        public NativeBitArray(int length, Allocator allocator)
        {
            Length = length;
            if (Length == 0)
            {
                array = new NativeArray<int>(
                    0,
                    allocator, NativeArrayOptions.ClearMemory);
            }
            else
            {
                array = new NativeArray<int>(
                    (length - 1) / 32 + 1,
                    allocator, NativeArrayOptions.ClearMemory);
            }
        }

        public void Set(int index, bool value)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            var i = index / 32;
            var j = index % 32;
            var item = array[i];
            if (value)
            {
                item = (item | (1 << j));
            }
            else
            {
                item = (item & ~(1 << j));
            }
            array[i] = item;
        }

        public bool this[int index]
        {
            get
            {
                var i = index / 32;
                var j = index % 32;
                return (array[i] & (1 << j)) != 0;
            }
            set
            {
                Set(index, value);
            }
        }

        public void Dispose()
        {
            array.Dispose();
        }

        public void SetAll(bool value)
        {
            var fillValue = value ? unchecked(((int)0xffffffff)) : 0;
            for (int i=0; i<array.Length; ++i)
            {
                array[i] = fillValue;
            }
        }

        public void And(NativeBitArray source)
        {
            for(int i=0; i<array.Length; ++i)
            {
                var v = array[i] & source.array[i];
                array[i] = v;
            }
        }

        public void Or(NativeBitArray source)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                var v = array[i] | source.array[i];
                array[i] = v;
            }
        }

        public void Xor(NativeBitArray source)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                var v = array[i] ^ source.array[i];
                array[i] = v;
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return this[i];
            }
        }
    }
}
