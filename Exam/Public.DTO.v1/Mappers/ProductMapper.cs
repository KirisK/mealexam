using AutoMapper;
using Shared.Base.Mapper;

namespace Public.DTO.v1.Mappers;

public class ProductMapper : EntityMapper<Domain.App.Product, Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}