using System.Reflection;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Domain.Common.Core.Abstractions;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Common.Core.Primitives;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BackingShop.Domain.Identity.Enumerations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BackingShop.Database.Common;

/// <summary>
/// Represents the application database context base class.
/// </summary>
public class BaseDbContext
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public BaseDbContext(DbContextOptions<BaseDbContext> options)
        : base(options) { }
    

    /// <inheritdoc />
    public BaseDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables));
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=BSGenericDb;User Id=postgres;Password=1111;");
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        #region Build some elements.

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasKey(l => new { l.UserId, l.RoleId });

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .HasKey(l => new { l.UserId, l.LoginProvider, l.Name });

        modelBuilder.Entity<Category>()
            .HasNoKey();

        modelBuilder.Entity<GroupEvent>()
            .HasOne(g => g.Author)
            .WithMany(u => u.YourGroupEvents)
            .HasForeignKey(g => g.UserId);

        #endregion
    }

    /// <inheritdoc />
    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : class
        => base.Set<TEntity>();

    /// <exception cref="ArgumentNullException"></exception>
    /// <inheritdoc />
    public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : Entity
        => id == Guid.Empty ?
            Maybe<TEntity>.None :
            Maybe<TEntity>.From(await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id) 
            ?? throw new ArgumentNullException());

    /// <inheritdoc />
    public async Task Insert<TEntity>(TEntity entity)
        where TEntity : Entity
        => await Set<TEntity>().AddAsync(entity);

    /// <inheritdoc />
    public async Task InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity
        => await Set<TEntity>().AddRangeAsync(entities);

    /// <inheritdoc />
    public new void Remove<TEntity>(TEntity entity)
        where TEntity : Entity
        => Set<TEntity>().Remove(entity);
    
    /// <inheritdoc />
    public Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default)
        => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
}