using Domain.App;
using Domain.App.Identity;
using Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Base;
using Shared.Contracts.Base.Entity;

namespace DAL.EF.App;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IBaseEntityTracker
{
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Recipe> Recipes { get; set; } = default!;
    public DbSet<UserProduct> UserProducts { get; set; } = default!;
    public DbSet<RecipeProduct> RecipesProducts { get; set; } = default!;

    private readonly Dictionary<IEntity, IEntity> _entityTracker = new();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
    
    public void AddToEntityTracker<TKey>(Shared.Contracts.Base.Entity.IEntityId<TKey> internalEntity, Shared.Contracts.Base.Entity.IEntityId<TKey> externalEntity
    ) where TKey : IEquatable<TKey>
    {
        _entityTracker.Add(internalEntity, externalEntity);
    }

    protected void UpdateTrackedEntities()
    {
        foreach (var (internalEntity, externalEntity) in _entityTracker)
        {
            externalEntity.Identifier = internalEntity.Identifier;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var changesCount = await base.SaveChangesAsync(cancellationToken);
        UpdateTrackedEntities();
        return changesCount;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // builder.Entity<UserProduct>().HasKey(l => new { l.AppUserId, l.ProductId });
        // builder.Entity<RecipeProduct>().HasKey(rp => new { rp.RecipeId, rp.ProductId });
        
        // disable cascade delete
        foreach (
            var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())
        )
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        foreach (
            var entityType in builder.Model
                .GetEntityTypes().Select(efType => efType.ClrType)
                .Where(type => typeof(IDomainEntityMeta).IsAssignableFrom(type))
        )
            
        {
            builder.Entity(entityType).Property("CreatedBy").HasMaxLength(64);
            builder.Entity(entityType).Property("UpdatedBy").HasMaxLength(64);
        }
    }
}
