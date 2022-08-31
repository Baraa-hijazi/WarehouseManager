// using AutoMapper;
// using WarehouseManager.Core.DTOs;
// using WarehouseManager.Persistence.Interfaces;
// using WarehouseManager.Persistence.Repositories;
// using WarehouseManager.Services.Interfaces;
//
// namespace WarehouseManager.Services.Product;
//
// public class ProductService : IProductService
// {
//     private readonly IMapper _mapper;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     public async Task<CreateProductDto> CreateProduct(CreateProductDto dto)
//     {
//         return new CreateProductDto();
//     }
//
//     public async Task<PagedResultDto<ProductDto>> GetProducts(int pageIndex, int pageSize)
//     {
//         var products = 
//             await _unitOfWork.ProductRepository.GetAllIncludedPagination(a => a.Id > 0 &&
//                 a.Name == "",
//             pageIndex: pageIndex,
//             pageSize: pageSize);
//
//         return _mapper.Map<PagedResultDto<ProductDto>>(products);
//     }
//
//     public async Task<IEnumerable<ProductDto>> GetFilteredProduct(string key, string value)
//     {
//         var spec = new EntityFilterSpec<Core.Entities.Product>(key, value);
//
//         var product = await _unitOfWork.ProductRepository.EntityFilterSpec(spec);
//
//         return _mapper.Map<IEnumerable<ProductDto>>(product);
//     }
//
//     public async Task<ProductDto?> GetProduct(int id, string key, string value)
//     {
//         var product = await _unitOfWork.ProductRepository.GetById(id);
//
//         return product == null ? null : _mapper.Map<ProductDto>(product);
//     }
//
//     public async Task<ProductDto?> UpdateProduct(int id, CreateProductDto dto)
//     {
//         var product = await _unitOfWork.ProductRepository.GetById(id);
//
//         if (product == null) return null;
//
//         _mapper.Map(product, dto);
//
//         _unitOfWork.ProductRepository.Update(product);
//         await _unitOfWork.CommitAsync();
//
//         return _mapper.Map<ProductDto>(product);
//     }
//
//     public async Task<ProductDto?> DeleteProduct(int id)
//     {
//         var product = await _unitOfWork.ProductRepository.GetById(id);
//
//         if (product == null) return null;
//
//         _unitOfWork.ProductRepository.Delete(product);
//         await _unitOfWork.CommitAsync();
//
//         return _mapper.Map<ProductDto>(product);
//     }
// }