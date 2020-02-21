using Capstone.DAL;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class ParkInfoMenu : CLIMenu
    {
        // Store any private variables, including DAOs here....
        protected ICampgroundDAO campgroundDAO;
        protected ICampsiteDAO campsiteDAO;
        protected IParkDAO parkDAO;
        protected IReservationDAO reservationDAO;
        private Park park;


        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ParkInfoMenu(ICampgroundDAO campgroundDAO, ICampsiteDAO campsiteDAO, IParkDAO parkDAO, IReservationDAO reservationDAO, Park park) :
            base("Sub-Menu 1")
        {
            // Store any values or DAOs passed in....
            this.campgroundDAO = campgroundDAO;
            this.campsiteDAO = campsiteDAO;
            this.parkDAO = parkDAO;
            this.reservationDAO = reservationDAO;
            this.park = park;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "View Campgrounds");
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
            IList<Campground> listCG = campgroundDAO.GetCampgrounds(park);
            
            List<string> listCGstrings = campgroundDAO.campgroundsToString(listCG);
            foreach (string s in listCGstrings)
            {
                Console.WriteLine();
            }
            Pause("");

            int inputNum = int.Parse(choice);
            Campground campground = listCG[inputNum - 1];

            ParkCampgroundsMenu pgm = new ParkCampgroundsMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO, campground);
            pgm.Run();

            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
            string displayParks = parkDAO.DisplayParkDetails(park);
            Console.WriteLine(displayParks);   
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
