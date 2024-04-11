using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.GroupEvent;
using BackingShop.RabbitMq.Messaging;

namespace BackingShop.Events.GroupEvent.Events.Events.AddToGroupEventAttendee;

/// <summary>
/// Represents the <see cref="AddToGroupEventAttendeeDomainEvent"/> class.
/// </summary>
internal sealed class PublishIntegrationEventOnAddToGroupEventAttendeeIntegrationEventDomainHandler
    : IDomainEventHandler<AddToGroupEventAttendeeDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnAddToGroupEventAttendeeIntegrationEventDomainHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnAddToGroupEventAttendeeIntegrationEventDomainHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(AddToGroupEventAttendeeDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new AddToGroupEventAttendeeIntegrationEvent(notification));
}