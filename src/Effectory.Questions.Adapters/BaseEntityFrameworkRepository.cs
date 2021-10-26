using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Effectory.Questions.Adapters
{
    public abstract class BaseEntityFrameworkRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected IQueryable<TEntity> DbSet => _dbSet;


        protected BaseEntityFrameworkRepository(TDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected virtual IAsyncEnumerable<TEntity> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int? take = null,
            int? skip = null)
        {
            IQueryable<TEntity> query = BuildQuery(filter, orderBy, includeProperties, take, skip);
            return query.AsNoTracking().AsAsyncEnumerable();
        }

        private IQueryable<TEntity> BuildQuery(
            Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
            string includeProperties,
            int? take = null,
            int? skip = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);


            return query;
        }

        public virtual ValueTask<TEntity?> GetByIdAsync(object id)
        {
            return _dbSet.FindAsync(id)!; // documentation states that if not found null will be returned
        }

        public virtual async ValueTask InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async ValueTask DeleteAsync(object id)
        {
            TEntity? entityToDelete = await GetByIdAsync(id);
            Delete(entityToDelete);
        }


        public virtual void Delete(TEntity? entityToDelete)
        {
            if (entityToDelete == null)
                return;

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected virtual void RemoveRange(IEnumerable<TEntity> entitiesToRemove)
        {
            _dbSet.RemoveRange(entitiesToRemove);
        }
    }
}