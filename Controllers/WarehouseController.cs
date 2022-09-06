using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Persistence.Interfaces;

namespace WarehouseManager.Controllers;

[Route("API/[Controller]")]
[ApiController]
public class WarehouseController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const string ActionName = nameof(GetValue);

    public WarehouseController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto)
    {
        var warehouse = _mapper.Map<Core.Entities.Warehouse>(dto);

        _unitOfWork.WarehouseRepository.Add(warehouse);
        await _unitOfWork.CommitAsync();

        var createdResource = new { warehouse.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return CreatedAtAction(ActionName, routeValues, createdResource);
    }

    [HttpGet("GetValues")]
    public async Task<IActionResult> GetWarehouses(int pageIndex, int pageSize)
    {
        var wareHouses = await _unitOfWork.WarehouseRepository.GetAllIncludedPagination(
            includes: i => i.WarehouseItems,
            pageIndex: pageIndex,
            pageSize: pageSize);

        return Ok(_mapper.Map<PagedResultDto<WarehouseDto>>(wareHouses));
    }

    [HttpGet]
    public async Task<IActionResult> GetValue(int id)
    {
        var wareHouse = await _unitOfWork.WarehouseRepository.GetById(id);

        if (wareHouse == null) return NotFound();

        return Ok(wareHouse);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] CreateWarehouseDto dto)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetById(id);

        if (warehouse == null) return NotFound();

        _mapper.Map(dto, warehouse);

        _unitOfWork.WarehouseRepository.Update(warehouse);
        await _unitOfWork.CommitAsync();

        var createdResource = new { warehouse.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return AcceptedAtAction(ActionName, routeValues, createdResource);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetById(id);

        if (warehouse == null) return NotFound();

        _unitOfWork.WarehouseRepository.Delete(warehouse);
        await _unitOfWork.CommitAsync();

        var createdResource = new { warehouse.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return AcceptedAtAction(ActionName, routeValues, createdResource);
    }
}