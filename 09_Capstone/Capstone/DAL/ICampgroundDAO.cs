using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public interface ICampgroundDAO
    {

        IList<Campground> GetCampgrounds(string parkName);

        List<string> campgroundsToString(List<Campground> campgrounds);


    }
}
