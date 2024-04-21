using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;

/// <summary>
/// Represents the <see cref="ChangePasswordCommand"/> handler.
/// </summary>
internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Result>
{
    private readonly IUserUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IUserIdentifierProvider _userIdentifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="userIdentifier">The user identifier provider.</param>
    public ChangePasswordCommandHandler(
        IUserUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IUserIdentifierProvider userIdentifier)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userIdentifier = userIdentifier;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return await Result.Failure(passwordResult.Error);
        }

        Maybe<User> maybeUser = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString()) 
                                ?? throw new ArgumentException();

        if (maybeUser.HasNoValue)
        {
            return await Result.Failure(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var passwordHash = _userManager.PasswordHasher.HashPassword(user, passwordResult.Value);

        Result result = user.ChangePassword(passwordHash);

        if (result.IsFailure)
        {
            return await Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}