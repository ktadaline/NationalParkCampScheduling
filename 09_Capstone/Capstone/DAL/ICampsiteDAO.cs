using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampsiteDAO
    {
        IList<Site> GetAvailableSitesOnCampground(string campgroundName, DateTime startDate, DateTime endDate);

        List<string> GetTop5Campsites(List<Site> allsites, DateTime startDate, DateTime endDate);

        decimal GetPriceOfStay(Site site, DateTime startDate, DateTime endDate);

        string campgroundIdToName(int campgroundId);
    }
}
