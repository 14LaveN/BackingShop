﻿using BackingShop.Application.Core.Abstractions.Notifications;
using BackingShop.BackgroundTasks.Abstractions.Messaging;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Exceptions;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Email.Contracts.Emails;
using BackingShop.RabbitMq.Messaging.User.Events.PasswordChanged;

namespace BackingShop.BackgroundTasks.IntegrationEvents.Users.UserPasswordChanged;

/// <summary>
/// Represents the <see cref="UserPasswordChangedIntegrationEvent"/> handler.
/// </summary>
internal sealed class NotifyUserOnPasswordChangedIntegrationEventHandler
    : IIntegrationEventHandler<UserPasswordChangedIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotifyUserOnPasswordChangedIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public NotifyUserOnPasswordChangedIntegrationEventHandler(
        IUserRepository userRepository,
        IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task Handle(UserPasswordChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(notification.UserId);

        if (maybeUser.HasNoValue)
        {
            throw new DomainException(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var passwordChangedEmail = new PasswordChangedEmail(user.EmailAddress, user.FullName);

        await _emailNotificationService.SendPasswordChangedEmail(passwordChangedEmail);
    }
}