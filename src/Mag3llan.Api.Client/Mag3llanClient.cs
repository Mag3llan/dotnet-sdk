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
        private IRestClient client;

        public Mag3llanClient(string hostname, string key)
            : this(new RestClient(), hostname, key)
        {
        }

        internal Mag3llanClient(IRestClient client, string hostname, string key)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (string.IsNullOrWhiteSpace(hostname)) throw new ArgumentNullException("hostname");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            this.client = client;
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

            var response = this.client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }
        }

        public bool DeletePreference(long userId, long itemId)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");
            if (itemId < 0) throw new ArgumentOutOfRangeException("itemId", "must be positive");

            var request = new RestRequest("preference/" + userId + "/" + itemId, Method.DELETE);

            var response = this.client.Execute(request);

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

    }
}
