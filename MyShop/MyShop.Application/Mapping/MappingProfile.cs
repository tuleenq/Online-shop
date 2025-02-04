using AutoMapper;

using MyShop.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<Product, CreateProductCommand>().ReverseMap()
        .ForPath(Product => Product.Price, opt => opt.MapFrom(CreateProductCommand=> CreateProductCommand.Amount));
        CreateMap<UpdateProductCommand , Product>();
        CreateMap<CreateUserCommand,User>();
        CreateMap<UpdateUserCommand,User>();
        CreateMap<AddToCartCommand,ShoppingCart>();
        CreateMap<CreateCategoryCommand, Ctegory>();
        CreateMap<CreateBrandCommand, Brand>();

    }
}
