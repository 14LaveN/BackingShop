﻿using System.Globalization;
using BackingShop.Application.Core.Abstractions.Notifications;
using BackingShop.BackgroundTasks.Abstractions.Messaging;
using BackingShop.Database.Attendee.Data.Interfaces;
using BackingShop.Database.GroupEvent.Data.Interfaces;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Exceptions;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Email.Contracts.Emails;
using BackingShop.Events.Attendee.Events.AttendeeCreated;

namespace BackingShop.BackgroundTasks.IntegrationEvents.Attendees.AttendeeCreated
{
    /// <summary>
    /// Represents the <see cref="AttendeeCreatedIntegrationEvent"/> handler.
    /// </summary>
    internal sealed class NotifyAttendeeOnAttendeeCreatedIntegrationEventHandler : IIntegrationEventHandler<AttendeeCreatedIntegrationEvent>
    {
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IGroupEventRepository _groupEventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailNotificationService _emailNotificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyAttendeeOnAttendeeCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="attendeeRepository">The attendee repository.</param>
        /// <param name="groupEventRepository">The group event repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="emailNotificationService">The email notification service.</param>
        public NotifyAttendeeOnAttendeeCreatedIntegrationEventHandler(
            IAttendeeRepository attendeeRepository,
            IGroupEventRepository groupEventRepository,
            IUserRepository userRepository,
            IEmailNotificationService emailNotificationService)
        {
            _emailNotificationService = emailNotificationService;
            _attendeeRepository = attendeeRepository;
            _groupEventRepository = groupEventRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task Handle(AttendeeCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            Maybe<Attendee> maybeAttendee = await _attendeeRepository.GetByIdAsync(notification.AttendeeId);

            if (maybeAttendee.HasNoValue)
            {
                throw new DomainException(DomainErrors.Attendee.NotFound);
            }

            Attendee attendee = maybeAttendee.Value;

            Maybe<GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(attendee.EventId);

            if (maybeGroupEvent.HasNoValue)
            {
                throw new DomainException(DomainErrors.GroupEvent.NotFound);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(attendee.UserId);

            if (maybeUser.HasNoValue)
            {
                throw new DomainException(DomainErrors.User.NotFound);
            }

            GroupEvent groupEvent = maybeGroupEvent.Value;

            User user = maybeUser.Value;

            var attendeeCreatedEmail = new AttendeeCreatedEmail(
                user.Email,
                user.FullName,
                groupEvent.Name,
                groupEvent.DateTimeUtc.ToString(CultureInfo.InvariantCulture));

            await _emailNotificationService.SendAttendeeCreatedEmail(attendeeCreatedEmail);
        }
    }
}
