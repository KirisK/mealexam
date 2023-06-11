using Shared.Contracts.Base.Entity;

namespace Public.DTO.v1.Identity;

public class AppUser : IEntityId
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string FirstLastName => FirstName + " " + LastName;
    public string LastFirstName => LastName + " " + FirstName;
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    
}