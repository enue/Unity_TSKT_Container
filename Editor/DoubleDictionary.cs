using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace TSKT.Tests
{
    public class DoubleDictionary
    {
        [Test]
        public void Access()
        {
            var dict = Sample;
            Assert.AreEqual("hoge", dict[0, 0]);

            var found = dict.TryGetValue(1, 1, out var value);
            Assert.IsTrue(found);
            Assert.AreEqual("piyo", value);

            found = dict.TryGetValue(2, 1, out value);
            Assert.IsFalse(found);
            Assert.AreEqual(default(string), value);

            dict[1, 0] = "foo";
            Assert.AreEqual("foo", dict[1, 0]);
            Assert.Catch(() => { var v = dict[2, 0]; });
            Assert.Catch(() => { var v = dict[1, 2]; });

            dict.Add(3, 1, "bar");
            Assert.Catch(() => dict.Add(1, 1, "bar"));
        }

        [Test]
        public void Iterate()
        {
            var dict = Sample;
            var list = dict.ToList();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual((0, 0, "hoge"), list[0]);
            Assert.AreEqual((1, 0, "fuga"), list[1]);
            Assert.AreEqual((1, 1, "piyo"), list[2]);
        }
        [Test]
        public void Clear()
        {
            var dict = Sample;
            dict.Clear();
            Assert.AreEqual(0, dict.Count());
        }

        DoubleDictionary<int, int, string> Sample
        {
            get
            {
                var dict = new DoubleDictionary<int, int, string>
                {
                    { 0, 0, "hoge" },
                    { 1, 0, "fuga" },
                    { 1, 1, "piyo" }
                };
                return dict;
            }
        }

        public void Performance()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            for (int w = 0; w < 10; ++w)
            {
                var dict = new DoubleDictionary<int, int, string>();
                for (int i = 0; i < 100; ++i)
                {
                    for (int j = 0; j < 100; ++j)
                    {
                        dict.Add(i, j, "hoge");
                    }
                }
                for (int i = 0; i < 100; ++i)
                {
                    for (int j = 0; j < 100; ++j)
                    {
                        var value = dict[i, j];
                    }
                }
                dict.ToArray();
            }
            watch.Stop();
            Debug.Log(watch.ElapsedMilliseconds);
        }
    }
}

