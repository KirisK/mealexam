using Domain.App;
using Domain.Base;

namespace Public.DTO.v1;

public class UserProduct : DomainEntityMetaId
{
    public int AvailableAmount { get; set; } = default!;
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
}