using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Product.Events;
using BackingShop.RabbitMq.Messaging.User.Events.UserCreated;

namespace BackingShop.RabbitMq.Messaging.Product.ProductCreated;

/// <summary>
/// Represents the <see cref="ProductCreatedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnProductCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnProductCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new ProductCreatedIntegrationEvent(notification));
}