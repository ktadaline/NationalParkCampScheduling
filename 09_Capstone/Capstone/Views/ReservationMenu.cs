using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Views
{
    //Reserveation menu
    public class ReservationMenu : CLIMenu
    {
        // Store any private variables, including DAOs here....
        protected ICampgroundDAO campgroundDAO;
        protected ICampsiteDAO campsiteDAO;
        protected IParkDAO parkDAO;
        protected IReservationDAO reservationDAO;
        private Site campsite;
        private DateTime startDate;
        private DateTime endDate;
        override public string SelectionText 
        {
            get { return $"\r\nEnter your name below to finalize your reservation at Campsite Number {campsite.siteNumber}.\n"; }
        } 



        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ReservationMenu(ICampgroundDAO campgroundDAO, ICampsiteDAO campsiteDAO, IParkDAO parkDAO, IReservationDAO reservationDAO, Site campsite, DateTime startDate, DateTime endDate) :
            base("Sub-Menu 1")
        {
            // Store any values or DAOs passed in....
            this.campgroundDAO = campgroundDAO;
            this.campsiteDAO = campsiteDAO;
            this.parkDAO = parkDAO;
            this.reservationDAO = reservationDAO;
            this.campsite = campsite;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        //menu options -> empty because this menu is booking only

        protected override void SetMenuOptions()
        {
        
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {

            Pause("");

            MainMenu mm = new MainMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO);
            mm.Run();
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("\nWhat name should the reservation be made under: ");

            string name = Console.ReadLine();

            int resId = reservationDAO.makeReservation(campsite, name, startDate, endDate);
            Console.WriteLine("\nYou booking was successful!");
            Console.WriteLine("Your reservation ID is: " + resId);

            Console.WriteLine("\nHappy camping!!");

            ResetColor();
            ExecuteSelection(resId.ToString());
           
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Book Reservation for"));
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"Campsite Number {campsite.siteNumber}"));
            ResetColor();
        }
    }
}
