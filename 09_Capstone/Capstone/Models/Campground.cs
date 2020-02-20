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

        public int openFromDate { get; set; }

        public int openToDate { get; set; }

        public decimal dailyFee { get; set; }

    }
}
