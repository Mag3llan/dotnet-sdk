using Mag3llan.Api.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag3llan.Api.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private Mag3llanClient sdk = new Mag3llanClient("api.mag3llan.com", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6Im1vdjN5IiwiRW1haWwiOiJkZXZvcHNAbWFnM2xsYW4uY29tIn0.g5i5fSyXjIJTMzSzqLrAMB-mPPlG-msfpFe5MHQKvTg");

        [Test]
        public void GetPluForValidUser()
        {
            var actual = this.sdk.GetPlu(2, 0.8m);

            Assert.That(actual.Count, Is.Not.EqualTo(0));
        }
    }
}
