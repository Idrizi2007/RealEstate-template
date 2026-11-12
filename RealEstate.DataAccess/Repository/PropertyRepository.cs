using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.DataAccess.Repository
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
