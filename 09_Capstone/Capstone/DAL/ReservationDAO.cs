using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public int makeReservation(Site site, string reserversName, DateTime startDate, DateTime endDate)
        {
            int reservationId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"Insert into reservation
(site_id, name, from_date, to_date, create_date)
VALUES
(@siteId, @reserversName, @startDate, @endDate, CURRENT_TIMESTAMP); 
SELECT @@identity;
";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteId", site.siteId);
                    cmd.Parameters.AddWithValue("@reserversName", reserversName);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);


                    reservationId = Convert.ToInt32(cmd.ExecuteScalar());

                    


                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reservationId;
        }

    }
}
