using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Services.Exception;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Controllers;

[Route("API/[Controller]")]
[ApiController]
public class ProductController : BaseController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto) =>
        Ok(await _productService.CreateProduct(dto));

    [HttpGet("GetProducts")]
    public async Task<IActionResult> GetProducts(int pageIndex, int pageSize) =>
        Ok(await _productService.GetProducts(pageIndex, pageSize));

    [HttpGet("GetProduct")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var result = await _productService.GetProduct(id);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }

    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDto dto)
    {
        var result = await _productService.UpdateProduct(id, dto);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }

    [HttpDelete("DeleteProduct")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProduct(id);
        if (result == null) return StatusCode(404, ExceptionState.IdNotFound);
        return Ok();
    }
}