using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System;

namespace TSKT
{
    public readonly struct NativeBitArray2 : IDisposable
    {
        readonly NativeBitArray bitArray;
        public int OffsetX { get; }
        public int OffsetY { get; }

        public int Width { get; }
        public int Height { get; }

        public NativeBitArray2(int width, int height, Allocator allocator, int offsetX = 0, int offsetY = 0)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            Width = width;
            Height = height;
            bitArray = new NativeBitArray(width * height, allocator);
        }

        public NativeBitArray2(Allocator allocator, params Vector2Int[] param)
        {
            if (param == null || param.Length == 0)
            {
                OffsetX = 0;
                OffsetY = 0;
                Width = 0;
                Height = 0;
                bitArray = new NativeBitArray(0, allocator);
                return;
            }

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach(var it in param)
            {
                if (maxX < it.x)
                {
                    maxX = it.x;
                }
                if (minX > it.x)
                {
                    minX = it.x;
                }
                if (maxY < it.y)
                {
                    maxY = it.y;
                }
                if (minY > it.y)
                {
                    minY = it.y;
                }
            }

            OffsetX = minX;
            OffsetY = minY;
            Width = maxX - minX + 1;
            Height = maxY - minY + 1;
            bitArray = new NativeBitArray(Width * Height, allocator);

            foreach (var it in param)
            {
                this[it.x, it.y] = true;
            }
        }

        public void Clear()
        {
            SetAll(false);
        }

        public void Or(IEnumerable<Vector2Int> values)
        {
            foreach(var it in values)
            {
                Set(it.x, it.y, true);
            }
        }

        public void And(in NativeBitArray2 src)
        {
            if (Width * Height == 0)
            {
                return;
            }
            if (src.Width * src.Height == 0)
            {
                Clear();
                return;
            }

            if (Width == src.Width
                && Height == src.Height
                && OffsetX == src.OffsetX
                && OffsetY == src.OffsetY)
            {
                bitArray.And(src.bitArray);
                return;
            }

            foreach (var it in src.All)
            {
                if (!it.Value)
                {
                    Set(it.Key.x, it.Key.y, false);
                }
            }
        }

        public void Or(in NativeBitArray2 src)
        {
            if (src.Width * src.Height == 0)
            {
                return;
            }

            if (Width == src.Width
                && Height == src.Height
                && OffsetX == src.OffsetX
                && OffsetY == src.OffsetY)
            {
                bitArray.Or(src.bitArray);
                return;
            }

            foreach (var it in src.Enableds)
            {
                Set(it.x, it.y, true);
            }
        }

        public void ExceptWith(IEnumerable<Vector2Int> cells)
        {
            foreach (var it in cells)
            {
                Set(it.x, it.y, false);
            }
        }

        public void ExceptWith(in NativeBitArray2 src)
        {
            if (Width * Height == 0)
            {
                return;
            }
            if (src.Width * src.Height == 0)
            {
                return;
            }

            // expect
            // true, true -> false
            // true, false -> true
            // false, true -> false
            // false, false -> false

            foreach (var it in src.Enableds)
            {
                Set(it.x, it.y, false);
            }
        }

        public void SetAll(bool value)
        {
            bitArray.SetAll(value);
        }

        void Set(int x, int y, bool value)
        {
            if (!value)
            {
                if (!Contains(x, y))
                {
                    return;
                }
            }
            bitArray.Set((x - OffsetX) * Height + y - OffsetY, value);
        }

        public bool this[int x, int y]
        {
            get
            {
                if (!Contains(x, y))
                {
                    return false;
                }
                return bitArray[(x - OffsetX) * Height + y - OffsetY];
            }
            set
            {
                Set(x, y, value);
            }
        }

        bool Contains(int x, int y)
        {
            if (x - OffsetX < 0)
            {
                return false;
            }
            if (y - OffsetY < 0)
            {
                return false;
            }
            if (x - OffsetX >= Width)
            {
                return false;
            }
            if (y - OffsetY >= Height)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Vector2Int> Enableds
        {
            get
            {
                for(var i=0; i<Width; ++i)
                {
                    for(var j=0; j<Height; ++j)
                    {
                        if (this[i + OffsetX, j + OffsetY])
                        {
                            yield return new Vector2Int(
                                i + OffsetX,
                                j + OffsetY);
                        }
                    }
                }
            }
        }

        public IEnumerable<KeyValuePair<Vector2Int, bool>> All
        {
            get
            {
                for (var i = 0; i < Width; ++i)
                {
                    for (var j = 0; j < Height; ++j)
                    {
                        if (this[i + OffsetX, j + OffsetY])
                        {
                            yield return new KeyValuePair<Vector2Int, bool>(
                                new Vector2Int(
                                    i + OffsetX,
                                    j + OffsetY),
                                true);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            bitArray.Dispose();
        }
    }
}

