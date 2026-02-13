using System.Linq.Expressions;

namespace IMS.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Delete(T entity);

    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default);

    Task<T?> GetOneAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default);
    
    IQueryable<T> Query(
        bool tracked = true,
        params Expression<Func<T, object>>[] includes);

    Task<bool> Any(Expression<Func<T, bool>> expression);

}