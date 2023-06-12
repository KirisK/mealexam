using AutoMapper;

namespace Public.DTO.v1.Mappers;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<RecipeProduct, Domain.App.RecipeProduct>().ReverseMap();
        CreateMap<Product, Domain.App.Product>()
            .ForMember(p => p.RecipeProducts, options => options.Ignore())
            .ForMember(p => p.UserProducts, options => options.Ignore())
            .ReverseMap()
            .ForMember(p => p.RecipeProductsCount,
                options => options
                    .MapFrom(src => src.RecipeProducts == null ? null : (int?) src.RecipeProducts.Count))
            .ForMember(p => p.UserProductsCount,
                options => options
                    .MapFrom(src => src.UserProducts == null ? null : (int?) src.UserProducts.Count));
        CreateMap<Recipe, Domain.App.Recipe>().ReverseMap();
        CreateMap<UserProduct, Domain.App.UserProduct>().ReverseMap();

        CreateMap<Identity.AppUser, Domain.App.Identity.AppUser>().ReverseMap();
        CreateMap<Identity.AppRole, Domain.App.Identity.AppRole>().ReverseMap();
    }
}