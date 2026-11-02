using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess.Repository
{
    public class PropertyTypeRepository : Repository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
