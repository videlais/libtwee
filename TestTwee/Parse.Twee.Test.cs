using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class ParseTweeTests
    {
        [Test]
        public void TestParse_InvalidDocument()
        {
            string tweeContent = "";
            var result = Twee.Parse(tweeContent);
            // Assert that an exception is thrown if the document does not contain any passages.
            Assert.That(() => Twee.Parse(tweeContent), Throws.Exception);
        }
    }
}