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
        throw new NotImplementedException();
    }
}