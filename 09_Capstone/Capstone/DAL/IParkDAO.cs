using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public interface IParkDAO
    {
        List<Park> GetParks();

        List<string> DisplayParkList(List<Park> parks);

        string DisplayParkDetails(Park park);

    }
}
