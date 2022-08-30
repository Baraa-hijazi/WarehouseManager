using WarehouseManager.Core.DTOs;

namespace WarehouseManager.Services.Interfaces;

public interface IProductService
{
    Task<CreateProductDto> CreateProduct(CreateProductDto dto);
    Task<PagedResultDto<ProductDto>> GetProducts(int pageIndex, int pageSize);
    Task<ProductDto?> GetProduct(int id);
    Task<ProductDto?> UpdateProduct(int id, CreateProductDto dto);
    Task<ProductDto?> DeleteProduct(int id);
}