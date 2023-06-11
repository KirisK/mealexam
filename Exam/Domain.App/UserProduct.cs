using Domain.App.Identity;
using Domain.Base;

namespace Domain.App;

public class UserProduct : DomainEntityMetaId
{
    public int AvailableAmount { get; set; } = default!;
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}