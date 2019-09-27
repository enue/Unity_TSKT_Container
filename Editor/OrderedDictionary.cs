using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class OrderedDictionary
    {
        [Test]
        public void Test()
        {
            var container = new OrderedDictionary<string, float>();
            container["hoge"] = 42f;
            Assert.AreEqual(42f, container["hoge"]);
            Assert.AreEqual(1, container.Count);

            Assert.AreEqual(default(float), container["fuga"]);

            container["hoge"] = 97f;

            Assert.False(container.Remove("piyo"));
            Assert.True(container.Remove("hoge"));

            Assert.AreEqual(container.Count, container.ToArray().Length);

            container.Clear();
            Assert.AreEqual(0, container.Count);
        }

        [Test]
        public void Comparer()
        {
            var dict = new Dictionary<string, float>();
            for (int i = 0; i < 1000; ++i)
            {
                var k = Random.Range(0, 1000).ToString();
                var v = Random.value;
                dict[k] = v;
            }

            var container = new OrderedDictionary<string, float>();
            foreach(var it in dict)
            {
                container[it.Key] = it.Value;
            }

            foreach (var it in dict)
            {
                Assert.AreEqual(it.Value, container[it.Key]);
            }
        }

        public void Performance()
        {
            var keys = new float[10000];

            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i] = Random.value;
            }
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                for (int w = 0; w < 10; ++w)
                {
                    var dict = new SortedDictionary<float, float>();
                    foreach (var it in keys)
                    {
                        dict[it] = 0;
                    }
                    foreach (var it in keys)
                    {
                        dict.TryGetValue(it, out var v);
                    }
                    dict.ToArray();
                }
                watch.Stop();
                Debug.Log("SortedDictionary:" + watch.ElapsedMilliseconds.ToString());
            }
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                for (int w = 0; w < 10; ++w)
                {
                    var dict = new OrderedDictionary<float, float>();
                    foreach (var it in keys)
                    {
                        dict[it] = 0;
                    }
                    foreach (var it in keys)
                    {
                        dict.TryGetValue(it, out var v);
                    }
                    dict.ToArray();
                }
                watch.Stop();
                Debug.Log("OrderedDictionary:" + watch.ElapsedMilliseconds.ToString());
            }
        }
    }
}

