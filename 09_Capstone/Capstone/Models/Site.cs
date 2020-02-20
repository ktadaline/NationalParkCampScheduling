using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Site
    {
        public int siteId { get; set; }

        public int campgroundId { get; set; }

        public int siteNumber { get; set; }

        public int maxOccupancy { get; set; }

        public bool accessible { get; set; }

        public int maxRvLength { get; set; }

        public bool utilities { get; set; }
    }
}


