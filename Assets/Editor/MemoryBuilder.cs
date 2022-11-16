#nullable enable
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace TSKT.Tests
{
    public class MemoryBuilder
    {
        [Test]
        public void Test()
        {
            var original = new[] { "hoge", "fuga", "piyo" };
            using var buffer = System.Buffers.MemoryPool<string>.Shared.Rent(original.Length);
            var builder = new MemoryBuilder<string>(buffer.Memory);
            builder.Add(original[0]);
            builder.Add(original[1]);
            builder.Add(original[2]);
            Assert.AreEqual(original, builder.Memory.ToArray());
        }
    }
}

