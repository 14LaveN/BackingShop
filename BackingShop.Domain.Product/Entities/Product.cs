using BackingShop.Domain.Common.Core.Abstractions;
using BackingShop.Domain.Common.Core.Primitives;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Core.Utility;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Product.Enumerations;
using BackingShop.Domain.Product.Events;

namespace BackingShop.Domain.Product.Entities;

/// <summary>
/// Represents the product entity.
/// </summary>
public sealed class Product
    : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
{
    /// <summary>
    /// Create the new instance <see cref="Product"/> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="price">The price.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="companyName">The company name.</param>
    /// <param name="productType">The product type.</param>
    /// <param name="userId">The user identifier.</param>
    public Product(
        Name title, 
        string description,
        decimal price, 
        string tag,
        string companyName,
        ProductType productType,
        Guid userId)
    {
        Ensure.NotEmpty(title, "The title is required.", nameof(title));
        Ensure.NotEmpty(tag, "The tag is required.", nameof(tag));
        Ensure.NotEmpty(companyName, "The company name is required.", nameof(companyName));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
        
        Price = price;
        ProductType = productType;
        Title = title;
        CompanyName = companyName;
        Description = description;
        Tag = tag;
    }

    /// <inheritdoc />
    private Product() { }

    /// <summary>
    /// Gets or sets price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets user identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets author.
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Gets or sets tag.
    /// </summary>
    public string Tag { get; set; } = null!;

    /// <summary>
    /// Gets or sets product type.
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// Gets or sets created at date/time.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets title.
    /// </summary>
    public Name Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets company name.
    /// </summary>
    public string CompanyName { get; set; } = null!;

    /// <summary>
    /// Creates a new product with the specified title, description, price, tag and product type.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="price">The price.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="companyName">The company name.</param>
    /// <param name="productType">The product type.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The newly created product instance.</returns>
    public static Product Create(
        Name title,
        string description,
        decimal price,
        string tag,
        string companyName,
        ProductType productType,
        Guid userId)
    {
        var product = new Product(title, description, price, tag, companyName, productType,userId);

        product.AddDomainEvent(new ProductCreatedDomainEvent(product));

        return product;
    }

    /// <inheritdoc/>
    public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;
    
    /// <inheritdoc/>
    public DateTime? ModifiedOnUtc { get; }
    
    /// <inheritdoc/>
    public DateTime? DeletedOnUtc { get; }
    
    /// <inheritdoc/>
    public bool Deleted { get; }
}