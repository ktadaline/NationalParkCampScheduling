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
        private List<string> top5Camp;
        IList<Site> siteList;
        DateTime arrivalDate;
        DateTime departureDate;

        //display setting
        override public string SelectionText
        {
            get 
            {
                return $"\r\nPlease select a campsite to reserve:\n\n{displayHeadingText()}";
            }

        }
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
            Console.Clear();
            PrintHeader();
            InitializeSetupInfo();
            this.menuOptions.Add("B", "Back to Previous Menu");
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
            int inputNum = int.Parse(choice);
            Site site = new Site();

            for (int i = 1; i <= siteList.Count; i++) 
            {
                if (i == inputNum)
                {
                    site = siteList[i - 1];
                }
            }

            Console.Write($" Press Enter to complete reservation on Site Number {site.siteNumber}...");
            Console.ReadLine();
            ReservationMenu pgm = new ReservationMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO, site, arrivalDate, departureDate);
            pgm.Run();



            Pause("");
            return true;
        }

        //Arrival date display settings
        public void InitializeSetupInfo()
        {
            getArrivalDate();
            getDepartureDate();

            for (int retries = 0; ; retries++)
            {
                try
                {
                    siteList = campsiteDAO.GetAvailableSitesOnCampground(campground.campgroundName, arrivalDate, departureDate);
                    break;
                }
                catch (System.Data.SqlTypes.SqlTypeException ex)
                {
                    if (retries <= 3)
                    {
                        Console.WriteLine($"{ex.Message}\n Try again.");
                        Pause("");
                        getArrivalDate();
                        getDepartureDate();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Too many incorrect attempts!");
                        throw ex;
                    }
                }
            }


            for (int retries = 0; ; retries++)
            {
                try
                {
                    top5Camp = campsiteDAO.GetTop5Campsites(siteList, arrivalDate, departureDate);
                    break;
                }
                catch (InvalidDateRangeSelectionException ex)
                {
                    if (retries <= 3)
                    {
                        Console.WriteLine("Try again. Stay must be at least one night.");
                        getArrivalDate();
                        getDepartureDate();
                        siteList = campsiteDAO.GetAvailableSitesOnCampground(campground.campgroundName, arrivalDate, departureDate);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Too many incorrect attempts!");
                        throw ex;
                    }
                }
            }
            int count = 1;
            foreach (string top in top5Camp)
            {
                this.menuOptions.Add(count.ToString(), top5Camp[count - 1]);
                count++;
            }
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();

        }

        private string displayHeadingText()
        {


            string  s = $"    Site number:  MaxOccupancy:   Is Accessible: Max RV:        Utilities:     Total Price: ";

                       return s;
        }

        private void getArrivalDate()
        {
            Console.WriteLine("What is your desired arrival date?");
            string input1 = Console.ReadLine();
            for (int retries = 0; ; retries++)
            {
                try
                {
                    arrivalDate = DateTime.Parse(input1);
                    break;
                }
                catch (System.FormatException ex)
                {
                    if (retries <= 3)
                    {
                        Console.WriteLine("Try again! Not a valid date format.");
                        Console.WriteLine("What is your desired arrival date?");
                        input1 = Console.ReadLine();
                        continue;

                    }
                    else
                    {
                        Console.WriteLine("Too many incorrect attempts!");
                        throw ex;
                    }
                }
            }
        }
        private void getDepartureDate()
        {
            Console.WriteLine("What is your desired departure date?");
            string input2 = Console.ReadLine();

            for (int retries = 0; ; retries++)
            {
                try
                {
                    departureDate = DateTime.Parse(input2);
                    break;
                }
                catch (System.FormatException ex)
                {
                    if (retries <= 3)
                    {
                        Console.WriteLine("Try again! Not a valid date format.");
                        Console.WriteLine("What is your desired departure date?");
                        input2 = Console.ReadLine();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Too many incorrect attempts!");
                        throw ex;
                    }
                }
            }
        }


        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Available Campsites"));


            if (campground.campgroundName.Length > 20)
            {
                Console.WriteLine(Figgle.FiggleFonts.Straight.Render($"at {campground.campgroundName}"));
            }
            else
            {
                Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"at {campground.campgroundName}"));
            }

            ResetColor();
        }

    }
}
