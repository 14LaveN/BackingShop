using BackingShop.Domain.Product.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackingShop.Database.Common.Configurations;

/// <summary>
/// Represents the configuration <see cref="Product"/> entity.
/// </summary>
internal sealed class ProductConfiguration: IEntityTypeConfiguration<Product>
{   
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);
        
        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdProductIndex");
        
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("UserIdProductIndex")
            .IncludeProperties(x=>x.Author);
        
        builder.HasIndex(x => x.Title)
            .HasDatabaseName("TitleProductIndex");

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x=>x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        
        builder
            .Property(product => product.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(product => product.ModifiedOnUtc);

        builder
            .Property(product => product.DeletedOnUtc);

        builder
            .Property(product => product.Deleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(product => !product.Deleted);
    }
}