using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Database.Notification.Data.Interfaces;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.GroupEvent;

namespace BackingShop.Events.GroupEvent.Events.Events.GroupEventCancelled;

/// <summary>
/// Represents the <see cref="GroupEventCancelledDomainEvent"/> class.
/// </summary>
internal sealed class RemoveNotificationsOnGroupEventCancelledDomainEventHandler : IDomainEventHandler<GroupEventCancelledDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveNotificationsOnGroupEventCancelledDomainEventHandler"/> class.
    /// </summary>
    /// <param name="notificationRepository">The notification repository.</param>
    /// <param name="dateTime">The date and time.</param>
    public RemoveNotificationsOnGroupEventCancelledDomainEventHandler(INotificationRepository notificationRepository,
        IDateTime dateTime)
    {
        _notificationRepository = notificationRepository;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task Handle(GroupEventCancelledDomainEvent notification, CancellationToken cancellationToken) =>
        await _notificationRepository.RemoveNotificationsForEventAsync(notification.GroupEvent, _dateTime.UtcNow);
}