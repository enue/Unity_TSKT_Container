using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class NativeList
    {
        [Test]
        public void Test()
        {
            var list = new NativeList<int>(Unity.Collections.Allocator.Temp, 4)
            {
                0,
                1,
                2,
                3,
                4,
                5
            };
            Assert.AreEqual(6, list.Count);
            for(int i=0; i<list.Count; ++i)
            {
                Assert.AreEqual(i, list[i]);
            }
            var array = list.ToArray();

            var index = 0;
            foreach(var i in list)
            {
                Assert.AreEqual(index, i);
                Assert.AreEqual(array[index], i);
                ++index;
            }

            list.Clear();
            Assert.AreEqual(0, list.Count);

            list.Dispose();
        }
    }
}

