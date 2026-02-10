using IMS.Application.Interfaces.Repositories;
using IMS.Domain.Entities;

namespace IMS.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Warehouse> Warehouses { get; }
    IRepository<SalesOrder> SalesOrders { get; }
    IRepository<User> Users { get; }
    
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}