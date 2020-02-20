using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Reservation
    {

        public int reservationId { get; set; }

        public int siteId { get; set; }

        public string reservationName { get; set; }

        public DateTime fromDate { get; set; }

        public DateTime createDate { get; set; }
    }
}
