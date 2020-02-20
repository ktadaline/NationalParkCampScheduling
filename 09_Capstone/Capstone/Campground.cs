using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    class Campground
    {
        int campgroundId { get; set; }
        int parkId { get; set; }

        string campgroundName { get; set; }

        int openFromDate { get; set; }//should this be an int? 

        int openToDate { get; set; }

        decimal dailyFee { get; set; }

    }
}
