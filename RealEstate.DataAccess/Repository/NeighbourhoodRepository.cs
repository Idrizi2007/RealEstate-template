using System;
using System.Collections.Generic;
using System.Text;
using RealEstate.DataAccess.Repository.IRepository;
using RealEstate.Models;

namespace RealEstate.DataAccess.Repository
{
    public class NeighbourhoodRepository : Repository<Neighbourhood>, INeighbourhoodRepository
    {
        public NeighbourhoodRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
