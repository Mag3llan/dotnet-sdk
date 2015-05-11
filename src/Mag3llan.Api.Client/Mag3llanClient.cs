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
            if (hostname.Contains("http")) throw new ArgumentException("please specify only the hostname", "hostname");
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

        public List<long> GetPlu(long userId, decimal threshold = -1)
        {
            return this.GetPlu(userId, 0, threshold);
        }

        public List<long> GetPlu(long userId, long itemId, decimal threshold = -1)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");
            if (itemId < 0) throw new ArgumentOutOfRangeException("itemId", "must be positive");
            if (threshold < -1 || threshold > 1) throw new ArgumentOutOfRangeException("threshold", "must be between -1 and 1");

            var url = "plu/" + userId +
                (itemId != 0 ? "/rating/" + itemId : string.Empty) +
                (threshold > -1 ? "?threshold=" + threshold : string.Empty);

            var request = new RestRequest(url, Method.GET);

            var response = this.client.Execute<List<long>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(response.ErrorMessage);

            return response.Data;
        }

        public decimal GetSimilarity(long userId, long otherUserId, bool force = false)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");
            if (otherUserId < 0) throw new ArgumentOutOfRangeException("otherUserId", "must be positive");

            var request = new RestRequest("similarity/" + userId + "/" + otherUserId + "?force=" + force, Method.GET);

            var response = this.client.Execute<decimal>(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return response.Data;
                case System.Net.HttpStatusCode.NotFound:
                    throw new ArgumentException("user not found");
                default:
                    throw new Exception(response.ErrorMessage);
            }
        }

        public List<Overlap> GetOverlaps(long userId, long otherUserId)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");
            if (otherUserId < 0) throw new ArgumentOutOfRangeException("otherUserId", "must be positive");

            var request = new RestRequest("overlaps/" + userId + "/" + otherUserId, Method.GET);

            var response = this.client.Execute<List<Overlap>>(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return response.Data;
                case System.Net.HttpStatusCode.NotFound:
                    throw new ArgumentException("user not found");
                default:
                    throw new Exception(response.ErrorMessage);
            }
        }

        public List<Recommendation> GetRecommendations(long userId)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");

            var request = new RestRequest("recommendation/" + userId, Method.GET);

            var response = this.client.Execute<List<Recommendation>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return response.Data;

            throw new Exception(response.ErrorMessage);
        }
    }
}
