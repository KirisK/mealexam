using AutoMapper;
using Shared.Base.Mapper;

namespace Public.DTO.v1.Mappers;

public class UserProductMapper : EntityMapper<UserProduct, Domain.App.UserProduct>
{
    public UserProductMapper(IMapper mapper) : base(mapper)
    {
    }
}