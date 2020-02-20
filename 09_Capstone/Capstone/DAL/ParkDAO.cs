using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone;

namespace Capstone.DAL
{
    class ParkDAO : IParkDAO
    {

        private string connectionString;

        public ParkDAO (string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park>GetParks()
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM park ORDER BY park.Name";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while(rdr.Read())
                    {
                        Park park = new Park();
                        park.parkName = Convert.ToString(rdr["name"]);
                        park.location = Convert.ToString(rdr["location"]);
                        park.establishDate = Convert.ToDateTime(rdr["establish_date"]);
                        park.area = Convert.ToInt32(rdr["area"]);
                        park.visitors = Convert.ToInt32(rdr["visitors"]);
                        park.description = Convert.ToString(rdr["description"]);

                        parks.Add(park);


                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return parks;
        }


    }
}
