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
    public class CampsiteTests
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
        public void GetAvailableSitesOnCampgroundTest()
        {
            string Camp = "Camp Blue";
            DateTime StartDate = new DateTime(2020, 1, 2);
            DateTime EndDate = new DateTime(2020, 1, 3);
            CampsiteDAO campsiteDAO = new CampsiteDAO(connectionString);
            IList<Site> sites = campsiteDAO.GetAvailableSitesOnCampground(Camp, StartDate, EndDate);

            Assert.AreEqual(1, sites.Count);
        }

        [TestMethod]
        public void GetTop5CampsitesTest()
        {
            CampsiteDAO campsiteDAO = new CampsiteDAO(connectionString);
            string Camp = "Camp Red";
            DateTime StartDate = new DateTime(2020, 3, 2);
            DateTime EndDate = new DateTime(2020, 3, 3);
            IList<Site> sites = campsiteDAO.GetAvailableSitesOnCampground(Camp, StartDate, EndDate);
            List<string> top5 = campsiteDAO.GetTop5Campsites(sites, StartDate, EndDate);
            //string s = "Site number: " + "2" + " MaxOccupancy: " + "6" + " Is Accessible: " + "Yes" + " Max RV: " + "25" + " Utilities:  " + "Yes" + " Total Price: " + "$100.00";

            string siteNumber = "2";
            string maxOccupancy = "6";
            string isAccessible = "Yes";
            string maxRV = "25";
            string utilities = "Yes";
            string totalPrice = "$100.00";

            string s = $"{siteNumber,-15}{maxOccupancy,-15}{isAccessible,-15}{maxRV,-15}{utilities,-15}{totalPrice,-15}";

            Assert.AreEqual(s, top5[0]);
         
        }

        //decimal GetPriceOfStay(Site site, DateTime startDate, DateTime endDate)

        [TestMethod]
        public void GetPriceOfStayTest()
        {
            CampsiteDAO campsiteDAO = new CampsiteDAO(connectionString);
            string Camp = "Camp Red";
            DateTime StartDate = new DateTime(2020, 3, 2);
            DateTime EndDate = new DateTime(2020, 3, 4);
            IList<Site> sites = campsiteDAO.GetAvailableSitesOnCampground(Camp, StartDate, EndDate);
            Site s = sites[0];
            decimal price = campsiteDAO.GetPriceOfStay(s, StartDate, EndDate);
            Assert.AreEqual(price, 200M);

        }

    }
}
