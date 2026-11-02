using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.DataAccess.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
