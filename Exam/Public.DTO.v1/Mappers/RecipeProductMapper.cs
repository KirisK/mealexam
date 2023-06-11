using AutoMapper;
using Shared.Base.Mapper;

namespace Public.DTO.v1.Mappers;

public class RecipeProductMapper : EntityMapper<RecipeProduct, Domain.App.RecipeProduct>
{
    public RecipeProductMapper(IMapper mapper) : base(mapper)
    {
    }
}