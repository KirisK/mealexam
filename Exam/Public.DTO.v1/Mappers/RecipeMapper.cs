using AutoMapper;
using Shared.Base.Mapper;

namespace Public.DTO.v1.Mappers;

public class RecipeMapper : EntityMapper<Recipe, Recipe>
{
    public RecipeMapper(IMapper mapper) : base(mapper)
    {
    }
}