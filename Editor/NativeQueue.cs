using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class NativeQueue
    {
        [Test]
        public void Test()
        {
            var queue = new NativeQueue<int>(Unity.Collections.Allocator.Temp, 4);
            queue.Enqueue(1);
            queue.Dequeue();

            for(int i=0; i<100; ++i)
            {
                queue.Enqueue(i);
            }
            Assert.AreEqual(100, queue.Count);

            for (int i = 0; i < 100; ++i)
            {
                Assert.AreEqual(i, queue.Dequeue());
            }
            Assert.AreEqual(0, queue.Count);

            for (int w = 0; w < 3; ++w)
            {
                for (int i = 0; i < 39; ++i)
                {
                    queue.Enqueue(i);
                }
                Assert.AreEqual(39, queue.Count);
                for (int i = 0; i < 39; ++i)
                {
                    Assert.AreEqual(i, queue.Dequeue());
                }
                Assert.AreEqual(0, queue.Count);
            }

            queue.Enqueue(0);
            queue.Clear();
            Assert.AreEqual(0, queue.Count);

            queue.Dispose();
        }
    }
}

