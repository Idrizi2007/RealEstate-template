
using RealEstate.DataAccess.Repository.IRepository;

namespace RealEstate.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICityRepository City { get; private set; }
        public IPropertyTypeRepository PropertyTypes { get; private set; }
        public IAmenityRepository Amenities { get; private set; }
        public INeighbourhoodRepository Neighbourhood { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            City = new CityRepository(_db);
            PropertyTypes = new PropertyTypeRepository(_db);
            Amenities = new AmenityRepository(_db);
            Neighbourhood = new NeighbourhoodRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
