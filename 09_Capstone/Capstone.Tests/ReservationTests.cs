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
    public class ReservationTests
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

        //int makeReservation(Site site, string reserversName, DateTime startDate, DateTime endDate)

        [TestMethod]
        public void makeReservationTest()
        {
            string Camp = "Camp Blue";
            DateTime StartDate = new DateTime(2020, 1, 2);
            DateTime EndDate = new DateTime(2020, 1, 3);
            CampsiteDAO campsiteDAO = new CampsiteDAO(connectionString);
            IList<Site> sites = campsiteDAO.GetAvailableSitesOnCampground(Camp, StartDate, EndDate);
            ReservationDAO reservationDAO = new ReservationDAO(connectionString);
            int confirmationNo = reservationDAO.makeReservation(sites[0], "John Brown", StartDate, EndDate);
            
            Assert.IsTrue(confirmationNo > newreservation_id);
        }

    }
}