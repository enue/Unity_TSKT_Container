#nullable enable
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public struct MemoryBuilder<T> : System.IDisposable
    {
        readonly IMemoryOwner<T> owner;
        int index;

        public MemoryBuilder(int capacity)
        {
            owner = MemoryPool<T>.Shared.Rent(capacity);
            index = 0;
        }

        public void Add(in T value)
        {
            owner.Memory.Span[index] = value;
            ++index;
        }

        public System.Memory<T> Memory => owner.Memory[..index];

        public void Dispose()
        {
            owner.Dispose();
        }
    }
}

