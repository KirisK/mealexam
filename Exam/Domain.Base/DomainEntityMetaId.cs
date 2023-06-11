using System.ComponentModel.DataAnnotations;
using Domain.Contracts.Base;
using Shared.Contracts.Base.Entity;

namespace Domain.Base;

public abstract class DomainEntityMetaId : DomainEntityMetaId<Guid>, IEntityId
{
    
}

public abstract class DomainEntityMetaId<TKey> : EntityId<TKey> , IDomainEntityMeta
    where TKey : IEquatable<TKey>
{
    [MaxLength(32)]    
    public string? CreatedBy { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(32)]    
    public string? UpdatedBy { get; set; } 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}