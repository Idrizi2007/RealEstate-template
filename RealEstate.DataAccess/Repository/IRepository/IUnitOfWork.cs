namespace RealEstate.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository City { get; }
        IPropertyTypeRepository PropertyTypes { get; }
        IAmenityRepository Amenities { get; }
        INeighbourhoodRepository Neighbourhood { get; }
        Task SaveAsync();
    }
}

