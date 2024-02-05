
using System.Linq.Expressions;

namespace Common.SQLServerRepository
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> GetAsync(Guid id);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, 
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
                                        string? includeString = null, 
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}