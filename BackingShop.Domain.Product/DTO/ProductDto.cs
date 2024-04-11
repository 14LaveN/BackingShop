namespace BackingShop.Domain.Product.DTO;

/// <summary>
/// Represents the product database transfer object record.
/// </summary>
/// <param name="Title">The title.</param>
/// <param name="Price">The price.</param>
/// <param name="Description">The description.</param>
/// <param name="Tag">The tag.</param>
/// <param name="ProductType">The product type.</param>
public sealed record ProductDto(
    string Title,
    decimal Price,
    string Description,
    string Tag,
    string ProductType,
    DateTime CreatedAt)
{
    /// <summary>
    /// Create the product data transfer object from <see cref="Product"/> record.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>The new product data transfer object.</returns>
    public static implicit operator ProductDto(Entities.Product product)
    {
        return new ProductDto(
            product.Title,
            product.Price,
            product.Description,
            product.Tag,
            product.ProductType.ToString(),
            product.CreatedOnUtc);
    }
}