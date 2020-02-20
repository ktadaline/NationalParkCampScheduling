using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    class CampsiteDAO :ICampsiteDAO
    {
        private string connectionString;

        public CampsiteDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetCampsites(string campgroundName, DateTime startDate, DateTime endDate)
        {
            List<Site> campsites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"SELECT site.site_number
FROM SITE
JOIN campground 
   ON site.campground_id = campground.campground_id
WHERE campground.name = @campgroundName 
   AND NOT EXISTS 
   (SELECT site.site_id 
   FROM reservation
   WHERE 
   ((@startDate <= reservation.to_date AND @endDate >= reservation.from_date)
   AND reservation.site_id = site.site_id))";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Site site = new Site();
                        site.campgroundId = Convert.ToInt32(rdr["campground_id"]);
                        site.siteNumber = Convert.ToInt32(rdr["site_number"]);
                        site.maxOccupancy = Convert.ToInt32(rdr["max_occupancy"]);
                        site.accessible = Convert.ToBoolean(rdr["accessible"]);
                        site.maxRvLength = Convert.ToInt32(rdr["max_rv_length"]);
                        site.utilities = Convert.ToBoolean(rdr["utilities"]);

                        campsites.Add(site);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return campsites;
        }

        public List<Site> GetTop5Campsites(List<Site> allsites)
        {

            List<Site> Top5Campsites = new List<Site>();
            Top5Campsites =  allsites.GetRange(0, 5);
            return Top5Campsites;

        }

        public decimal GetPriceOfStay(Site site, DateTime startDate, DateTime endDate)
        {
            decimal priceOfStay = 0M;
            TimeSpan durationOfStay = endDate - startDate;
            int durationOfStayNum = Convert.ToInt32(durationOfStay);
            decimal dailyFee = 0M;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"SELECT campground.daily_fee
FROM Site
JOIN campground ON site.campground_id = campground.campground_id
WHERE site.site_id = @siteId and campground.campground_id = site.campground_id
";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteId", site.siteId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        dailyFee = Convert.ToDecimal(rdr["campground.daily_fee"]);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            priceOfStay = durationOfStayNum * dailyFee;
            return priceOfStay;
        }

    }
}
