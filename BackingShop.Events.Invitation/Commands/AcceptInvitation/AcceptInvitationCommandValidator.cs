using FluentValidation;
using BackingShop.Application.Core.Errors;
using BackingShop.Application.Core.Extensions;

namespace BackingShop.Events.Invitation.Commands.AcceptInvitation;

/// <summary>
/// Represents the <see cref="AcceptInvitationCommand"/> validator.
/// </summary>
public sealed class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptInvitationCommandValidator"/> class.
    /// </summary>
    public AcceptInvitationCommandValidator() =>
        RuleFor(x => x.InvitationId)
            .NotEmpty()
            .WithError(ValidationErrors.AcceptInvitation.InvitationIdIsRequired);
}