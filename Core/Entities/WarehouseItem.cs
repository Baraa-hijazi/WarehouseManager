namespace WarehouseManager.Core.Entities;

public class WarehouseItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int WarehouseId { get; set; }

    public Warehouse Warehouse { get; set; } = null!;

    public int Quantity { get; set; }
}