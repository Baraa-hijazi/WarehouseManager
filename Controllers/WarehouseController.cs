using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Services.Exception;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Controllers;

[Route("API/[Controller]")]
[ApiController]
public class WarehouseController : BaseController
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost("CreateWarehouse")]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto) =>
        Ok(await _warehouseService.CreateWarehouse(dto));

    [HttpGet("GetWarehouses")]
    public async Task<IActionResult> GetWarehouses(int pageIndex, int pageSize) =>
        Ok(await _warehouseService.GetWarehouses(pageIndex, pageSize));

    [HttpGet("GetWarehouse")]
    public async Task<IActionResult> GetWarehouse(int id)
    {
        var result = await _warehouseService.GetWarehouse(id);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }

    [HttpGet("GetWarehouseItems")]
    public async Task<IActionResult> GetWarehouseItems(int id)
    {
        var result = await _warehouseService.GetWarehouseItems(id);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }

    [HttpPut("UpdateWarehouse")]
    public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] CreateWarehouseDto dto)
    {
        var result = await _warehouseService.UpdateWarehouse(id, dto);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }

    [HttpDelete("DeleteWarehouse")]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        var result = await _warehouseService.DeleteWarehouse(id);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }
}