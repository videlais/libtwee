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

        [Test]
        public void TestIsValidTwineIFID_EdgeCases()
        {
            Assert.Multiple(() =>
            {
                // Test empty string
                Assert.That(Babel.IsValidTwineIFID(""), Is.False);
                
                // Test null string - this should handle gracefully
                Assert.That(() => Babel.IsValidTwineIFID(null!), Throws.TypeOf<ArgumentNullException>());
                
                // Test too short
                Assert.That(Babel.IsValidTwineIFID("123"), Is.False);
                
                // Test too long
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-000000000000-extra"), Is.False);
                
                // Test missing hyphens
                Assert.That(Babel.IsValidTwineIFID("00000000000000000000000000000000"), Is.False);
                
                // Test wrong hyphen positions
                Assert.That(Babel.IsValidTwineIFID("0000000-00000-0000-0000-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-000-00000-0000-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-000-00000-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-000-0000000000000"), Is.False);
                
                // Test invalid characters
                Assert.That(Babel.IsValidTwineIFID("0000000G-0000-0000-0000-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-000Z-0000-0000-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-000X-0000-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-000Y-000000000000"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000V"), Is.False);
                
                // Test special characters
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000@"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000#"), Is.False);
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000$"), Is.False);
                
                // Test spaces
                Assert.That(Babel.IsValidTwineIFID("00000000-0000-0000-0000-00000000000 "), Is.False);
                Assert.That(Babel.IsValidTwineIFID(" 0000000-0000-0000-0000-000000000000"), Is.False);
                
                // Test valid uppercase and lowercase
                Assert.That(Babel.IsValidTwineIFID("ABCDEF01-2345-6789-ABCD-EF0123456789"), Is.True);
                Assert.That(Babel.IsValidTwineIFID("abcdef01-2345-6789-abcd-ef0123456789"), Is.True);
                Assert.That(Babel.IsValidTwineIFID("AbCdEf01-2345-6789-AbCd-Ef0123456789"), Is.True);
                
                // Test numbers
                Assert.That(Babel.IsValidTwineIFID("12345678-1234-5678-1234-567812345678"), Is.True);
                
                // Test real UUID formats
                Assert.That(Babel.IsValidTwineIFID("550e8400-e29b-41d4-a716-446655440000"), Is.True);
                Assert.That(Babel.IsValidTwineIFID("A77C26C0-C331-11EF-A8FA-0800200C9A66"), Is.True);
            });
        }
    }
}