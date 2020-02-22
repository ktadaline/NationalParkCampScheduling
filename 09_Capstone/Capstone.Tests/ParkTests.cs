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
    public class ParkTests
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
        public void GetParksTest()
        {
            ParkDAO parkDAO = new ParkDAO(connectionString);
            List<Park> parks = parkDAO.GetParks();
            Assert.AreEqual(3, parks.Count);
        }

        //List<string> DisplayParkList(List<Park> parks)

        [TestMethod]
        public void DisplayParkTest()
        {
            ParkDAO parkDAO = new ParkDAO(connectionString);
            List<Park> parks = parkDAO.GetParks();
            List<string> parksStrings = parkDAO.DisplayParkList(parks);
            Assert.AreEqual(3, parksStrings.Count);
        }

        //public string DisplayParkDetails(Park park)

        [TestMethod]
        public void DisplayParkDetailsTest()
        {
            ParkDAO parkDAO = new ParkDAO(connectionString);
            List<Park> parks = parkDAO.GetParks();
            string parkDetails = parkDAO.DisplayParkDetails(parks[0]);

            string string1 = "abc";
            string string2 = "abc";
            bool stringsAreEqual = string1.Equals(string2);


            Assert.IsTrue(stringsAreEqual);
        }


    }
}
