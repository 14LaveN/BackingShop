﻿using System.Text.Json.Serialization;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Identity.Events.Invitation;

namespace BackingShop.Events.Invitation.Events.InvitationAccepted;

/// <summary>
/// Represents the integration event that is raised when an invitation is accepted.
/// </summary>
public sealed class InvitationAcceptedIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationAcceptedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="invitationAcceptedDomainEvent">The invitation accepted domain event.</param>
    internal InvitationAcceptedIntegrationEvent(InvitationAcceptedDomainEvent invitationAcceptedDomainEvent) =>
        InvitationId = invitationAcceptedDomainEvent.Invitation.Id;

    [JsonConstructor]
    private InvitationAcceptedIntegrationEvent(Guid invitationId) => InvitationId = invitationId;

    /// <summary>
    /// Gets the invitation identifier.
    /// </summary>
    public Guid InvitationId { get; }
}