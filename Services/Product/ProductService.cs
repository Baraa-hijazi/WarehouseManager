using AutoMapper;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Persistence.Interfaces;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Services.Product;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateProductDto> CreateProduct(CreateProductDto dto)
    {
        var product = _mapper.Map<Core.Entities.Product>(dto);

        _unitOfWork.ProductRepository.Add(product);
        await _unitOfWork.CommitAsync();

        return dto;
    }

    public async Task<PagedResultDto<ProductDto>> GetProducts(int pageIndex, int pageSize)
    {
        var products = await _unitOfWork.ProductRepository.GetAllIncludedPagination(a => a.Id > 0,
            pageIndex: pageIndex,
            pageSize: pageSize);

        return _mapper.Map<PagedResultDto<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetById(id);

        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _unitOfWork.ProductRepository.GetById(id);

        if (product == null) return null;

        _mapper.Map(product, dto);

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> DeleteProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetById(id);

        if (product == null) return null;

        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ProductDto>(product);
    }
}