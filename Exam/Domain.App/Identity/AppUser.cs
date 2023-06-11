using System.ComponentModel.DataAnnotations;
using Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts.Base.Entity;

namespace Domain.App.Identity;

public class AppUser : IdentityUser<Guid>, IEntityId
{
    [MinLength(1)]
    [MaxLength(128)]
    [Required]
    public string FirstName { get; set; } = default!;

    [MinLength(1)]
    [MaxLength(128)]
    [Required]
    public string LastName { get; set; } = default!;

    public string FirstLastName => FirstName + " " + LastName;
    public string LastFirstName => LastName + " " + FirstName;
    
    public ICollection<Recipe>?  Recipes { get; set; }
    public ICollection<UserProduct>?  UserProducts { get; set; }
}