using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class NativeBitArray
    {
        [Test]
        public void Test()
        {
            var list = new TSKT.NativeBitArray(100, Unity.Collections.Allocator.Temp);

            Assert.AreEqual(100, list.Length);

            foreach (var it in list)
            {
                Assert.False(it);
            }

            for (var i = 0; i < list.Length; ++i)
            {
                list[i] = true;
                Assert.True(list[i]);
            }

            for (var i = 0; i < list.Length; ++i)
            {
                list[i] = false;
                Assert.False(list[i]);
            }

            list.SetAll(true);
            foreach (var it in list)
            {
                Assert.True(it);
            }

            list.SetAll(false);
            foreach (var it in list)
            {
                Assert.False(it);
            }

            list.Dispose();
        }
    }
}

