using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag3llan.Api.Client
{
    public class Preference
    {
        public Preference(long userId, long itemId, double value, string comments = null)
        {
            this.UserId = userId;
            this.ItemId = itemId;
            this.Value = value;
            this.Comments = comments;
        }

        public long UserId { get; set; }
        public long ItemId { get; set; }
        public double Value { get; set; }
        public string Comments { get; set; }
    }
}
