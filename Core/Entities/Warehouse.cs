namespace WarehouseManager.Core.Entities;

public class Warehouse
{
    public Warehouse()
    {
        WarehouseItems = new List<WarehouseItem>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ICollection<WarehouseItem> WarehouseItems { get; set; }
}