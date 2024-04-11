using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.Invitation;
using BackingShop.RabbitMq.Messaging;

namespace BackingShop.Events.Invitation.Events.InvitationAccepted;

/// <summary>
/// Represents the <see cref="InvitationAcceptedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnInvitationAcceptedDomainEventHandler
    : IDomainEventHandler<InvitationAcceptedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnInvitationAcceptedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnInvitationAcceptedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new InvitationAcceptedIntegrationEvent(notification));

        await Task.CompletedTask;
    }
}