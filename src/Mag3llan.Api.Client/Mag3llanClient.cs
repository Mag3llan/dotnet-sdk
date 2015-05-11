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
        public void SetPreference(long userId, long itemId, double value, string comments = null, bool force = false)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");
            if (itemId < 0) throw new ArgumentOutOfRangeException("itemId", "must be positive");

            var preference = new Preference(userId, itemId, value, comments);

            var request = new RestRequest("preference", Method.PUT);
            request.AddBody(preference);

            var response = this.client.Execute(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                case System.Net.HttpStatusCode.Created:
                    break;
                default:
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

        public bool DeleteUser(long userId)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");

            var request = new RestRequest("user/" + userId, Method.DELETE);

            var response = this.client.Execute(request);

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public List<long> GetPlu(long userId, bool force = false)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");

            var request = new RestRequest("plu/" + userId + "?force=" + force.ToString(), Method.GET);

            var response = this.client.Execute<List<long>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(response.ErrorMessage);

            return response.Data;
        }
    }
}
