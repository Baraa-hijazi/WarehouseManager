namespace WarehouseManager.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string SkuName { get; set; } = null!;
    public double Price { get; set; }
    public double MsrpPrice { get; set; }
}