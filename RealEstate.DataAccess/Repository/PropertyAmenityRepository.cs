using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.DataAccess.Repository
{
    public class PropertyAmenityRepository : Repository<PropertyAmenity>, IPropertyAmenityRepository
    {
        public PropertyAmenityRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
