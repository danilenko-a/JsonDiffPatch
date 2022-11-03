﻿using JsonDiffPatch;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tavis.JsonPatch.Tests
{
    public class CopyTests
    {
        [Test]
        public void Copy_array_element()
        {
            var sample = PatchTests.GetSample2();

            var patchDocument = new PatchDocument();
            var frompointer = new JsonPointer("/books/0");
            var topointer = new JsonPointer("/books/-");

            patchDocument.AddOperation(new CopyOperation(topointer, frompointer));

            var patcher = new JsonPatcher();
            patcher.Patch(ref sample, patchDocument);

            var result = new JsonPointer("/books/2").Find(sample);
            Assert.IsInstanceOf(typeof(JObject), result);

        }

        [Test]
        public void Copy_property()
        {
            var sample = PatchTests.GetSample2();

            var patchDocument = new PatchDocument();
            var frompointer = new JsonPointer("/books/0/ISBN");
            var topointer = new JsonPointer("/books/1/ISBN");

            patchDocument.AddOperation(new AddOperation(frompointer, new JValue("21123123")));
            patchDocument.AddOperation(new CopyOperation(topointer, frompointer));

            var patcher = new JsonPatcher();
            patcher.Patch(ref sample, patchDocument);

            var result = new JsonPointer("/books/1/ISBN").Find(sample);
            Assert.AreEqual("21123123", result.Value<string>());
        }
    }
}
