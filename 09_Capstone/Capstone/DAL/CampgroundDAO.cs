using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    class CampgroundDAO : ICampgroundDAO
    {
        private string connectionString;

        public CampgroundDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Campground> GetCampgrounds(string parkName)
        {
            List<Campground> campgrounds = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM campground Join park on park.park_id =campground.park_id WHERE park.name = @parkName;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@parkName", parkName);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Campground campground = new Campground();
                        campground.parkId = Convert.ToInt32(rdr["park_id"]);
                        campground.campgroundName = Convert.ToString(rdr["name"]);
                        campground.openFromDate = Convert.ToDateTime(rdr["open_from_mm"]);
                        campground.openToDate= Convert.ToDateTime(rdr["open_to_mm"]);
                        campground.dailyFee = Convert.ToDecimal(rdr["daily_fee"]);
                   

                        campgrounds.Add(campground);


                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return campgrounds;
        }
    }
}
