using IMS.Application.Interfaces.Repositories;
using IMS.Application.Interfaces.UOW;
using IMS.Domain.Entities;
using IMS.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Infrastructure.Persistence.Common;


public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    public IRepository<Product> Products { get; private set; }
    public IRepository<Customer> Customers { get; private set; }
    public IRepository<Warehouse> Warehouses { get; private set; }
    public IRepository<SalesOrder> SalesOrders { get; private set; }
    public IRepository<User> Users { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Products = new Repository<Product>(_context);
        Customers = new Repository<Customer>(_context);
        Warehouses = new Repository<Warehouse>(_context);
        SalesOrders = new Repository<SalesOrder>(_context);
        Users = new Repository<User>(_context);
    }
    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_currentTransaction != null) await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}