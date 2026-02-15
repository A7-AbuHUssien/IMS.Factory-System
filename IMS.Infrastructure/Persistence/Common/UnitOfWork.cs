using IMS.Application.Common.Interfaces;
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
    public IRepository<SalesOrderItem> SalesOrderItems { get; set; }
    public IRepository<Stock> Stocks { get; private set; }
    public IRepository<StockTransaction> StockTransactions { get; private set; }
    public IRepository<InventoryAdjustment> InventoryAdjustments { get; private set; }
    public IRepository<ReservationRequests> ReservationRequests { get; private set; }


    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Products = new Repository<Product>(_context);
        Customers = new Repository<Customer>(_context);
        Warehouses = new Repository<Warehouse>(_context);
        SalesOrders = new Repository<SalesOrder>(_context);
        SalesOrderItems = new Repository<SalesOrderItem>(_context);
        Stocks = new Repository<Stock>(_context);
        StockTransactions = new Repository<StockTransaction>(_context);
        InventoryAdjustments = new Repository<InventoryAdjustment>(_context);
        ReservationRequests = new Repository<ReservationRequests>(_context);
        
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