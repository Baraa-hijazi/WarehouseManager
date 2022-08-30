namespace WarehouseManager.Core.DTOs;

public class CreateWarehouseItemDto
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
}