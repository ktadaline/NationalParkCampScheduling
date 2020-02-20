﻿using Capstone.DAL;
using Capstone.Views;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("ncampground");

            /********************************************************************
            // If you do not want to use CLIMenu, you can remove the following
            *********************************************************************/
            // Create any DAOs needed here, and then pass them into main menu...

           
            ICampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);
            ICampsiteDAO campsiteDAO = new CampsiteDAO(connectionString);
            IParkDAO parkDAO = new ParkDAO(connectionString);
            IReservationDAO reservationDAO = new ReservationDAO(connectionString);

            


            MainMenu mainMenu = new MainMenu( campgroundDAO, campsiteDAO, parkDAO, reservationDAO);  // You'll probably be adding daos to the constructor

            // Run the menu.
            mainMenu.Run();


        }
    }
}
