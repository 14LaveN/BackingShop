using FluentValidation;
using BackingShop.Application.Core.Errors;
using BackingShop.Application.Core.Extensions;

namespace BackingShop.Events.PersonalEvent.Events.Commands.CancelPersonalEvent;

/// <summary>
/// Represents the <see cref="CancelPersonalEventCommand"/> validator.
/// </summary>
public sealed class CancelGroupEventCommandValidator : AbstractValidator<CancelPersonalEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelGroupEventCommandValidator"/> class.
    /// </summary>
    public CancelGroupEventCommandValidator() =>
        RuleFor(x => x.PersonalEventId)
            .NotEmpty()
            .WithError(ValidationErrors.CancelPersonalEvent.PersonalEventIdIsRequired);
}