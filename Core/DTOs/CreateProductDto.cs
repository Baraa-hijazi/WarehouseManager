namespace WarehouseManager.Core.DTOs;

public class CreateProductDto
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public string SkuName { get; set; }

    public double Price { get; set; }

    public double MsrpPrice { get; set; }
}