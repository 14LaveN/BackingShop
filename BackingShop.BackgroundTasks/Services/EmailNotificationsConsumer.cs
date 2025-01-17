﻿using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Notifications;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Notification.Data.Interfaces;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Email.Contracts.Emails;

namespace BackingShop.BackgroundTasks.Services;

/// <summary>
/// Represents the emailAddress notifications consumer.
/// </summary>
internal sealed class EmailNotificationsConsumer : IEmailNotificationsConsumer
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly IEmailNotificationService _emailNotificationService;
        
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailNotificationsConsumer"/> class.
    /// </summary>
    /// <param name="notificationRepository">The notification repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public EmailNotificationsConsumer(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IDateTime dateTime,
        IEmailNotificationService emailNotificationService)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _emailNotificationService = emailNotificationService;
    }

    /// <inheritdoc />
    public async Task ConsumeAsync(
        int batchSize,
        int allowedNotificationTimeDiscrepancyInMinutes,
        CancellationToken cancellationToken = default)
    {
        var notificationsToBeSent =
            await _notificationRepository.GetNotificationsToBeSentIncludingUserAndEvent(
                batchSize,
                _dateTime.UtcNow,
                allowedNotificationTimeDiscrepancyInMinutes);

        var sendNotificationEmailTasks = new List<Task>();

        foreach ((Notification notification, Event @event, User user) in notificationsToBeSent)
        {
            Result result = notification.MarkAsSent();

            if (result.IsFailure)
            {
                continue;
            }

            _notificationRepository.Update(notification);

            (string subject, string body) = notification.CreateNotificationEmail(@event, user);

            var notificationEmail = new NotificationEmail(user.EmailAddress, subject, body);

            sendNotificationEmailTasks.Add(_emailNotificationService.SendNotificationEmail(notificationEmail));
        }

        await Task.WhenAll(sendNotificationEmailTasks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}