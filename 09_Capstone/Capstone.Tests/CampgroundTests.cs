using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=npcampground;Trusted_Connection=True;";
        private int newpark_id;
        private int newcampground_id;
        private int newsite_id;
        private int newreservation_id;


        //SELECT @newpark_id as newpark_id, @newcampground_id as newcampground_id, @newsite_id as newsite_id, @newreservation_id as newreservation_id

        [TestInitialize]
        public void SetupDatabase()
        {
            transaction = new TransactionScope(); //same as starting transaction
            string setUpSql;
            string path = "setup.sql";
            using (StreamReader rdr = new StreamReader(path))
            {
                setUpSql = rdr.ReadToEnd();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(setUpSql, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    newpark_id = Convert.ToInt32(rdr["newpark_id"]);
                    newcampground_id = Convert.ToInt32(rdr["newcampground_id"]);
                    newsite_id = Convert.ToInt32(rdr["newsite_id"]);
                    newreservation_id = Convert.ToInt32(rdr["newreservation_id"]);
                }
            }
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            transaction.Dispose();
        }


        [TestMethod]
        public void GetCampgroundsTest()
        {
            ParkDAO parkDAO = new ParkDAO(connectionString);
            CampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);


            List<Park> parks = parkDAO.GetParks();
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(parks[0]);

            Assert.AreEqual(1, campgrounds.Count);
        }

        //List<string> campgroundsToString(IList<Campground> campgrounds)

        [TestMethod]
        public void CampgroundsToStringCountTest()
        {
            ParkDAO parkDAO = new ParkDAO(connectionString);
            CampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);


            List<Park> parks = parkDAO.GetParks();
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(parks[0]);
            List<string> campground = campgroundDAO.campgroundsToString(campgrounds);

            Assert.AreEqual(1, campground.Count);
        }


        [TestMethod]
        public void CampgroundsToStringTest()
        {
            ParkDAO parkDAO = new ParkDAO(connectionString);
            CampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);

            List<Park> parks = parkDAO.GetParks();
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(parks[0]);
            List<string> campground = campgroundDAO.campgroundsToString(campgrounds);
            string campName = "Camp Blue";
            string openMonth = "April";
            string closeMonth = "November";
            string price = "$10.00";

            string cg = $"{campName,-30} {openMonth,-15} {closeMonth,-15} {price,-15}";


            Assert.AreEqual(cg, campground[0]);
        }

    }
}
