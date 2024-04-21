using BackingShop.Application.Core.Errors;
using BackingShop.Application.Core.Extensions;
using FluentValidation;

namespace BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;

/// <summary>
/// Represents the <see cref="ChangePasswordCommand"/> validator.
/// </summary>
public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandValidator"/> class.
    /// </summary>
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(ValidationErrors.ChangePassword.PasswordIsRequired);
    }
}