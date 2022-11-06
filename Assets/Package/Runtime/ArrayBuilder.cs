#nullable enable
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public readonly struct ArrayBuilder<T>
    {
        public readonly ArrayBufferWriter<T> memory;

        public ArrayBuilder(int count)
        {
            memory = new(count);
        }

        public void Add(in T value)
        {
            memory.GetSpan(1)[0] = value;
            memory.Advance(1);
        }
    }
}

