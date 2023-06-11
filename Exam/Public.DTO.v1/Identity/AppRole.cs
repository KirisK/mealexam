using Shared.Contracts.Base.Entity;

namespace Public.DTO.v1.Identity;

public class AppRole : IEntityId 
{
    public string DisplayName { get; set; } = default!;

    public Guid Id { get; set; }
}