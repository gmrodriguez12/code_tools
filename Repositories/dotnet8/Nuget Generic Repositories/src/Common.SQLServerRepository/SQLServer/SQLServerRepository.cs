
using System.Linq.Expressions;
using Common.SQLServerRepository.Context;
using Microsoft.EntityFrameworkCore;

namespace Common.SQLServerRepository.SQLServer
{
    public class SQLServerRepository<T> : IRepository<T> where T : IEntity
    {
        protected readonly GenericDbContext _dbContext;

        public SQLServerRepository(GenericDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = _dbContext.Set<T>().Find(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var entities = await _dbContext.Set<T>().ToListAsync();
            return entities ?? throw new Exception($"Entities {typeof(T).Name} not found");
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Entity {typeof(T).Name} with id {id} not found");
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                               string? includeString = null,
                                               bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
                query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }
    }
}