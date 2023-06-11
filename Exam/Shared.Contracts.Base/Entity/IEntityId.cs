namespace Shared.Contracts.Base.Entity;

/// <summary>
/// default Guid based Entity interface
/// </summary>
public interface IEntityId : IEntityId<Guid>
{
}

/// <summary>
/// Universal Entity interface based on generic identifier type. For domain entities the id is a PK.
/// </summary>
/// <typeparam name="TKey">Type for identifier / primary key</typeparam>
public interface IEntityId<TKey> : IEntity
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }

    object IEntity.Identifier
    {
        get => Id;
        set => Id = (TKey) value;
    }
}

/// <summary>
/// Universal Entity interface. Entity should have an identifier of any type
/// </summary>
public interface IEntity
{
    object Identifier { get; set; }
}