using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class BabelTest
    {
        [Test]
        public void TestGenerateTwineIFID()
        {
            string ifid = Babel.GenerateTwineIFID();
            Assert.That(Babel.IsValidTwineIFID(ifid), Is.True);
        }

        [Test]
        public void TestIsValidTwineIFID()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-000000000000"), Is.True);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-0000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000g"), Is.False);
            });

        }
    }
}