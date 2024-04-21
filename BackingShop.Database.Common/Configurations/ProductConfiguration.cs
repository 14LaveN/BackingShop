using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Identity.Entities;
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
            .HasDatabaseName("UserIdProductIndex");
        
        builder.HasIndex(x => x.Title)
            .HasDatabaseName("TitleProductIndex");

        builder.Ignore(x => x.Title);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x=>x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        //TODO builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        
        builder.OwnsOne(product => product.Title, titleBuilder =>
        {
            titleBuilder.WithOwner();

            titleBuilder.Property(title => title.Value)
                .HasColumnName(nameof(Product.Title))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });
        
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