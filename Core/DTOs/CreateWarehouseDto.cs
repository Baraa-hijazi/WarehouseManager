namespace WarehouseManager.Core.DTOs;

public class CreateWarehouseDto
{
    public CreateWarehouseDto()
    {
        WarehouseItems = new List<CreateWarehouseItemDto>();
    }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<CreateWarehouseItemDto> WarehouseItems { get; set; }
}