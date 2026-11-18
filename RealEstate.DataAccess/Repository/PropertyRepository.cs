using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.DataAccess.Repository
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        private readonly ApplicationDbContext _db;
        public PropertyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public override void Update(Property entity)
        {
            var objFromDb = _db.Properties.FirstOrDefault(u => u.Id == entity.Id);
            if (objFromDb != null)
            {
                objFromDb.ReferenceCode = entity.ReferenceCode;
                objFromDb.Title = entity.Title;

                objFromDb.PropertyTypeId = entity.PropertyTypeId;
                objFromDb.CityId = entity.CityId;
                objFromDb.NeighbourhoodId = entity.NeighbourhoodId;
                objFromDb.Address = entity.Address;

                objFromDb.SalePrice = entity.SalePrice;
                objFromDb.RentPrice = entity.RentPrice;
                objFromDb.Deposit = entity.Deposit;
                objFromDb.MinimumTermMonths = entity.MinimumTermMonths;

                objFromDb.AreaM2 = entity.AreaM2;
                objFromDb.Bedrooms = entity.Bedrooms;
                objFromDb.Bathrooms = entity.Bathrooms;
                objFromDb.Floor = entity.Floor;
                objFromDb.TotalFloors = entity.TotalFloors;
                objFromDb.YearBuilt = entity.YearBuilt;
                objFromDb.Description = entity.Description;

                objFromDb.IsActive = entity.IsActive;
            }
        }
    }
}
