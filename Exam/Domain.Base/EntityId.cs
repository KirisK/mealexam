using Shared.Contracts.Base.Entity;

namespace Domain.Base;

public abstract class EntityId : EntityId<Guid>, IEntityId
{
}

public abstract class EntityId<TKey> : IEntityId<TKey> 
    where TKey :  IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}