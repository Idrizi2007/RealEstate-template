
using RealEstate.DataAccess.Repository.IRepository;

namespace RealEstate.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICityRepository City { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            City = new CityRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
