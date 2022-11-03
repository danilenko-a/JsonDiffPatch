﻿using System.IO;
using JsonDiffPatch;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tavis.JsonPatch.Tests
{
    public class PatchTests
    {
        [Test]
        public void CreateEmptyPatch()
        {

            var sample = GetSample2();
            var sampletext = sample.ToString();

            var patchDocument = new PatchDocument();
            new JsonPatcher().Patch(ref sample, patchDocument);

            Assert.AreEqual(sampletext,sample.ToString());
        }


        [Test]
        public void LoadPatch1()
        {
            var names = this.GetType()
                    .Assembly.GetManifestResourceNames();
            var patchDoc =
                PatchDocument.Load(this.GetType()
                    .Assembly.GetManifestResourceStream("JsonDiffPatch.Tests.Samples.LoadTest1.json"));

            Assert.NotNull(patchDoc);
            Assert.AreEqual(6,patchDoc.Operations.Count);
        }


        [Test]
        public void TestExample1()
        {
            var targetDoc = JToken.Parse("{ 'foo': 'bar'}");
            var patchDoc = PatchDocument.Parse(@"[
                                                    { 'op': 'add', 'path': '/baz', 'value': 'qux' }
                                                ]");
            new JsonPatcher().Patch(ref targetDoc, patchDoc);


            Assert.True(JToken.DeepEquals(JToken.Parse(@"{
                                                             'foo': 'bar',
                                                             'baz': 'qux'
                                                           }"), targetDoc));
        }


  

        [Test]
        public void SerializePatchDocument()
        {
            var patchDoc = new PatchDocument(new Operation[]
            {
             new TestOperation(new JsonPointer("/a/b/c"), new JValue("foo")),
             new RemoveOperation(new JsonPointer("/a/b/c")),
             new AddOperation(new JsonPointer("/a/b/c"), new JArray(new JValue("foo"), new JValue("bar"))),
             new ReplaceOperation(new JsonPointer("/a/b/c"), new JValue(42)),
             new MoveOperation(new JsonPointer("/a/b/d"), new JsonPointer("/a/b/c")),
             new CopyOperation(new JsonPointer("/a/b/e"), new JsonPointer("/a/b/d")),
            });

            var outputstream = patchDoc.ToStream();
            var output = new StreamReader(outputstream).ReadToEnd();

            var jOutput = JToken.Parse(output);

            var jExpected = JToken.Parse(new StreamReader(this.GetType()
                .Assembly.GetManifestResourceStream("JsonDiffPatch.Tests.Samples.LoadTest1.json")).ReadToEnd());
            Assert.True(JToken.DeepEquals(jExpected,jOutput));
        }



        public static JToken GetSample2()
        {
            return JToken.Parse(@"{
    'books': [
        {
          'title' : 'The Great Gatsby',
          'author' : 'F. Scott Fitzgerald'
        },
        {
          'title' : 'The Grapes of Wrath',
          'author' : 'John Steinbeck'
        }
    ]
}");
        }
    }
}
