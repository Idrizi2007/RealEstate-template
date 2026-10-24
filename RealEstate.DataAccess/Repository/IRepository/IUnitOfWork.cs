namespace RealEstate.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository City { get; }

        void Save();
    }
}

