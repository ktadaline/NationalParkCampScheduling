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
            int inputNum = int.Parse(choice);
            Site site = new Site();

            for (int i = 1; i <= siteList.Count; i++) 
            {
                if (i == inputNum)
                {
                    site = siteList[i - 1];
                }
            }
            //foreach (Site fsite in siteList)
            //{


            //    if (fsite.siteNumber == inputNum)
            //    {
            //        site = fsite;
            //    }
            //}
            //Pause("");
            Console.Write($" Press Enter to continue with reservation on Site Number {site.siteNumber}...");
            Console.ReadLine();
            ReservationMenu pgm = new ReservationMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO, site, arrivalDate, departureDate);
            pgm.Run();



            Pause("");
            return true;
        }

        public void InitializeSetupInfo()
        {
            Console.WriteLine("What is your desired arrival date?");
            string input1 = Console.ReadLine();
            //try
            //{
            //    arrivalDate = DateTime.Parse(input1);
            //}
            //catch (System.FormatException)
            //{
            //    Console.WriteLine("Try again! Not a valid date format.");
            //}
           // arrivalDate = DateTime.Parse(input1);

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
                        arrivalDate = DateTime.Parse(input1);
                        continue;

                    }
                    else
                    {
                        Console.WriteLine("Too many incorrect attempts!");
                        throw ex;
                    }
                }
            }
            Console.WriteLine("What is your desired departure date?");
            string input2 = Console.ReadLine();
            //try
            //{
            //    departureDate = DateTime.Parse(input2);
            //}
            //catch (System.FormatException)
            //{
            //    Console.WriteLine("Try again! Not a valid date format.");
            //}
            ////departureDate = DateTime.Parse(input2);

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
                        departureDate = DateTime.Parse(input2);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Too many incorrect attempts!");
                        throw ex;
                    }
                }
            }

            //Console.WriteLine("What is your desired departure date?");
            //string input2 = Console.ReadLine();
            //departureDate = DateTime.Parse(input2);     //
            siteList = campsiteDAO.GetAvailableSitesOnCampground(campground.campgroundName, arrivalDate, departureDate);
      
            for (int retries = 0; ; retries++)
            {
                try
                {
                    top5Camp = campsiteDAO.GetTop5Campsites(siteList, arrivalDate, departureDate);
                    break;
                }
                catch (InvalidDateSelectionException ex)
                {
                    if (retries <= 3)
                    {
                        Console.WriteLine("Try again. Stay must be at least one night.");
                        Console.WriteLine("What is your desired arrival date?");
                        input1 = Console.ReadLine();

                        arrivalDate = DateTime.Parse(input1);

                        Console.WriteLine("What is your desired departure date?");

                        input2 = Console.ReadLine();

                        departureDate = DateTime.Parse(input2);

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
        //string  s = $"Site number: {site.siteNumber.ToString(),-5}MaxOccupancy: {site.maxOccupancy.ToString(),-5}Is Accessible: {isAccessible,-5}Max RV: {maxRV,-5}Utilities: {utilities,-5}Total Price: {GetPriceOfStay(site, startDate, endDate).ToString("C"), -5}";

        private string displayHeadingText()
        {


            string  s = $"    Site number:  MaxOccupancy:   Is Accessible: Max RV:        Utilities:     Total Price: ";

                       return s;
        }



        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            //Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Available Campsites"));

            //Console.WriteLine($"Find available campsites at {campground.campgroundName}: ");
            //Console.WriteLine();
            if (campground.campgroundName.Length > 20)
            {
                //The Unnamed Primitive Campsites
                Console.WriteLine(Figgle.FiggleFonts.Straight.Render($"at {campground.campgroundName}"));
            }
            else
            {
                Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"at {campground.campgroundName}"));
            }
            //.Render($"{campground.campgroundName}"));
            //SMSlant.Render($"{campground.campgroundName}"));

            //Console.WriteLine(Figgle.FiggleFonts.Straight.Render($"{campground.campgroundName}"));
            //SlantSmall
            //mini
            //pepper
            //threepoint
            ResetColor();
        }

    }
}
