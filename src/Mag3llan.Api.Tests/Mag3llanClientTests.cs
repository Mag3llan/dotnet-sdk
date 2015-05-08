using System;
using NUnit;
using NUnit.Framework;
using Mag3llan.Api.Client;
using RestSharp;
using FakeItEasy;

namespace Mag3llan.Api.Tests
{
    public class Mag3llanClientTests
    {
        [TestFixture]
        public class ConstructorTests
        {
            [TestCase("@")]
            [TestCase("#")]
            [TestCase(".")]
            [TestCase("'")]
            [TestCase("(")]
            [TestCase(")")]
            [TestCase("%")]
            [TestCase("~")]
            public void InvalidUriCharactersThrowsException(string chars)
            {
                Assert.Throws<UriFormatException>(() => new Mag3llanClient(chars, "bar"));
            }

            [Test]
            public void MissingHostnameThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient(null, "bar"));

                Assert.That(ex.ParamName, Is.EqualTo("hostname"));
            }

            [Test]
            public void EmptyHostnameThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient(string.Empty, "bar"));

                Assert.That(ex.ParamName, Is.EqualTo("hostname"));
            }

            [Test]
            public void BlankHostnameThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("  ", "bar"));

                Assert.That(ex.ParamName, Is.EqualTo("hostname"));
            }

            [Test]
            public void MissingKeyThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("api", null));

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

        [TestFixture]
        public class SetPreferenceTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [TestFixtureSetUp]
            public void SetupOnce()
            {
                this.client = A.Fake<IRestClient>();
                //A.CallTo(() => )
                this.sdk = new Mag3llanClient(this.client, "http://api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.SetPreference(-1, 1, 1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void NegativeItemId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.SetPreference(1, -1, 1));

                Assert.That(ex.ParamName, Is.EqualTo("itemId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ValidRequestCreatesPreference()
            {
                sdk.SetPreference(1, 1, 1);
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }
    }
}
