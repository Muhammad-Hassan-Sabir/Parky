using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NationalParkRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Add(nationalPark);

            return Save();

            
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Remove(nationalPark);

            return Save();

        }

        public NationalPark GetNationalPark(int id)
        {
         var nationalPark=   _dbContext.NationalParks.FirstOrDefault(x=>x.Id == id);


            return nationalPark;
            
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _dbContext.NationalParks.OrderBy(n=>n.Name).ToList();

        }


        public bool NationalParkExists(int id)
        {
            bool value=_dbContext.NationalParks.Any(x=>x.Id == id);
            return value;
        }

        public bool NationalParkExists(string name)
        {
            bool value=_dbContext.NationalParks.Any(x=>x.Name.Trim().ToLower() == name.Trim().ToLower());
            return value;

        }

        public bool Save()
        {
            return _dbContext.SaveChanges()>=0?true:false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Update(nationalPark);

            return Save();
        }
    }
}
