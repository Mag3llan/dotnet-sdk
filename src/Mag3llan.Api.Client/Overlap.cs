using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mag3llan.Api.Client
{
    public class Overlap
    {
        public long ItemId { get; set; }
        public decimal Rating { get; set; }
        public decimal OtherRating { get; set; }
        public decimal Delta { get; set; }
    }
}
