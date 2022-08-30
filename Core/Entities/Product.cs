namespace WarehouseManager.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string SkuName { get; set; }
    public double Price { get; set; }
    public double MsrpPrice { get; set; }
}