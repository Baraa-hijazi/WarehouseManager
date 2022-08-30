namespace WarehouseManager.Core.DTOs;

public class WarehouseDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<WarehouseItemDto> WarehouseItems { get; set; }
}