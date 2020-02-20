using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    class CampsiteDAO :ICampsiteDAO
    {
        private string connectionString;

        public CampsiteDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
    }
}
