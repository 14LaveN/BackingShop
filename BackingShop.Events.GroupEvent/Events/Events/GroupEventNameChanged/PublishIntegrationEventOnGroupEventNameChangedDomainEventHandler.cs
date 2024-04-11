using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.GroupEvent;
using BackingShop.RabbitMq.Messaging;

namespace BackingShop.Events.GroupEvent.Events.Events.GroupEventNameChanged;

/// <summary>
/// Represents the <see cref="GroupEventNameChangedDomainEvent"/> class.
/// </summary>
public sealed class PublishIntegrationEventOnGroupEventNameChangedDomainEventHandler
    : IDomainEventHandler<GroupEventNameChangedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnGroupEventNameChangedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnGroupEventNameChangedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(GroupEventNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new GroupEventNameChangedIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}