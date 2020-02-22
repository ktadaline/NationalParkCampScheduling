using System;
using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface ICampsiteDAO
    {
        //string campgroundIdToName(int campgroundId);
        IList<Site> GetAvailableSitesOnCampground(string campgroundName, DateTime startDate, DateTime endDate);
        decimal GetPriceOfStay(Site site, DateTime startDate, DateTime endDate);
        List<string> GetTop5Campsites(IList<Site> allsites, DateTime startDate, DateTime endDate);
    }
}