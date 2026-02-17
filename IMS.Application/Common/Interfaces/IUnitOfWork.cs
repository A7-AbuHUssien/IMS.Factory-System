using IMS.Domain.Entities;

namespace IMS.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Warehouse> Warehouses { get; }
    IRepository<SalesOrder> SalesOrders { get; }
    IRepository<Stock> Stocks { get; }
    IRepository<StockTransaction> StockTransactions { get; }
    IRepository<InventoryAdjustment> InventoryAdjustments { get; }
    public IRepository<ReservationRequests> ReservationRequests { get;}
    IRepository<SalesOrderItem> SalesOrderItems { get; }
    IRepository<ReturnedItem> ReturnenItems { get; }
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<UserRole> UserRoles { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}