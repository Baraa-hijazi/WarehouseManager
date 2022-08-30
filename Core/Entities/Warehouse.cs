namespace WarehouseManager.Core.Entities;

public class Warehouse
{
    public Warehouse()
    {
        WarehouseItems = new List<WarehouseItem>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public ICollection<WarehouseItem> WarehouseItems { get; set; }
}