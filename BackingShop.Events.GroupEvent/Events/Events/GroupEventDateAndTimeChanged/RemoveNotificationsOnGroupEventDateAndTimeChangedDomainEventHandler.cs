﻿using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Database.Notification.Data.Interfaces;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.GroupEvent;

namespace BackingShop.Events.GroupEvent.Events.Events.GroupEventDateAndTimeChanged;

/// <summary>
/// Represents the <see cref="GroupEventDateAndTimeChangedDomainEvent"/> class.
/// </summary>
public sealed class RemoveNotificationsOnGroupEventDateAndTimeChangedDomainEventHandler
    : IDomainEventHandler<GroupEventDateAndTimeChangedDomainEvent>
{
    private readonly INotificationRepository _dbContext;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveNotificationsOnGroupEventDateAndTimeChangedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The notification repository.</param>
    /// <param name="dateTime">The date and time.</param>
    public RemoveNotificationsOnGroupEventDateAndTimeChangedDomainEventHandler(INotificationRepository dbContext, IDateTime dateTime)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task Handle(GroupEventDateAndTimeChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await _dbContext.RemoveNotificationsForEventAsync(notification.GroupEvent, _dateTime.UtcNow);
}