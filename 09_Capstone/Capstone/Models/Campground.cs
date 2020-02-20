using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Campground
    {
        public int campgroundId { get; set; }
        public int parkId { get; set; }

        public string campgroundName { get; set; }

        public DateTime openFromDate { get; set; }

        public DateTime openToDate { get; set; }

        public decimal dailyFee { get; set; }

    }
}
