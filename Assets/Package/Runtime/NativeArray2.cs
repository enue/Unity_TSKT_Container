using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace TSKT
{
    public struct NativeArray2<T> : IDisposable, IEnumerable<KeyValuePair<Vector2Int, T>>
        where T : struct
    {
        NativeArray<T> nativeArray;
        readonly public int Width { get; }
        readonly public int Height { get; }

        public NativeArray2(int w, int h, Allocator allocator)
        {
            Width = w;
            Height = h;
            nativeArray = new NativeArray<T>(w * h, allocator, NativeArrayOptions.ClearMemory);
        }

        public T this[int x, int y]
        {
            readonly get
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return nativeArray[x * Height + y];
            }
            set
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                {
                    throw new ArgumentOutOfRangeException();
                }
                nativeArray[x * Height + y] = value;
            }
        }

        readonly public void Dispose()
        {
            nativeArray.Dispose();
        }

        readonly public IEnumerator<KeyValuePair<Vector2Int, T>> GetEnumerator()
        {
            for(int i=0; i<nativeArray.Length; ++i)
            {
                yield return new KeyValuePair<Vector2Int, T>(
                    new Vector2Int(i / Height, i % Height),
                    nativeArray[i]);
            }
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < nativeArray.Length; ++i)
            {
                yield return new KeyValuePair<Vector2Int, T>(
                    new Vector2Int(i / Height, i % Height),
                    nativeArray[i]);
            }
        }
    }
}
