using System.ComponentModel.DataAnnotations;
using Domain.App;
using Domain.Base;
using Public.DTO.v1.Identity;
using Shared.Contracts.Base.Entity;

namespace Public.DTO.v1;

public class UserProduct : EntityId
{
    [Range(minimum:0, maximum:int.MaxValue)]
    public int AvailableAmount { get; set; } = default!;
    public string Units { get; set; } = default!;
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid AppUserId { get; set; }
    
}