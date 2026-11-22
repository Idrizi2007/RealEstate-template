using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.DataAccess.Repository
{
    public class PropertyImageRepository : Repository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
