using BackingShop.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackingShop.Database.Common.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Event"/> entity.
/// </summary>
internal sealed class EventConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.Event>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);
    }
}