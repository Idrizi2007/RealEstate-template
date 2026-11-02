namespace RealEstate.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository City { get; }
        IPropertyTypeRepository PropertyTypes { get; }
        void Save();
    }
}

