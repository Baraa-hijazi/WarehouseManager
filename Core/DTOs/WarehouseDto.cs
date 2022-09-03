namespace WarehouseManager.Core.DTOs;

public class WarehouseDto
{
    public WarehouseDto(ICollection<WarehouseItemDto> warehouseItems)
    {
        WarehouseItems = warehouseItems;
    }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<WarehouseItemDto> WarehouseItems { get; set; }
}