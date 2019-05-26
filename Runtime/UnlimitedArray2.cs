using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public class UnlimitedArray2<T>
    {
        T[,] array;
        public Vector2Int Min { get; private set; }

        public Vector2Int Size { get; private set; }
        public int Width => Size.x;
        public int Height => Size.y;
        public int MinX => Min.x;
        public int MinY => Min.y;
        public int MaxX => Size.x + Min.x - 1;
        public int MaxY => Size.y + Min.y - 1;

        public UnlimitedArray2(int width, int height)
            : this(0, 0, width, height)
        {
        }

        public UnlimitedArray2(int minX, int minY, int width, int height)
        {
            array = new T[width, height];
            Min = new Vector2Int(minX, minY);
            Size = new Vector2Int(width, height);
        }

        public UnlimitedArray2(Dictionary<Vector2Int, T> source)
        {
            if (source.Count == 0)
            {
                array = new T[0, 0];
                Min = new Vector2Int(0, 0);
                Size = new Vector2Int(0, 0);
            }
            else
            {
                var xMin = int.MaxValue;
                var yMin = int.MaxValue;
                var xMax = int.MinValue;
                var yMax = int.MinValue;
                foreach (var it in source)
                {
                    if (xMin > it.Key.x)
                    {
                        xMin = it.Key.x;
                    }
                    if (yMin > it.Key.y)
                    {
                        yMin = it.Key.y;
                    }
                    if (xMax < it.Key.x)
                    {
                        xMax = it.Key.x;
                    }
                    if (yMax < it.Key.y)
                    {
                        yMax = it.Key.y;
                    }
                }
                array = new T[xMax - xMin + 1, yMax - yMin + 1];
                Min = new Vector2Int(xMin, yMin);
                Size = new Vector2Int(xMax - xMin + 1, yMax - yMin + 1);

                foreach (var it in source)
                {
                    this[it.Key.x, it.Key.y] = it.Value;
                }
            }
        }

        public T this[int x, int y]
        {
            get
            {
                if (!Contains(x, y))
                {
                    return default;
                }
                return array[x - Min.x, y - Min.y];
            }
            set
            {
                EnsureCapacity(x, y);
                array[x - Min.x, y - Min.y] = value;
            }
        }

        public bool Contains(int x, int y)
        {
            return x >= MinX
                && y >= MinY
                && x <= MaxX
                && y <= MaxY;
        }

        void EnsureCapacity(int x, int y)
        {
            EnsureCapacity(new RectInt(x, y, 0, 0));
        }

        void EnsureCapacity(RectInt rect)
        {
            var oldMin = Min;
            var shouldReplace = false;

            if (Min.x > rect.xMin)
            {
                Size = new Vector2Int(MaxX - rect.xMin + 1, Height);
                Min = new Vector2Int(rect.xMin, Min.y);
                shouldReplace = true;
            }
            if (MaxX < rect.xMax)
            {
                Size = new Vector2Int(rect.xMax - Min.x + 1, Height);
                shouldReplace = true;
            }

            if (Min.y > rect.yMin)
            {
                Size = new Vector2Int(Width, MaxY - rect.yMin + 1);
                Min = new Vector2Int(Min.x, rect.yMin);
                shouldReplace = true;
            }

            if (MaxY < rect.yMax)
            {
                Size = new Vector2Int(Width, rect.yMax - Min.y + 1);
                shouldReplace = true;
            }

            if (shouldReplace)
            {
                var oldArray = array;
                array = new T[Width, Height];
                Copy(oldMin, oldArray, Min, array);
            }
        }

        static void Copy(Vector2Int srcMin, T[,] src, Vector2Int destMin, T[,] dest)
        {
            for (int i = 0; i < src.GetLength(0); ++i)
            {
                for (int j = 0; j < src.GetLength(1); ++j)
                {
                    var v = src[i, j];
                    dest[i + srcMin.x - destMin.x, j + srcMin.y - destMin.y] = v;
                }
            }
        }
    }
}
