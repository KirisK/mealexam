using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Public.DTO.v1;

public class Product: DomainEntityMetaId
{
    [MaxLength(64)] 
    public string ProductName { get; set; } = default!;
    
    
    public ICollection<Domain.App.RecipeProduct>?  RecipeProducts { get; set; }
    public ICollection<Domain.App.UserProduct>?  UserProducts { get; set; }
}