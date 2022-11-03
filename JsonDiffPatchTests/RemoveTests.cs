﻿using System;
using JsonDiffPatch;
using NUnit.Framework;

namespace Tavis.JsonPatch.Tests
{
    public class RemoveTests
    {
        [Test]
        public void Remove_a_property()
        {

            var sample = PatchTests.GetSample2();

            var patchDocument = new PatchDocument();
            var pointer = new JsonPointer("/books/0/author");

            patchDocument.AddOperation(new RemoveOperation(pointer));

            new JsonPatcher().Patch(ref sample, patchDocument);

            Assert.Throws(typeof(ArgumentException), () => { pointer.Find(sample); });
        }

        [Test]
        public void Remove_an_array_element()
        {

            var sample = PatchTests.GetSample2();

            var patchDocument = new PatchDocument();
            var pointer = new JsonPointer("/books/0");

            patchDocument.AddOperation(new RemoveOperation(pointer));

            var patcher = new JsonPatcher();
            patcher.Patch(ref sample, patchDocument);

            Assert.Throws(typeof(ArgumentException), () =>
            {
                var x = pointer.Find("/books/1");
            });

        }
    }
}
