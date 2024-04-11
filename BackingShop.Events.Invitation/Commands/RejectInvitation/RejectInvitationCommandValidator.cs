using FluentValidation;
using BackingShop.Application.Core.Errors;
using BackingShop.Application.Core.Extensions;

namespace BackingShop.Events.Invitation.Commands.RejectInvitation;

/// <summary>
/// Represents the <see cref="RejectInvitationCommand"/> validator.
/// </summary>
public sealed class RejectInvitationCommandValidator : AbstractValidator<RejectInvitationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RejectInvitationCommandValidator"/> class.
    /// </summary>
    public RejectInvitationCommandValidator() =>
        RuleFor(x => x.InvitationId)
            .NotEmpty()
            .WithError(ValidationErrors.RejectInvitation.InvitationIdIsRequired);
}