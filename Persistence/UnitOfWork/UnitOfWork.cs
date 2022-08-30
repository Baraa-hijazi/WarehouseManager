using WarehouseManager.Core.Entities;
using WarehouseManager.Persistence.Context;
using WarehouseManager.Persistence.Interfaces;
using WarehouseManager.Persistence.Repositories;

namespace WarehouseManager.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IBaseRepository<User> UserRepository { get; }
    public IBaseRepository<Warehouse> WarehouseRepository { get; }
    public IBaseRepository<WarehouseItem> WarehouseItemRepository { get; }
    public IBaseRepository<Product> ProductRepository { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        UserRepository = new BaseRepository<User>(context);
        WarehouseRepository = new BaseRepository<Warehouse>(context);
        WarehouseItemRepository = new BaseRepository<WarehouseItem>(context);
        ProductRepository = new BaseRepository<Product>(context);
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}