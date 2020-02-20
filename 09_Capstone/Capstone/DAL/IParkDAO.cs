using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface IParkDAO
    {
        string DisplayParkDetails(Park park);
        List<string> DisplayParkList(List<Park> parks);
        List<Park> GetParks();
    }
}