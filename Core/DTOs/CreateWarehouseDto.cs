namespace WarehouseManager.Core.DTOs;

public class CreateWarehouseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<CreateWarehouseItemDto> WarehouseItems { get; set; } = new List<CreateWarehouseItemDto>();
}