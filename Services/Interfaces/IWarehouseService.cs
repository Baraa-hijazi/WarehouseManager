using WarehouseManager.Core.DTOs;

namespace WarehouseManager.Services.Interfaces;

public interface IWarehouseService
{
    Task<CreateWarehouseDto> CreateWarehouse(CreateWarehouseDto dto);
    Task<PagedResultDto<WarehouseDto>> GetWarehouses(int pageIndex, int pageSize);
    Task<WarehouseDto?> GetWarehouse(int id);
    Task<WarehouseDto?> GetWarehouseItems(int id);
    Task<WarehouseDto?> UpdateWarehouse(int id, CreateWarehouseDto dto);
    Task<WarehouseDto?> DeleteWarehouse(int id);
}