using System.Text.Json.Serialization;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Product.Events;

namespace BackingShop.RabbitMq.Messaging.Product.ProductCreated;

/// <summary>
/// Represents the integration event that is raised when a product is created.
/// </summary>
public sealed class ProductCreatedIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCreatedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="productCreatedDomainEvent">The product created domain event.</param>
    internal ProductCreatedIntegrationEvent(ProductCreatedDomainEvent productCreatedDomainEvent) => ProductId = productCreatedDomainEvent.Product.Id;
        
    [JsonConstructor]
    public ProductCreatedIntegrationEvent(Guid productId) => ProductId = productId;

    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public Guid ProductId { get; }
}