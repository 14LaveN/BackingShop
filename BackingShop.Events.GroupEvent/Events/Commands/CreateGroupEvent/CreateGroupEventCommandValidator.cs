﻿using FluentValidation;
using BackingShop.Application.Core.Errors;
using BackingShop.Application.Core.Extensions;

namespace BackingShop.Events.GroupEvent.Events.Commands.CreateGroupEvent;

/// <summary>
/// Represents the <see cref="CreateGroupEventCommand"/> validator.
/// </summary>
public sealed class CreateGroupEventCommandValidator : AbstractValidator<CreateGroupEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupEventCommandValidator"/> class.
    /// </summary>
    public CreateGroupEventCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(ValidationErrors.CreateGroupEvent.UserIdIsRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(ValidationErrors.CreateGroupEvent.NameIsRequired);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithError(ValidationErrors.CreateGroupEvent.CategoryIdIsRequired);

        RuleFor(x => x.DateTimeUtc)
            .NotEmpty()
            .WithError(ValidationErrors.CreateGroupEvent.DateAndTimeIsRequired);
    }
}