﻿using System.ComponentModel.DataAnnotations.Schema;
using BackingShop.Domain.Common.Core.Abstractions;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Common.Core.Primitives;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Entities;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Core.Utility;
using BackingShop.Domain.Identity.Enumerations;
using BackingShop.Domain.Identity.Events.PersonalEvent;
using BackingShop.Domain.Identity.Events.User;
using Microsoft.AspNetCore.Identity;

namespace BackingShop.Domain.Identity.Entities;

/// <summary>
/// Represents the user entity.
/// </summary>
public sealed class User : IdentityUser<Guid>, IAuditableEntity, ISoftDeletableEntity
{
    public override string? PasswordHash { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="firstName">The user first name.</param>
    /// <param name="lastName">The user last name.</param>
    /// <param name="emailAddress">The user emailAddress instance.</param>
    /// <param name="passwordHash">The user password hash.</param>
    public User(
        string userName,
        FirstName firstName,
        LastName lastName,
        EmailAddress emailAddress,
        string passwordHash)
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
        Ensure.NotEmpty(emailAddress, "The emailAddress is required.", nameof(emailAddress));
        Ensure.NotEmpty(passwordHash, "The password hash is required", nameof(passwordHash));
        Ensure.NotEmpty(userName, "The user name is required", nameof(userName));

        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PasswordHash = passwordHash;
    }

    /// <inheritdoc />
    private User() { }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id { get; set; }
    
    /// <summary>
    /// Gets the user first name.
    /// </summary>
    public override string? UserName { get; set; }
    
    /// <summary>
    /// Gets the user first name.
    /// </summary>
    public FirstName FirstName { get; private set; }

    /// <summary>
    /// Gets the user last name.
    /// </summary>
    public LastName LastName { get; private set; }
    
    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<Attendee>? Attendees { get; set; }
    
    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<GroupEvent> YourGroupEvents { get; set; }
    
    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<GroupEvent> AttendeeGroupEvents { get; set; }
    
    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<PersonalEvent> PersonalEvents { get; set; }

    /// <summary>
    /// Gets the user full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets the user emailAddress.
    /// </summary>
    public EmailAddress EmailAddress { get; set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }

    /// <inheritdoc />
    public bool Deleted { get; }

    /// <inheritdoc cref="RefreshToken" />
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The domain events.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the domain events. This collection is readonly.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Gets or sets first name.
    /// </summary>
    public string? Firstname { get; set; }

    /// <summary>
    /// Clears all the domain events from the <see cref="AggregateRoot"/>.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
        
    /// <summary>
    /// Adds the specified <see cref="IDomainEvent"/> to the <see cref="AggregateRoot"/>.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Creates a new user with the specified first name, last name, emailAddress and password hash.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="userName">The user name.</param>
    /// <param name="emailAddress">The emailAddress.</param>
    /// <param name="passwordHash">The password hash.</param>
    /// <returns>The newly created user instance.</returns>
    public static User Create(
        FirstName firstName,
        LastName lastName,
        string userName,
        EmailAddress emailAddress,
        string passwordHash)
    {
        var user = new User(userName, firstName, lastName, emailAddress, passwordHash);

        user.AddDomainEvent(new UserCreatedDomainEvent(user));

        return user;
    }

    /// <summary>
    /// Creates a new personal event.
    /// </summary>
    /// <param name="name">The event name.</param>
    /// <param name="category">The event category.</param>
    /// <param name="dateTimeUtc">The date and time of the event.</param>
    /// <returns>The newly created personal event.</returns>
    public PersonalEvent CreatePersonalEvent(
        Name name,
        Category category,
        DateTime dateTimeUtc)
    {
        var personalEvent = new PersonalEvent(this, name, category, dateTimeUtc);

        AddDomainEvent(new PersonalEventCreatedDomainEvent(personalEvent));

        return personalEvent;
    }

    /// <summary>
    /// Changes the users password.
    /// </summary>
    /// <param name="passwordHash">The password hash of the new password.</param>
    /// <returns>The success result or an error.</returns>
    public Result ChangePassword(string passwordHash)
    {
        if (passwordHash == PasswordHash)
        {
            return Result.Failure(DomainErrors.User.CannotChangePassword).GetAwaiter().GetResult();
        }

        PasswordHash = passwordHash;

        AddDomainEvent(new UserPasswordChangedDomainEvent(this));

        return Result.Success().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Changes the users first and last name.
    /// </summary>
    /// <param name="firstName">The new first name.</param>
    /// <param name="lastName">The new last name.</param>
    public void ChangeName(FirstName firstName, LastName lastName)
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));

        FirstName = firstName;

        LastName = lastName;

        AddDomainEvent(new UserNameChangedDomainEvent(this));
    }
}