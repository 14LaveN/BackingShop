﻿using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Database.Attendee.Data.Interfaces;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.GroupEvent;

namespace BackingShop.Events.GroupEvent.Events.Events.GroupEventCancelled;

/// <summary>
/// Represents the <see cref="GroupEventCancelledDomainEvent"/> handler.
/// </summary>
internal sealed class RemoveAttendeesOnGroupEventCancelledDomainEventHandler : IDomainEventHandler<GroupEventCancelledDomainEvent>
{
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveAttendeesOnGroupEventCancelledDomainEventHandler"/> class.
    /// </summary>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="dateTime">The date and time.</param>
    public RemoveAttendeesOnGroupEventCancelledDomainEventHandler(IAttendeeRepository attendeeRepository, IDateTime dateTime)
    {
        _attendeeRepository = attendeeRepository;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task Handle(GroupEventCancelledDomainEvent notification, CancellationToken cancellationToken) =>
        await _attendeeRepository.RemoveAttendeesForGroupEventAsync(notification.GroupEvent, _dateTime.UtcNow);
}