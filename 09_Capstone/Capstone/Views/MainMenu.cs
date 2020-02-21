using Capstone.DAL;
using System;
using System.Collections.Generic;
using Capstone;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class MainMenu : CLIMenu
    {
        // DAOs - Interfaces to our data objects can be stored here...
        //protected ICityDAO cityDAO;
        //protected ICountryDAO countryDAO;
        protected ICampgroundDAO campgroundDAO;
        protected ICampsiteDAO campsiteDAO;
        protected IParkDAO parkDAO;
        protected IReservationDAO reservationDAO;


        /// <summary>
        /// Constructor adds items to the top-level menu. YOu will likely have parameters for one or more DAO's here...
        /// </summary>
        public MainMenu(ICampgroundDAO campgroundDAO, ICampsiteDAO campsiteDAO, IParkDAO parkDAO, IReservationDAO reservationDAO) : base("Main Menu")
        {
            this.campgroundDAO = campgroundDAO;
            this.campsiteDAO = campsiteDAO;
            this.parkDAO = parkDAO;
            this.reservationDAO = reservationDAO;

            //this.cityDAO = cityDAO;
            //this.countryDAO = countryDAO;
        }

        protected override void SetMenuOptions()
        {
            List<Park> viewParks = parkDAO.GetParks();
            int Count = 1;
            //List<string> displayParks = parkDAO.DisplayParkList(viewParks);
            foreach (Park park in viewParks)
            {
                menuOptions.Add(Count.ToString(), park.parkName); ;
                Count++;
            }
            //this.menuOptions.Add("1", "View Available Parks");
            //this.menuOptions.Add("2", "View Campgrounds at Park");
            //this.menuOptions.Add("3", "Go to a sub-menu");
            this.menuOptions.Add("Q", "Quit program");
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {

            List<Park> parkSelection = parkDAO.GetParks();
            
            int inputNum = int.Parse(choice);
            Park park = parkSelection[inputNum - 1];

           // string displayParks = parkDAO.DisplayParkDetails(parkSelection[inputNum - 1]);
            //Console.WriteLine(displayParks);
            ParkInfoMenu sm = new ParkInfoMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO, park);
            sm.Run();
            return true;

            //Pause("");
     
            //switch (choice)
            //{
            //    case "1": // Do whatever option 1 is
            //        //int i1 = GetInteger("Enter the first integer: ");
            //        //int i2 = GetInteger("Enter the second integer: ");
            //        //Console.WriteLine($"{i1} + {i2} = {i1+i2}");

            //        //Console.WriteLine("Select a Park for Further Details:");
            //        //DisplayParkList(GetParks());
            //        Console.WriteLine();
            //        //parkDAO.GetParks();
            //            //campgroundDAO.GetCampgrounds("Arches");
            //        viewParks();
            //        viewParkDetails();
            //        Pause("Press enter to continue");
            //        return true;    // Keep running the main menu
            //    case "2": // Do whatever option 2 is

            //        viewParks();
            //        Console.WriteLine("Would you like to learn more about parks before making your selection?(Y/N)");
            //        string input = Console.ReadLine();
            //        input = input.ToUpper();
            //        if (input.StartsWith("Y"))
            //        {
            //            viewParkDetails();
            //            Pause("Press enter to continue");
            //            viewParks();
            //        }
            //        getCampgrounds();

            //        Console.WriteLine("Would you like to view reservation menu");
            //        string input2 = Console.ReadLine();
            //        input = input2.ToUpper();
            //        if (input2.StartsWith("Y"))
            //        {
            //            SubMenu1 sm1 = new SubMenu1();
            //            sm1.Run();
            //            return true;
            //            //viewParkDetails();
            //            //Pause("Press enter to continue");
            //            //viewParks();
            //        }

            //        //WriteError("Not yet implemented");
            //        Pause("");
            //        return true;    // Keep running the main menu
            //    case "3": // Create and show the sub-menu
            //        SubMenu1 sm = new SubMenu1();
            //        sm.Run();
            //        return true;    // Keep running the main menu
            //}

        }

        public void viewParks()
        {
            //Console.WriteLine("test");
            List<Park> viewParks = parkDAO.GetParks();
            List<string> displayParks = parkDAO.DisplayParkList(viewParks);
            foreach (string park in displayParks)
            {
                Console.WriteLine(park);
            }
        }

        public void viewParkDetails()
        {

            List<Park> parkSelection = parkDAO.GetParks();
            Console.WriteLine("Which Park would you like to learn more about?");
            string input = Console.ReadLine();
            int inputNum = int.Parse(input);
            string displayParks = parkDAO.DisplayParkDetails(parkSelection[inputNum - 1]);
            Console.WriteLine(displayParks);
        }

        

        public void getCampgrounds()
        {
            List<Park> parkSelection = parkDAO.GetParks();
            Console.WriteLine("Which park would you like to view the campgrounds on?");
            string input = Console.ReadLine();
            int inputNum = int.Parse(input);
            Park park = parkSelection[inputNum-1];

            IList<Campground> listCG = campgroundDAO.GetCampgrounds(park);
            List<string> listCGstrings = campgroundDAO.campgroundsToString(listCG);

            foreach (string line in listCGstrings)
            {
                Console.WriteLine(line);
            }


        }


        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }


        private void PrintHeader()
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("My Program"));
            ResetColor();
        }
    }
}
