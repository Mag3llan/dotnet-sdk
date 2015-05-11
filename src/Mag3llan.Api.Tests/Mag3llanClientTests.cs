using System;
using NUnit;
using NUnit.Framework;
using Mag3llan.Api.Client;
using RestSharp;
using FakeItEasy;
using System.Collections.Generic;

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

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
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
                var response = A.Fake<RestResponse>();
                response.StatusCode = System.Net.HttpStatusCode.Created;
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).Returns(response);

                sdk.SetPreference(1, 1, 1);
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).MustHaveHappened(Repeated.Exactly.Once);
            }

            [Test]
            public void DuplicateRequestUpdatesPreference()
            {
                var response = A.Fake<RestResponse>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).Returns(response);

                sdk.SetPreference(1, 1, 1);
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        [TestFixture]
        public class DeletePreferenceTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "http://api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.DeletePreference(-1, 1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void NegativeItemId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.DeletePreference(1, -1));

                Assert.That(ex.ParamName, Is.EqualTo("itemId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ValidRequestDeletesPreference()
            {
                var response = A.Fake<RestResponse>();
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.DeletePreference(1, 1), Is.EqualTo(true));
            }

            [Test]
            public void MissingPreferenceDoesNotDelete()
            {
                var response = A.Fake<RestResponse>();
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.DeletePreference(1, 1), Is.EqualTo(false));
            }
        }

        [TestFixture]
        public class DeleteUserTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "http://api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.DeleteUser(-1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ValidRequestDeletesUser()
            {
                var response = A.Fake<RestResponse>();
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.DeleteUser(1), Is.EqualTo(true));
            }

            [Test]
            public void MissingUserDoesNotDelete()
            {
                var response = A.Fake<RestResponse>();
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                A.CallTo(() => this.client.Execute(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.DeleteUser(1), Is.EqualTo(false));
            }
        }

        [TestFixture]
        public class GetPluTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "http://api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(-1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ThresholdTooLow()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(1, -1.1m));

                Assert.That(ex.ParamName, Is.EqualTo("threshold"));
                Assert.That(ex.Message, Is.StringStarting("must be between -1 and 1"));
            }

            [Test]
            public void ThresholdTooHigh()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(1, 1.1m));

                Assert.That(ex.ParamName, Is.EqualTo("threshold"));
                Assert.That(ex.Message, Is.StringStarting("must be between -1 and 1"));
            }

            [Test]
            public void ValidRequestReturnsOtherUsers()
            {
                var expected = new List<long> { 123, 456 };
                var response = A.Fake<RestResponse<List<long>>>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = expected;
                A.CallTo(() => this.client.Execute<List<long>>(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.GetPlu(1), Is.EqualTo(expected));
            }

            [Test]
            public void MissingUserReturnsEmptyList()
            {
                var expected = new List<long>();
                var response = A.Fake<RestResponse<List<long>>>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = expected;
                A.CallTo(() => this.client.Execute<List<long>>(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.GetPlu(1), Is.EqualTo(expected));
            }
        }
    }
}
