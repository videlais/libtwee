using NUnit.Framework;
using System;
using System.Collections.Generic;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class ParseTwine1TWSTest
    {
        [Test]
        public void TestParseTwine1TWS_InvalidDocument_InvalidBase64() {
            // Expected exception.
            Assert.Throws<Exception>(() => {
                // Parse the TWS file.
                Twine1TWS.Parse(Convert.FromBase64String("aW52YWxpZA=="));
            });
        }

        [Test]
        public void TestParse1TWS_ValidDocument() {
            // Create path string.
            string path = Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, @"../../../TestFiles/Example1.tws");

            // Read as array of bytes.
            byte[] tws = System.IO.File.ReadAllBytes(path);

            // Parse the TWS file.
            //Story story = Twine1TWS.Parse(tws);
        }
    }
}