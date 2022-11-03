﻿using System;
using JsonDiffPatch;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tavis.JsonPatch.Tests
{
    public class TestTests
    {
        [Test]
        public void Test_a_value()
        {

            var sample = PatchTests.GetSample2();

            var patchDocument = new PatchDocument();
            var pointer = new JsonPointer("/books/0/author");

            patchDocument.AddOperation(new TestOperation(pointer, new JValue("Billy Burton")));

            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                var patcher = new JsonPatcher();
                patcher.Patch(ref sample, patchDocument);
            });

        }
    }
}
