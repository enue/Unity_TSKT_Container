#nullable enable
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public readonly struct ArrayBuilder<T>
    {
        public readonly ArrayBufferWriter<T> writer;

        public ArrayBuilder(int count)
        {
            if (count == 0)
            {
                writer = new();
            }
            else
            {
                writer = new(count);
            }
        }

        public void Add(in T value)
        {
            writer.GetSpan(1)[0] = value;
            writer.Advance(1);
        }
    }
}

