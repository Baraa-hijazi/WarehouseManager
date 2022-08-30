using AutoMapper;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Persistence.Interfaces;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Services.Warehouse;

public class WarehouseService : IWarehouseService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateWarehouseDto> CreateWarehouse(CreateWarehouseDto dto)
    {
        var wareHouse = _mapper.Map<Core.Entities.Warehouse>(dto);

        _unitOfWork.WarehouseRepository.Add(wareHouse);
        await _unitOfWork.CommitAsync();

        return dto;
    }

    public async Task<PagedResultDto<WarehouseDto>> GetWarehouses(int pageIndex, int pageSize)
    {
        var wareHouses = await _unitOfWork.WarehouseRepository.GetAllIncludedPagination(
            includes: i => i.WarehouseItems,
            pageIndex: pageIndex,
            pageSize: pageSize);

        return _mapper.Map<PagedResultDto<WarehouseDto>>(wareHouses);
    }

    public async Task<WarehouseDto?> GetWarehouse(int id)
    {
        var wareHouse = await _unitOfWork.WarehouseRepository.GetById(id);

        return wareHouse == null ? null : _mapper.Map<WarehouseDto>(wareHouse);
    }

    public async Task<WarehouseDto?> GetWarehouseItems(int id)
    {
        var wareHouse = (await _unitOfWork.WarehouseRepository.GetAllIncluded(a =>
            a.Id == id, includes: i => i.WarehouseItems)).SingleOrDefault();

        return wareHouse == null ? null : _mapper.Map<WarehouseDto>(wareHouse);
    }

    public async Task<WarehouseDto?> UpdateWarehouse(int id, CreateWarehouseDto dto)
    {
        var wareHouse = (await _unitOfWork.WarehouseRepository.GetAllIncluded(a =>
            a.Id == id, includes: i => i.WarehouseItems)).SingleOrDefault();

        if (wareHouse == null) return null;

        _mapper.Map(wareHouse, dto);

        _unitOfWork.WarehouseRepository.Update(wareHouse);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<WarehouseDto>(wareHouse);
    }

    public async Task<WarehouseDto?> DeleteWarehouse(int id)
    {
        var wareHouse = (await _unitOfWork.WarehouseRepository.GetAllIncluded(a =>
            a.Id == id, includes: i => i.WarehouseItems)).SingleOrDefault();

        if (wareHouse == null) return null;

        _unitOfWork.WarehouseRepository.Delete(wareHouse);

        return _mapper.Map<WarehouseDto>(wareHouse);
    }
}