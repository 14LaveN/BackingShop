﻿using System.Text.Json.Serialization;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Identity.Events.GroupEvent;

namespace BackingShop.Events.GroupEvent.Events.Events.GroupEventCancelled;

/// <summary>
/// Represents the event that is raised when a group event is cancelled.
/// </summary>
public sealed class GroupEventCancelledIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventCancelledIntegrationEvent"/> class.
    /// </summary>
    /// <param name="groupEventCancelledDomainEvent">The group event cancelled domain event.</param>
    internal GroupEventCancelledIntegrationEvent(GroupEventCancelledDomainEvent groupEventCancelledDomainEvent) =>
        GroupEventId = groupEventCancelledDomainEvent.GroupEvent.Id;

    [JsonConstructor]
    private GroupEventCancelledIntegrationEvent(Guid groupEventId) => GroupEventId = groupEventId;

    /// <summary>
    /// Gets the group event identifier.
    /// </summary>
    public Guid GroupEventId { get; }
}