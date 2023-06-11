using Microsoft.AspNetCore.Identity;
using Shared.Contracts.Base.Entity;

namespace Domain.App.Identity;

public class AppRole : IdentityRole<Guid>, IEntityId { }
