using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository
    {

        ICollection<Trail> GetTrails();

        Trail GetTrail(int id);

        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);


        bool TrailExists(int id);
        bool TrailExists(string name);

        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);

        bool DeleteTrail(Trail trail);

        bool Save();





    }
}
