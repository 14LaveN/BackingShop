using BackingShop.Domain.Common.Core.Events;

namespace BackingShop.Domain.Product.Events;

/// <summary>
/// Represents the event that is raised when a product is created.
/// </summary>
public sealed class ProductCreatedDomainEvent
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="product">The Product.</param>
    internal ProductCreatedDomainEvent(Entities.Product product) => Product = product;

    /// <summary>
    /// Gets the group event.
    /// </summary>
    public Entities.Product Product { get; }
}