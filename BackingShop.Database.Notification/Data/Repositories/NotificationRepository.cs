﻿using BackingShop.Database.Common;
using BackingShop.Database.Notification.Data.Interfaces;
using BackingShop.Domain.Identity.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BackingShop.Database.Notification.Data.Repositories;

/// <summary>
/// Represents the notification repository.
/// </summary>
internal sealed class NotificationRepository : GenericRepository<Domain.Identity.Entities.Notification>, INotificationRepository
{
    private readonly BaseDbContext _eventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="eventDbContext">The some event database context.</param>
    public NotificationRepository(
        BaseDbContext dbContext,
        BaseDbContext eventDbContext)
        : base(dbContext)
    {
        _eventDbContext = eventDbContext;
    }

    /// <inheritdoc />
    public async Task<(Domain.Identity.Entities.Notification Notification, Event Event, User User)[]> GetNotificationsToBeSentIncludingUserAndEvent(
        int batchSize,
        DateTime utcNow,
        int allowedNotificationTimeDiscrepancyInMinutes)
    {
        DateTime startTime = utcNow.AddMinutes(-allowedNotificationTimeDiscrepancyInMinutes);
        DateTime endTime = utcNow.AddMinutes(allowedNotificationTimeDiscrepancyInMinutes);

        var notificationsWithUsersAndEvents = await (
                from notification in DbContext.Set<Domain.Identity.Entities.Notification>()
                join @event in _eventDbContext.Set<Event>()
                    on notification.EventId equals @event.Id
                join user in _eventDbContext.Set<User>()
                    on notification.UserId equals user.Id
                where !notification.Sent &&
                      notification.DateTimeUtc >= startTime &&
                      notification.DateTimeUtc <= endTime
                orderby notification.DateTimeUtc
                select new
                {
                    Notification = notification,
                    Event = @event,
                    User = user
                })
            .Take(batchSize)
            .ToArrayAsync();

        return notificationsWithUsersAndEvents.Select(x => (x.Notification, x.Event, x.User)).ToArray();
    }

    /// <inheritdoc />
    public async Task RemoveNotificationsForEventAsync(Event @event, DateTime utcNow)
    {
        const string sql = @"
                UPDATE Notification
                SET DeletedOnUtc = @DeletedOn, Deleted = @Deleted
                WHERE EventId = @EventId AND Deleted = 0";

        SqlParameter[] parameters =
        {
            new("@DeletedOn", utcNow),
            new("@Deleted", true),
            new("@EventId", @event.Id)
        };

        await DbContext.ExecuteSqlAsync(sql, parameters);
    }
}