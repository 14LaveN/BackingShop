using BackingShop.Domain.Product.Enumerations;

namespace BackingShop.Micro.Product.Contracts.Product;

/// <summary>
/// Represents the create product request record.
/// </summary>
/// <param name="Title">The title.</param>
/// <param name="Description">The description.</param>
/// <param name="Tag">The tag.</param>
/// <param name="CompanyName">The company name.</param>
/// <param name="ProductType">The product type.</param>
/// <param name="Price">The price.</param>
public sealed record CreateProductRequest(
    string Title,
    string Description,
    string Tag,
    string CompanyName,
    ProductType ProductType,
    decimal Price);