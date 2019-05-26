using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class UnlimitedArray2
    {
        [Test]
        public void Test()
        {
            var container = new UnlimitedArray2<int>(-1, -1, 10, 10);
            Assert.True(container.Contains(0, 0));
            Assert.False(container.Contains(10, 0));
            container[10, 0] = 42;
            Assert.True(container.Contains(10, 0));
            Assert.AreEqual(42, container[10, 0]);
            container[-2, -5] = 97;
            Assert.AreEqual(97, container[-2, -5]);
            Assert.AreEqual(42, container[10, 0]);
        }
    }
}

