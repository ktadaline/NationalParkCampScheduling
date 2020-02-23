using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundDAO : ICampgroundDAO
    {
        private string connectionString;

        public CampgroundDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Campground> GetCampgrounds(Park park)
        {
            string parkName = park.parkName;

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
                        campground.openFromDate = Convert.ToInt32(rdr["open_from_mm"]);
                        campground.openToDate = Convert.ToInt32(rdr["open_to_mm"]);
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

        public List<string> campgroundsToString(IList<Campground> campgrounds)
        {
            List<string> campgroundStrings = new List<string>();
            Dictionary<int, string> months = new Dictionary<int, string>()
            {
                {1, "January"},
                {2, "February"},
                {3, "March"},
                {4, "April"},
                {5, "May"},
                {6, "June"},
                {7, "July"},
                {8, "August"},
                {9, "September"},
                {10, "October"},
                {11, "November"},
                {12, "December"}
            };

            int count = 1;
            foreach (Campground campground in campgrounds)
            {
                //string cg = count.ToString() + campground.campgroundName + months[campground.openFromDate] + months[campground.openToDate] + campground.dailyFee.ToString("C");
                //string cg = campground.campgroundName + "\t" + months[campground.openFromDate] + "\t" + months[campground.openToDate] + "\t" + campground.dailyFee.ToString("C");
                string cg = $"{campground.campgroundName,-30} {months[campground.openFromDate],-15} {months[campground.openToDate],-15} {campground.dailyFee.ToString("C"),-15}";
                //display += $"\t\t\t\t{product.SlotLocation,-5} {product.ProductName, -20} {product.Price.ToString("C"), -13} {product.QuantityLeft.ToString(), -20} \n";

                campgroundStrings.Add(cg);

                count++;
            }
            return campgroundStrings;
        }

    }
}
