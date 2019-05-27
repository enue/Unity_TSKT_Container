using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public readonly struct BitArray2
    {
        readonly BitArray bitArray;
        readonly int width;
        readonly int height;
        readonly int offsetX;
        readonly int offsetY;

        public BitArray2(int width, int height)
        {
            this.height = height;
            this.width = width;
            offsetX = 0;
            offsetY = 0;
            if (width == 0 && height == 0)
            {
                bitArray = null;
            }
            else
            {
                bitArray = new BitArray(height * width);
            }
        }

        public BitArray2(IEnumerable<Vector2Int> source)
        {
            if (source == null)
            {
                width = 0;
                height = 0;
                offsetX = 0;
                offsetY = 0;
                bitArray = null;
                return;
            }

            using (var tmp = new NativeList<Vector2Int>(Unity.Collections.Allocator.TempJob))
            {
                foreach(var it in source)
                {
                    tmp.Add(it);
                }

                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int maxX = int.MinValue;
                int maxY = int.MinValue;
                foreach (var it in tmp)
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

                offsetX = minX;
                offsetY = minY;
                width = maxX - minX + 1;
                height = maxY - minY + 1;
                bitArray = new BitArray(height * width);

                foreach (var it in tmp)
                {
                    this[it.x, it.y] = true;
                }
            }
        }

        public void Clear()
        {
            if (bitArray != null)
            {
                bitArray.SetAll(false);
            }
        }

        public void Or(IEnumerable<Vector2Int> values)
        {
            foreach(var it in values)
            {
                Set(it.x, it.y, true);
            }
        }

        public void And(in BitArray2 src)
        {
            if (bitArray == null)
            {
                return;
            }
            if (src.bitArray == null)
            {
                bitArray.SetAll(false);
                return;
            }

            UnityEngine.Assertions.Assert.AreEqual(width, src.width, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(height, src.height, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetX, src.offsetX, "boardのoffsetが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetY, src.offsetY, "boardのoffsetが一致しません");

            bitArray.And(src.bitArray);
        }

        public void Or(in BitArray2 src)
        {
            if (src.bitArray == null)
            {
                return;
            }

            UnityEngine.Assertions.Assert.AreEqual(width, src.width, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(height, src.height, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetX, src.offsetX, "boardのoffsetが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetY, src.offsetY, "boardのoffsetが一致しません");

            bitArray.Or(src.bitArray);
        }

        public void Xor(in BitArray2 src)
        {
            if (src.bitArray == null)
            {
                return;
            }

            UnityEngine.Assertions.Assert.AreEqual(width, src.width, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(height, src.height, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetX, src.offsetX, "boardのoffsetが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetY, src.offsetY, "boardのoffsetが一致しません");

            bitArray.Xor(src.bitArray);
        }

        public void ExceptWith(in BitArray2 src)
        {
            if (bitArray == null)
            {
                return;
            }
            if (src.bitArray == null)
            {
                return;
            }

            UnityEngine.Assertions.Assert.AreEqual(width, src.width, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(height, src.height, "boardのサイズが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetX, src.offsetX, "boardのoffsetが一致しません");
            UnityEngine.Assertions.Assert.AreEqual(offsetY, src.offsetY, "boardのoffsetが一致しません");

            // expect
            // true, true -> false
            // true, false -> true
            // false, true -> false
            // false, false -> false

            // or
            // true, true -> true
            // true, false -> true
            // false, true -> true
            // false, false -> false

            // xor
            // true, true -> false
            // true, false -> true
            // false, true -> true
            // false, false -> false

            bitArray.Or(src.bitArray);
            bitArray.Xor(src.bitArray);
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

            UnityEngine.Assertions.Assert.IsTrue(x - offsetX >= 0, "out of range " + x.ToString());
            UnityEngine.Assertions.Assert.IsTrue(y - offsetY >= 0, "out of range " + y.ToString());
            UnityEngine.Assertions.Assert.IsTrue(x - offsetX < width, "out of range " + x.ToString());
            UnityEngine.Assertions.Assert.IsTrue(y - offsetY < height, "out of range " + y.ToString());
            bitArray.Set((x - offsetX) * height + (y - offsetY), value);
        }

        public bool this[int x, int y]
        {
            get
            {
                if (bitArray == null)
                {
                    return false;
                }
                if (!Contains(x, y))
                {
                    return false;
                }
                return bitArray.Get((x - offsetX) * height + (y - offsetY));
            }
            set
            {
                Set(x, y, value);
            }
        }

        bool Contains(int x, int y)
        {
            if (x - offsetX < 0)
            {
                return false;
            }
            if (y - offsetY < 0)
            {
                return false;
            }
            if (x - offsetX >= width)
            {
                return false;
            }
            if (y - offsetY >= height)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Vector2Int> Enables()
        {
            if (bitArray == null)
            {
                yield break;
            }
            for (int i = 0; i < bitArray.Length; ++i)
            {
                if (bitArray[i])
                {
                    var x = i / height + offsetX;
                    var y = i % height + offsetY;
                    yield return new Vector2Int(x, y);
                }
            }
        }
    }
}

