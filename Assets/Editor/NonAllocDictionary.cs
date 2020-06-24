using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace TSKT.Tests
{
    public class NonAllocDictionary
    {
        [Test]
        public void Iterate()
        {
            int[] keys = { 0, 1, 2, 3, 4, 5, 6 };
            int[] values = { 10, 11, 12, 13, 14, 15, 16 };
            var dict = new NonAllocDictionary<int, int>(keys, values);

            var k = new List<int>();
            var v = new List<int>();

            foreach (var it in dict)
            {
                k.Add(it.Key);
                v.Add(it.Value);
            }

            Assert.AreEqual(keys, k.ToArray());
            Assert.AreEqual(values, v.ToArray());

        }
        [Test]
        public void Index()
        {
            int[] keys = { 0, 1, 2, 3, 4, 5, 6 };
            int[] values = { 10, 11, 12, 13, 14, 15, 16 };
            var dict = new NonAllocDictionary<int, int>(keys, values);
            for (int i = 0; i < keys.Length; ++i)
            {
                Assert.AreEqual(values[i], dict[keys[i]]);
            }
        }

        [Test]
        public void Comparer()
        {
            var dict = new Dictionary<string, float>();
            for (int i = 0; i < 1000; ++i)
            {
                var k =Random.Range(0, 1000).ToString();
                var v = Random.value;
                dict[k] = v;
            }

            var ordered = dict.OrderBy(_ => _.Key, System.StringComparer.Ordinal).ToArray();
            var container = new NonAllocDictionary<string, float>(
                ordered.Select(_ => _.Key).ToArray(),
                ordered.Select(_ => _.Value).ToArray(),
                System.StringComparer.Ordinal);

            foreach(var it in dict)
            {
                Assert.AreEqual(it.Value, container[it.Key]);
            }
        }
    }
}

