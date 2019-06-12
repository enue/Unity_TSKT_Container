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
            Assert.AreEqual(10, container.MaxX);
            Assert.AreEqual(8, container.MaxY);
            Assert.AreEqual(-1, container.MinX);
            Assert.AreEqual(-1, container.MinY);

            container[-2, -5] = 97;
            Assert.AreEqual(97, container[-2, -5]);
            Assert.AreEqual(42, container[10, 0]);
            Assert.AreEqual(10, container.MaxX);
            Assert.AreEqual(8, container.MaxY);
            Assert.AreEqual(-2, container.MinX);
            Assert.AreEqual(-5, container.MinY);
        }

        [Test]
        public void Empty()
        {
            var container = new UnlimitedArray2<string>(0, 0, 0, 0);
            Assert.False(container.Contains(0, 0));
            container[10, -10] = "hoge";
            Assert.AreEqual("hoge", container[10, -10]);
        }

        [Test]
        public void FromDictionary()
        {
            var dict = new Dictionary<Vector2Int, string>()
            {
                { new Vector2Int(1, 1), "hoge"}
            };

            var container = new UnlimitedArray2<string>(dict);
            Assert.AreEqual("hoge", container[1, 1]);
            Assert.AreEqual(null, container[0, 0]);
        }
    }
}

