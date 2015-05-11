using System;
using NUnit;
using NUnit.Framework;
using Mag3llan.Api.Client;
using RestSharp;
using FakeItEasy;
using System.Collections.Generic;

namespace Mag3llan.Api.Tests
{
    public class UnitTests
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
            [TestCase("/")]
            [TestCase(":")]
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
            public void HostnameContainingHttpThrowsException()
            {
                var ex = Assert.Throws<ArgumentException>(() => new Mag3llanClient("http://foo", "bar"));

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
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("api", string.Empty));

                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }

            [Test]
            public void BlankKeyThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new Mag3llanClient("api", "  "));

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
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
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
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
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
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
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
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
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

        [TestFixture]
        public class GetPluItemTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(-1, 1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void NegativeItemId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(1, -1));

                Assert.That(ex.ParamName, Is.EqualTo("itemId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ThresholdTooLow()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(1, 1, -1.1m));

                Assert.That(ex.ParamName, Is.EqualTo("threshold"));
                Assert.That(ex.Message, Is.StringStarting("must be between -1 and 1"));
            }

            [Test]
            public void ThresholdTooHigh()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetPlu(1, 1, 1.1m));

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

                Assert.That(sdk.GetPlu(1, 1), Is.EqualTo(expected));
            }

            [Test]
            public void MissingUserReturnsEmptyList()
            {
                var expected = new List<long>();
                var response = A.Fake<RestResponse<List<long>>>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = expected;
                A.CallTo(() => this.client.Execute<List<long>>(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.GetPlu(1, 1), Is.EqualTo(expected));
            }
        }

        [TestFixture]
        public class GetSimilarityTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetSimilarity(-1, 1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void NegativeOtherUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetSimilarity(1, -1));

                Assert.That(ex.ParamName, Is.EqualTo("otherUserId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ValidRequestReturnsSimilarity()
            {
                var expected = 3.5m;
                var response = A.Fake<RestResponse<decimal>>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = expected;
                A.CallTo(() => this.client.Execute<decimal>(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.GetSimilarity(1, 1), Is.EqualTo(expected));
            }

            [Test]
            public void MissingUserReturnsNotFound()
            {
                var response = A.Fake<RestResponse<decimal>>();
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                A.CallTo(() => this.client.Execute<decimal>(A<RestRequest>._)).Returns(response);

                var ex = Assert.Throws<ArgumentException>(() => sdk.GetSimilarity(1, 1));

                Assert.That(ex.Message, Is.StringStarting("user not found"));
            }
        }

        [TestFixture]
        public class GetOverlapsTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetOverlaps(-1, 1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void NegativeOtherUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetOverlaps(1, -1));

                Assert.That(ex.ParamName, Is.EqualTo("otherUserId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ValidRequestReturnsSimilarity()
            {
                var expected = new List<Overlap> { new Overlap { ItemId = 1, Rating = 2.5m, OtherRating = 3.5m, Delta = 1m } };
                var response = A.Fake<RestResponse<List<Overlap>>>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = expected;
                A.CallTo(() => this.client.Execute<List<Overlap>>(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.GetOverlaps(1, 1), Is.EqualTo(expected));
            }

            [Test]
            public void MissingUserReturnsNotFound()
            {
                var response = A.Fake<RestResponse<List<Overlap>>>();
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                A.CallTo(() => this.client.Execute<List<Overlap>>(A<RestRequest>._)).Returns(response);

                var ex = Assert.Throws<ArgumentException>(() => sdk.GetOverlaps(1, 1));

                Assert.That(ex.Message, Is.StringStarting("user not found"));
            }
        }

        [TestFixture]
        public class GetRecommendationsTests
        {
            private Mag3llanClient sdk;
            private IRestClient client;

            [SetUp]
            public void SetupBeforeEachTest()
            {
                this.client = A.Fake<IRestClient>();
                this.sdk = new Mag3llanClient(this.client, "api", "abc");
            }

            [Test]
            public void NegativeUserId()
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sdk.GetRecommendations(-1));

                Assert.That(ex.ParamName, Is.EqualTo("userId"));
                Assert.That(ex.Message, Is.StringStarting("must be positive"));
            }

            [Test]
            public void ValidRequestReturnsSimilarity()
            {
                var expected = new List<Recommendation> { new Recommendation { ItemId = 1, Value = 2.5m } };
                var response = A.Fake<RestResponse<List<Recommendation>>>();
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = expected;
                A.CallTo(() => this.client.Execute<List<Recommendation>>(A<RestRequest>._)).Returns(response);

                Assert.That(sdk.GetRecommendations(1), Is.EqualTo(expected));
            }

            [Test]
            public void MissingUserThrowsError()
            {
                var response = A.Fake<RestResponse<List<Recommendation>>>();
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                A.CallTo(() => this.client.Execute<List<Recommendation>>(A<RestRequest>._)).Returns(response);

                Assert.Throws<Exception>(() => sdk.GetRecommendations(1));
            }
        }
    }
}
