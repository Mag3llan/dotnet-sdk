using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag3llan.Api.Client
{
    public class Mag3llanClient
    {
        private RestClient client = new RestClient();
        public Mag3llanClient(string hostname, string key)
        {
            if (string.IsNullOrWhiteSpace(hostname)) throw new ArgumentNullException("hostname");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            this.client.BaseUrl = new Uri(string.Format("http://{0}/api/", hostname));
            this.client.AddDefaultHeader("content-type", "application/json");
            this.client.AddDefaultHeader("Access_Token", key);
        }

        /// <summary>
        /// Create a Preference on an item for a user
        /// </summary>
        /// <param name="userId">User Identifier, must be positive</param>
        /// <param name="itemId">Item Identifier, must be positive</param>
        /// <param name="value">score, can be positive or negative</param>
        public void SetPreference(long userId, long itemId, double value)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");
            if (itemId < 0) throw new ArgumentOutOfRangeException("itemId", "must be positive");

            var preference = new Preference(userId, itemId, value);

            var request = new RestRequest("preference", Method.POST);
            request.AddBody(preference);

            var repsonse = this.client.Execute(request);
        }

    }
}
