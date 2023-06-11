using Domain.App.Identity;
using Domain.Base;

namespace Public.DTO.v1;

public class User: DomainEntityMetaId
{
    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }

    public ICollection<Domain.App.Recipe>?  Recipes { get; set; }
    public ICollection<Domain.App.UserProduct>?  UserProducts { get; set; }
    

}