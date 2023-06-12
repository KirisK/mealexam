using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App;

public class Product: DomainEntityMetaId
{
    [MaxLength(256)] 
    public string ProductName { get; set; } = default!;
    
    
    public ICollection<RecipeProduct>?  RecipeProducts { get; set; }
    public ICollection<UserProduct>?  UserProducts { get; set; }
}