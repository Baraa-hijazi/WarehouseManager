namespace WarehouseManager.Core.DTOs;

public class CreateWarehouseDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<CreateWarehouseItemDto> WarehouseItems { get; set; }
}