using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        List<string> campgroundsToString(IList<Campground> campgrounds);
        IList<Campground> GetCampgrounds(Park park);
    }
}