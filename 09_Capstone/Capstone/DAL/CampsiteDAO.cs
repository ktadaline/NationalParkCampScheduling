using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampsiteDAO : ICampsiteDAO
    {
        private string connectionString;

        public CampsiteDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetAvailableSitesOnCampground(string campgroundName, DateTime startDate, DateTime endDate)
        {
            List<Site> campsites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"SELECT *
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
                        site.siteId = Convert.ToInt32(rdr["site_id"]);
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

        public List<string> GetTop5Campsites(IList<Site> allsites, DateTime startDate, DateTime endDate)
        {
            string isAccessible = "";
            string maxRV = "";
            string utilities = "";
            List<string> Top5CampsitesString = new List<string>();
            foreach (Site site in allsites)
            {
                if (site.accessible)
                {
                    isAccessible = "Yes";
                }
                else
                {
                    isAccessible = "No";
                }

                if (site.maxRvLength == 0)
                {
                    maxRV = "N/A";
                }
                else
                {
                    maxRV = site.maxRvLength.ToString();
                }
                if (site.utilities)
                {
                    utilities = "Yes";
                }
                else
                {
                    utilities = "N/A";
                }

                string  s = $"{site.siteNumber.ToString(),-15}{site.maxOccupancy.ToString(),-15}{isAccessible,-15}{maxRV,-15}{utilities,-15}{GetPriceOfStay(site, startDate, endDate).ToString("C"), -15}";

                //string s = $"Site number: {site.siteNumber.ToString(),-30}MaxOccupancy: {site.maxOccupancy.ToString(),-15}Is Accessible: {isAccessible,-15}Max RV: {maxRV,-15}Utilities: {utilities,-15}Total Price: {GetPriceOfStay(site, startDate, endDate).ToString("C"),-15}";

                //string s = "Site number: " +site.siteNumber.ToString()+ " MaxOccupancy: " + site.maxOccupancy.ToString() + " Is Accessible: " + isAccessible + " Max RV: " +maxRV + " Utilities:  " + utilities + " Total Price: " + GetPriceOfStay(site, startDate, endDate).ToString("C");

                Top5CampsitesString.Add(s);

            }

            if (Top5CampsitesString.Count >= 5)
            {
                return Top5CampsitesString.GetRange(0, 5);
            }
            else
            {
                int n = Top5CampsitesString.Count;
                return Top5CampsitesString.GetRange(0, n);
            }

        }

        public decimal GetPriceOfStay(Site site, DateTime startDate, DateTime endDate)
        {
            decimal priceOfStay = 0M;
            //TimeSpan durationOfStay = endDate - startDate;
            int duration = (int)(endDate - startDate).TotalDays;

            int durationOfStayNum = Convert.ToInt32(duration);

            if (durationOfStayNum <= 0)
            {
                throw new InvalidDateSelectionException("Can't select zero or negative days");
            }
            

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
                        dailyFee = Convert.ToDecimal(rdr["daily_fee"]);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            priceOfStay = (decimal) durationOfStayNum * dailyFee;

            //priceOfStay = dailyFee;


            return priceOfStay;
        }
        //public string campgroundIdToName(int campgroundId)
        //{
        //    string cn = "";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            string sql = "SELECT Top1 campground.name FROM site Join Campground on Site.campground_id = campground.campground_id  WHERE site.campground_id=@campgroundId;";

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

        //            SqlDataReader rdr = cmd.ExecuteReader();

        //            while (rdr.Read())
        //            {
        //                cn = Convert.ToString(rdr["campground.name"]);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return cn;
        //}
    }
}
