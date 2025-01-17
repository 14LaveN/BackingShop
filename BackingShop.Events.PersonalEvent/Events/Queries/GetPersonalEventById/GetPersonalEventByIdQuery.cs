﻿using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Events.PersonalEvent.Contracts.PersonalEvents;

namespace BackingShop.Events.PersonalEvent.Events.Queries.GetPersonalEventById;

/// <summary>
/// Represents the query for getting the personal event by identifier.
/// </summary>
public sealed class GetPersonalEventByIdQuery : IQuery<Maybe<DetailedPersonalEventResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonalEventByIdQuery"/> class.
    /// </summary>
    /// <param name="personalEventId">The personal event identifier.</param>
    /// <param name="userId"></param>
    public GetPersonalEventByIdQuery(
        Guid personalEventId,
        Guid userId)
    {
        PersonalEventId = personalEventId;
        UserId = userId;
    }

    /// <summary>
    /// Gets the personal event identifier.
    /// </summary>
    public Guid PersonalEventId { get; }
    
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}