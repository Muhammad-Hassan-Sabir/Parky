using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TrailRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateTrail(Trail trail)
        {
            _dbContext.Trails.Add(trail);

            return Save();

            
        }





        public bool DeleteTrail(Trail trail)
        {
            _dbContext.Trails.Remove(trail);

            return Save();

        }

        public Trail GetTrail(int id)
        {
         var trail=   _dbContext.Trails.Include(c=>c.NationalPark).FirstOrDefault(x=>x.Id == id);


            return trail;
            
        }

        public ICollection<Trail> GetTrails()
        {
            return _dbContext.Trails.Include(c => c.NationalPark).OrderBy(n=>n.Name).ToList();

        }


        public bool TrailExists(int id)
        {
            bool value=_dbContext.Trails.Any(x=>x.Id == id);
            return value;
        }

        public bool TrailExists(string name)
        {
            bool value=_dbContext.Trails.Any(x=>x.Name.Trim().ToLower() == name.Trim().ToLower());
            return value;

        }

        public bool Save()
        {
            return _dbContext.SaveChanges()>=0?true:false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _dbContext.Trails.Update(trail);

            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {

            return _dbContext.Trails.Include(c => c.NationalPark)
                    .Where(t => t.NationalParkId == nationalParkId).ToList();

        }
    }
}
