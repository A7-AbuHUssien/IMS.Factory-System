using System.Linq.Expressions;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Persistence.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = Query(tracked, includes ?? Array.Empty<Expression<Func<T, object>>>());
        if (expression != null) query = query.Where(expression);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetOneAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = Query(tracked, includes ?? Array.Empty<Expression<Func<T, object>>>());
        if (expression != null) query = query.Where(expression);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public IQueryable<T> Query(bool tracked = true, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked) query = query.AsNoTracking();
        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        return query;
    }

    public async Task<bool> Any(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }
}