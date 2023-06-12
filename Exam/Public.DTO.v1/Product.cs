using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Public.DTO.v1;

public class Product: EntityId
{
    [MaxLength(256)] 
    public string ProductName { get; set; } = default!;
    
    public int? RecipeProductsCount { get; set; }
    public int? UserProductsCount { get; set; }
    
}