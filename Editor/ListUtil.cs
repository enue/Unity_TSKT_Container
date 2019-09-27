using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace TSKT.Tests
{
    public class ListUtil
    {
        [Test]
        public void RemoveAll()
        {
            var list = new List<int>() { 1, 1, 2, 3, 1, 1, 1 };
            var removedCount = TSKT.ListUtil.RemoveAll(list, 1);
            Assert.AreEqual(5, removedCount);
            Assert.AreEqual(new List<int>() { 2, 3 }, list);

            TSKT.ListUtil.RemoveAll(list, 2);
            Assert.AreEqual(1, list.Count);
            TSKT.ListUtil.RemoveAll(list, 3);
            Assert.AreEqual(0, list.Count);
            TSKT.ListUtil.RemoveAll(list, 1);
        }
    }
}
