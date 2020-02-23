﻿using Capstone.DAL;
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

        //menu options

        protected override void SetMenuOptions()
        {
            //this.menuOptions.Add("1", "View Available Campsites");
            //this.menuOptions.Add("2", "Do Option 2 and return to Main");
           // this.menuOptions.Add("1", "Back to Main Menu");
            //this.quitKey = "1";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {


            //Console.WriteLine("Make your reservation here!");

            //Console.WriteLine("What name should the reservation be made under: ");

            //string name = Console.ReadLine();

            //int resId = reservationDAO.makeReservation(campsite, name, startDate, endDate);

            //Console.WriteLine("Your reservation ID is: " + resId);

            //Console.WriteLine("What is your desired arrival date?");
            //string input = Console.ReadLine();

            //DateTime arrivalDate = DateTime.Parse(input);

            //Console.WriteLine("What is your desired departure date?");

            //input = Console.ReadLine();

            //DateTime departureDate = DateTime.Parse(input);

            //IList<Site> siteList = campsiteDAO.GetAvailableSitesOnCampground(campground.campgroundName, arrivalDate, departureDate);

            //List<string> top5Camp = campsiteDAO.GetTop5Campsites(siteList, arrivalDate, departureDate);
            //foreach (string top in top5Camp)
            //{
            //    Console.WriteLine(top);
            //}

            Pause("");

            MainMenu mm = new MainMenu(campgroundDAO, campsiteDAO, parkDAO, reservationDAO);
            mm.Run();
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();


            //string displayParks = campgroundDAO.Display(campground);
            //Console.WriteLine("Make a reservation at Campsite Number " + campsite.siteNumber);
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);

            //Console.WriteLine("Make your reservation here!");

            Console.WriteLine("\nWhat name should the reservation be made under: ");

            string name = Console.ReadLine();

            int resId = reservationDAO.makeReservation(campsite, name, startDate, endDate);
            Console.WriteLine("\nYou booking was successful!");
            Console.WriteLine("Your reservation ID is: " + resId);

            Console.WriteLine("\nHappy camping!!");

            ResetColor();
            ExecuteSelection(resId.ToString());
            //Pause("");
            //return true;

            //Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            //ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Book Reservation"));
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render($"at Campsite Number {campsite.siteNumber}"));

            //Console.WriteLine("Make a reservation at Campsite Number " + campsite.siteNumber);

            ResetColor();
        }
    }
}
