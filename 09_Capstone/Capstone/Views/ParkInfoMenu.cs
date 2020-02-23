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
        override public string SelectionText 
        {
            get { return $"\r\nSelect one of the following campgrounds in {park.parkName} Park to view its available campsites: \n\n{displayHeadingText()}"; }
        }


        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ParkInfoMenu(ICampgroundDAO campgroundDAO, ICampsiteDAO campsiteDAO, IParkDAO parkDAO, IReservationDAO reservationDAO, Park park) :
            base("ParkInfoMenu")
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

            IList<Campground> listCG = campgroundDAO.GetCampgrounds(park);

            List<string> listCGstrings = campgroundDAO.campgroundsToString(listCG);

      
            int count = 1;
            foreach (Campground s in listCG)
            {
                menuOptions.Add(count.ToString(), listCGstrings[count-1]);
                count++;
            }

            //Campground campground = listCG[inputNum - 1];


            //this.menuOptions.Add("1", "View Campgrounds");
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
            
        //    List<string> listCGstrings = campgroundDAO.campgroundsToString(listCG);

        //    Console.WriteLine("Select a campground:");
        //    foreach (string s in listCGstrings)
        //    {
        //        Console.WriteLine(s);
        //    }

            //choice = Console.ReadLine();
            int inputNum = int.Parse(choice);
            Campground campground = listCG[inputNum - 1];


            //Pause("");
            Console.Write($" Press Enter to see campsite availablity for {campground.campgroundName}...");
            Console.ReadLine();


            ParkCampgroundsMenu pgm2 = new ParkCampgroundsMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO, campground);
            pgm2.Run();

            return true;
        }

        //display campground settings

        private string displayHeadingText()
        {
            string campgroundNameCategory = $"Campground:";
            string openFromCategory = "Open From:";
            string openToCategory = "Open To:";
            string dailyFeeCategory = "Daily Fee:";

            string campgrounddisplay = $"    {campgroundNameCategory,-30} {openFromCategory,-15} {openToCategory,-15} {dailyFeeCategory,-15}";
            return campgrounddisplay;
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
            //Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            Console.WriteLine();
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"{park.parkName} Park"));
            //Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Campgrounds"));
            //Figgle.FiggleFonts.Pebbles
            //Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"of {park.parkName} Park"));

            ResetColor();
        }

    }
}
