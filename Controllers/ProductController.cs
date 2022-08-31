using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Persistence.Interfaces;

namespace WarehouseManager.Controllers;

[Route("API/[Controller]")]
[ApiController]
public class ProductController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const string ActionName = nameof(GetValue);

    public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductDto dto)
    {
        var product = _mapper.Map<Core.Entities.Product>(dto);

        _unitOfWork.ProductRepository.Add(product);
        await _unitOfWork.CommitAsync();

        var createdResource = new { product.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return CreatedAtAction(ActionName, routeValues, createdResource);
    }

    [AllowAnonymous]
    [HttpGet("GetValues")]
    public async Task<IActionResult> GetValues(int pageIndex, int pageSize)
    {
        var products = await _unitOfWork.ProductRepository.GetAllIncludedPagination(
            pageIndex: pageIndex,
            pageSize: pageSize);

        return Ok(_mapper.Map<PagedResultDto<ProductDto>>(products));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetValue(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetById(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] CreateProductDto dto)
    {
        var product = await _unitOfWork.ProductRepository.GetById(id);

        if (product == null) return NotFound();

        _mapper.Map(dto, product);

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();

        var createdResource = new { product.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return AcceptedAtAction(ActionName, routeValues, createdResource);
    }

    [AllowAnonymous]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetById(id);

        if (product == null) return NotFound();

        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.CommitAsync();

        var createdResource = new { product.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return AcceptedAtAction(ActionName, routeValues, createdResource);
    }
}