﻿using System.Text.Json.Serialization;
using BackingShop.Application.Core.Abstractions.Messaging;

using BackingShop.Domain.Identity.Events.User;

namespace BackingShop.RabbitMq.Messaging.User.Events.UserCreated;

/// <summary>
/// Represents the integration event that is raised when a user is created.
/// </summary>
public sealed class UserCreatedIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreatedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="userCreatedDomainEvent">The user created domain event.</param>
    internal UserCreatedIntegrationEvent(UserCreatedDomainEvent userCreatedDomainEvent) => UserId = userCreatedDomainEvent.User.Id;
        
    [JsonConstructor]
    private UserCreatedIntegrationEvent(Guid userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}