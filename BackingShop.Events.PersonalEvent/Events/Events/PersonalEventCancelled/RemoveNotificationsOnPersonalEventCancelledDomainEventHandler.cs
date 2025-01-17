﻿using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Database.Notification.Data.Interfaces;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.PersonalEvent;

namespace BackingShop.Events.PersonalEvent.Events.Events.PersonalEventCancelled;

/// <summary>
/// Represents the <see cref="PersonalEventCancelledDomainEvent"/> class.
/// </summary>
internal sealed class RemoveNotificationsOnPersonalEventCancelledDomainEventHandler
    : IDomainEventHandler<PersonalEventCancelledDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveNotificationsOnPersonalEventCancelledDomainEventHandler"/> class.
    /// </summary>
    /// <param name="notificationRepository">The notification repository.</param>
    /// <param name="dateTime">The date and time.</param>
    public RemoveNotificationsOnPersonalEventCancelledDomainEventHandler(
        INotificationRepository notificationRepository,
        IDateTime dateTime)
    {
        _notificationRepository = notificationRepository;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task Handle(PersonalEventCancelledDomainEvent notification, CancellationToken cancellationToken) =>
        await _notificationRepository.RemoveNotificationsForEventAsync(notification.PersonalEvent, _dateTime.UtcNow);
}