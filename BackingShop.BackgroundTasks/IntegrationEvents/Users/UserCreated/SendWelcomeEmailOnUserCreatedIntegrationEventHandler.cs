using BackingShop.Application.Core.Abstractions.Notifications;
using BackingShop.BackgroundTasks.Abstractions.Messaging;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Exceptions;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Email.Contracts.Emails;
using BackingShop.RabbitMq.Messaging.User.Events.UserCreated;

namespace BackingShop.BackgroundTasks.IntegrationEvents.Users.UserCreated;

/// <summary>
/// Represents the <see cref="UserCreatedIntegrationEvent"/> handler.
/// </summary>
internal sealed class SendWelcomeEmailOnUserCreatedIntegrationEventHandler 
    : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendWelcomeEmailOnUserCreatedIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public SendWelcomeEmailOnUserCreatedIntegrationEventHandler(
        IUserRepository userRepository,
        IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(notification.UserId);

        if (maybeUser.HasNoValue)
        {
            throw new DomainException(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var welcomeEmail = new WelcomeEmail(user.EmailAddress, user.FullName);

        await _emailNotificationService.SendWelcomeEmail(welcomeEmail);
    }
}