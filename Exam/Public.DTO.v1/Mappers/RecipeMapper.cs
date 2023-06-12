using AutoMapper;
using Shared.Base.Mapper;

namespace Public.DTO.v1.Mappers;

public class RecipeMapper : EntityMapper<Domain.App.Recipe, Recipe>
{
    public RecipeMapper(IMapper mapper) : base(mapper)
    {
    }
}