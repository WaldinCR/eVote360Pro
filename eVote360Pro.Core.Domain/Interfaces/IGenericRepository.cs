using System.Linq.Expressions;

namespace eVote360Pro.Core.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetAllListWithIncludeAsync(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllQuery();
        IQueryable<T> GetAllQueryWithInclude(params Expression<Func<T, object>>[] includes);
    }
}

