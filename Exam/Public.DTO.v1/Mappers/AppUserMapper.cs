using AutoMapper;
using Public.DTO.v1.Identity;
using Shared.Base.Mapper;

namespace Public.DTO.v1.Mappers
{
    public class AppUserMapper: EntityMapper<Domain.App.Identity.AppUser, AppUser>
    {
        public AppUserMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}