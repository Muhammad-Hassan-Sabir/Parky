using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {

        ICollection<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int id);

        bool NationalParkExists(int id);
        bool NationalParkExists(string name);

        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);

        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();





    }
}
