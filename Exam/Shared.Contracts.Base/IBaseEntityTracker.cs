using Shared.Contracts.Base.Entity;

namespace Shared.Contracts.Base;

public interface IBaseEntityTracker
{
    void AddToEntityTracker<TKey>(IEntityId<TKey> internalEntity, IEntityId<TKey> externalEntity)
        where TKey : IEquatable<TKey>;
}