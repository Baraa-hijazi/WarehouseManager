using WarehouseManager.Core.Entities;

namespace WarehouseManager.Persistence.Interfaces;

public interface IUnitOfWork
{
    IBaseRepository<User> UserRepository { get; }
    IBaseRepository<Warehouse> WarehouseRepository { get; }
    IBaseRepository<WarehouseItem> WarehouseItemRepository { get; }
    IBaseRepository<Product> ProductRepository { get; }
    Task CommitAsync();
}