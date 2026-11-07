using Microsoft.EntityFrameworkCore;
using RealEstate.DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace RealEstate.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            var query = _dbSet.AsQueryable();
            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>())
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);
            foreach (var includeProperty in includeProperties?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>())
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
