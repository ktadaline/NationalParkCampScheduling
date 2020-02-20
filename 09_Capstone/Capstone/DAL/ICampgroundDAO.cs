using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        List<string> campgroundsToString(List<Campground> campgrounds);
        IList<Campground> GetCampgrounds(string parkName);
    }
}