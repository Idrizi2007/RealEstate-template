namespace RealEstate.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository City { get; }
        IPropertyTypeRepository PropertyTypes { get; }
        IAmenityRepository Amenities { get; }
        INeighbourhoodRepository Neighbourhood { get; }
        IContactInfoRepository ContactInfo { get; }
        IPropertyRepository Properties { get; }
        IPropertyImageRepository PropertyImages { get; }
        IPropertyAmenityRepository PropertyAmenities { get; }
        Task SaveAsync();
    }
}

