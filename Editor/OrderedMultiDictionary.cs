using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class OrderedMultiDictionary
    {
        [Test]
        public void Test()
        {
            var container = new OrderedMultiDictionary<string, float>();
            container.Add("hoge", 42f);
            Assert.AreEqual(42f, container["hoge"][0]);

            Assert.AreEqual(0, container["fuga"].Count);

            container.Add("hoge", 97f);

            Assert.False(container.Remove("piyo"));
            Assert.True(container.Remove("hoge"));
        }

        [Test]
        public void Comparer()
        {
            var keys = new List<string>();
            for(int i=0; i<1000; ++i)
            {
                keys.Add(Random.Range(0, 10).ToString());
            }

            var container = new OrderedMultiDictionary<string, float>(System.StringComparer.Ordinal);
            foreach(var it in keys)
            {
                container.Add(it, 0f);
            }

            Assert.AreEqual(
                keys.OrderBy(_ => _, System.StringComparer.Ordinal).ToArray(),
                container.Select(_ => _.Key).ToArray());
        }

        [Test]
        [TestCase(-0.5f, 1f)]
        [TestCase(-0.5f, 1.5f)]
        [TestCase(0f, 1f)]
        public void GetRange(float begin, float end)
        {
            var container = new OrderedMultiDictionary<float, string>();
            container.Add(0f, "hoge");
            container.Add(0f, "fuga");
            container.Add(0f, "piyo");
            container.Add(-1f, "a");
            container.Add(1f, "b");

            var range = container.GetRange(begin, end).ToArray();
            Assert.AreEqual(new KeyValuePair<float, string>(0f, "hoge"), range[0]);
            Assert.AreEqual(new KeyValuePair<float, string>(0f, "fuga"), range[1]);
            Assert.AreEqual(new KeyValuePair<float, string>(0f, "piyo"), range[2]);
            Assert.AreEqual(new KeyValuePair<float, string>(1f, "b"), range[3]);
            Assert.AreEqual(4, range.Length);
        }
    }
}

