namespace WarehouseManager.Core.DTOs;

public class WarehouseItemDto
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }

    public int Quantity { get; set; }
}