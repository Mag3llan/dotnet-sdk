using System;
using NUnit;
using NUnit.Framework;
using Mag3llan.Api.Client;

namespace Mag3llan.Api.Tests
{
    public class Mag3llanClientTests
    {
        [TestFixture]
        public class ConstructorTests
        {
            [Test]
            public void InvalidUriThrowsException()
            {
                Assert.Throws<UriFormatException>(() => new Mag3llanClient("foo", "bar"));
            }

            [Test]
            public void MissingUriThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient(null, "bar"));

                Assert.That(ex.ParamName, Is.EqualTo("uri"));
            }

            [Test]
            public void EmptyUriThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient(string.Empty, "bar"));

                Assert.That(ex.ParamName, Is.EqualTo("uri"));
            }

            [Test]
            public void BlankUriThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("  ", "bar"));

                Assert.That(ex.ParamName, Is.EqualTo("uri"));
            }

            [Test]
            public void MissingKeyThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("http://api", null));

                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }

            [Test]
            public void EmptyKeyThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("http://api", string.Empty));

                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }

            [Test]
            public void BlankKeyThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("http://api", "  "));

                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }
    }
}
