namespace WarehouseManager.Core.DTOs;

public class WarehouseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<WarehouseItemDto> WarehouseItems { get; set; } = new List<WarehouseItemDto>();
}