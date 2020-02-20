using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    class ReservationDAO : IReservationDAO
    {
        private string connectionString;

        public ReservationDAO (string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
    }
}
