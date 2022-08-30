using AutoMapper;
using WarehouseManager.Core.DTOs;

namespace WarehouseManager.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<WarehouseItemDto, Core.Entities.WarehouseItem>().ReverseMap();
        CreateMap<WarehouseDto, Core.Entities.Warehouse>().ReverseMap();
        CreateMap<ProductDto, Core.Entities.Product>().ReverseMap();
        CreateMap<UserDto, Core.Entities.User>().ReverseMap();


        CreateMap<CreateWarehouseItemDto, Core.Entities.WarehouseItem>().ReverseMap();
        CreateMap<CreateWarehouseDto, Core.Entities.Warehouse>().ReverseMap();
        CreateMap<CreateProductDto, Core.Entities.Product>().ReverseMap();
        CreateMap<CreateUserDto, Core.Entities.User>()
            .ForMember(s => s.UserName,
                opt =>
                    opt.MapFrom(g => g.UserName))
            .ReverseMap();
        
        CreateMap(typeof(PagedResultDto<>), typeof(PagedResultDto<>));
    }
}