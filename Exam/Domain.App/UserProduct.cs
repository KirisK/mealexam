using System.ComponentModel.DataAnnotations.Schema;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App;

public class UserProduct : DomainEntityMetaId
{
    public int AvailableAmount { get; set; } = default!;
    public string Units { get; set; } = default!;
    
    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    [ForeignKey(nameof(AppUser))]
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}