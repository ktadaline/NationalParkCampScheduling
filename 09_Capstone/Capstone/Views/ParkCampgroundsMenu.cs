using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Views
{

    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class ParkCampgroundsMenu : CLIMenu
    {
        // Store any private variables, including DAOs here....
        protected ICampgroundDAO campgroundDAO;
        protected ICampsiteDAO campsiteDAO;
        protected IParkDAO parkDAO;
        protected IReservationDAO reservationDAO;
        private Campground campground;


        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ParkCampgroundsMenu(ICampgroundDAO campgroundDAO, ICampsiteDAO campsiteDAO, IParkDAO parkDAO, IReservationDAO reservationDAO, Campground campground) :
            base("Sub-Menu 1")
        {
            // Store any values or DAOs passed in....
            this.campgroundDAO = campgroundDAO;
            this.campsiteDAO = campsiteDAO;
            this.parkDAO = parkDAO;
            this.reservationDAO = reservationDAO;
            this.campground = campground;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "View Available Campsites");
            //this.menuOptions.Add("2", "Do Option 2 and return to Main");
            this.menuOptions.Add("B", "Back to Main Menu");
            this.quitKey = "B";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {




            Console.WriteLine("What is your desired arrival date?");
            string input = Console.ReadLine();

            DateTime arrivalDate = DateTime.Parse(input);

            Console.WriteLine("What is your desired departure date?");

            input = Console.ReadLine();

            DateTime departureDate = DateTime.Parse(input);

            IList<Site> siteList = campsiteDAO.GetAvailableSitesOnCampground(campground.campgroundName, arrivalDate, departureDate);

            List<string> top5Camp = campsiteDAO.GetTop5Campsites(siteList, arrivalDate, departureDate);
            foreach (string top in top5Camp)
            {
                Console.WriteLine(top);
            }

            Pause("");
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();


            //string displayParks = campgroundDAO.Display(campground);
            Console.WriteLine("Add campground info here later");
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Sub-Menu 1"));
            ResetColor();
        }
    }
}
