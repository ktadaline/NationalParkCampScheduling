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
            string expectedValue = "Site number: 2 MaxOccupancy: 6 Is Accessible: Yes Max RV: 25 Utilities: Yes Total Price: 100.0000";
            StringAssert.Equals(expectedValue, top5[0]);
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

        //   public string campgroundIdToName(int campgroundId)

        
        //[TestMethod]
        //public void AssignEmployeeToProjectTest()
        //{
        //    ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);

        //    bool employeeIsAssigned = dao.AssignEmployeeToProject(newproject_id, newemployee_id);

        //    Assert.AreEqual(true, employeeIsAssigned);
        //}

        ////RemoveEmployeeFromProject

        //[TestMethod]
        //public void RemoveEmployeeFromProjectTest()
        //{
        //    ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);

        //    bool employeeIsRemoved = dao.RemoveEmployeeFromProject(newproject_id, newemployee_id);

        //    Assert.AreEqual(true, employeeIsRemoved);
        //}

        //[TestMethod]
        //public void CreateProjectTest()
        //{
        //    ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);

        //    Project newproject = new Project()
        //    {
        //        Name = "So so project",
        //        StartDate = new DateTime(2016, 7, 15),
        //        EndDate = new DateTime(2018, 7, 15),
        //    };

        //    int projectid = dao.CreateProject(newproject);

        //    bool isAdded = projectid > newproject_id;

        //    Assert.IsTrue(isAdded);
        //}
        ////add count test later
        //[TestMethod]
        //public void GetNumberOfDepartmentsTest()
        //{
        //    DepartmentSqlDAO dao = new DepartmentSqlDAO(connectionString);

        //    IList<Department> departments = dao.GetDepartments();
        //    Assert.AreEqual(3, departments.Count);
        //}

        //[TestMethod]
        //public void CreateDepartmentTest()
        //{
        //    DepartmentSqlDAO dao = new DepartmentSqlDAO(connectionString);

        //    Department department = new Department()
        //    {
        //        Name = "Transponders Department",
        //    };

        //    int departmentId = dao.CreateDepartment(department);

        //    bool isAdded = departmentId > newdepartment_id;

        //    Assert.IsTrue(isAdded);
        //}

        //[TestMethod]

        //public void UpdateDepartmentTest()
        //{
        //    DepartmentSqlDAO dao = new DepartmentSqlDAO(connectionString);

        //    Department newdepartment = new Department()
        //    {
        //        Name = "xyz",
        //    };

        //    bool departmentIsUpdated = dao.UpdateDepartment(newdepartment);

        //    Assert.AreEqual(true, departmentIsUpdated);

        //}
        //[TestMethod]
        //public void GetNumberOfEmployeesTest()
        //{
        //    EmployeeSqlDAO dao = new EmployeeSqlDAO(connectionString);

        //    IList<Employee> employees = dao.GetAllEmployees();
        //    Assert.AreEqual(3, employees.Count);
        //}

        //[TestMethod]

        //public void EmployeeSearchResultNumberTest()
        //{

        //    EmployeeSqlDAO dao = new EmployeeSqlDAO(connectionString);

        //    IList<Employee> employees = dao.Search("Rachel", "H");
        //    Assert.AreEqual(1, employees.Count);


        //}
        //[TestMethod]
        //public void EmployeesWithoutProjectsTests()
        //{

        //    EmployeeSqlDAO dao = new EmployeeSqlDAO(connectionString);

        //    IList<Employee> employees = dao.GetEmployeesWithoutProjects();
        //    Assert.AreEqual(0, employees.Count);


        //}
    }
}
