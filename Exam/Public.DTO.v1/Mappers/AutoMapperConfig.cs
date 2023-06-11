using AutoMapper;

namespace Public.DTO.v1.Mappers;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<RecipeProduct, Domain.App.RecipeProduct>().ReverseMap();
        CreateMap<Product, Domain.App.Product>().ReverseMap();
        CreateMap<Recipe, Domain.App.Recipe>().ReverseMap();
        CreateMap<UserProduct, Domain.App.UserProduct>().ReverseMap();

        CreateMap<Identity.AppUser, Domain.App.Identity.AppUser>().ReverseMap();
        CreateMap<Identity.AppRole, Domain.App.Identity.AppRole>().ReverseMap();

    }
}