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

        public Mag3llanClient(string uri, string key)
        {
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException("uri");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            var client = new RestClient();
            client.BaseUrl = new Uri(uri);
            client.AddDefaultHeader("Access_Token", key);
        }

        public void SetPreference(long userId, long itemId, double value)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException("userId", "must be positive");

            var preference = new Preference(userId, itemId, value);


        }

    }
}
