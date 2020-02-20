using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public interface IReservationDAO
    {
        int makeReservation(Site site, string reserversName, DateTime startDate, DateTime endDate);
    }
}
