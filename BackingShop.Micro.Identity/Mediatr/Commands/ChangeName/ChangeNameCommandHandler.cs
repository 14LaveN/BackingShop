using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace BackingShop.Micro.Identity.Mediatr.Commands.ChangeName;

/// <summary>
/// Represents the <see cref="ChangeNameCommand"/> handler.
/// </summary>
internal sealed class ChangeNameCommandHandler : ICommandHandler<ChangeNameCommand, IBaseResponse<Result>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IUserIdentifierProvider _userIdentifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeNameCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="userIdentifier">The user identifier provider.</param>
    public ChangeNameCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IUserIdentifierProvider userIdentifier)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userIdentifier = userIdentifier;
    }

    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        Result<FirstName> nameResult = FirstName.Create(request.FirstName);

        if (nameResult.IsFailure)
        {
            return new BaseResponse<Result>
            {
                Data = await Result.Failure(nameResult.Error),
                StatusCode = StatusCode.InternalServerError,
                Description = "First Name result is failure."
            };
        }
        
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

        if (lastNameResult.IsFailure)
        {
            return new BaseResponse<Result>
            {
                Data =await Result.Failure(lastNameResult.Error),
                StatusCode = StatusCode.InternalServerError,
                Description = "Last Name result is failure."
            };
        }
        
        Maybe<User> maybeUser = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString()) 
                                ?? throw new ArgumentException();

        if (maybeUser.HasNoValue)
        {
            return new BaseResponse<Result>
            {
                Data =await Result.Failure(DomainErrors.User.NotFound),
                StatusCode = StatusCode.NotFound,
                Description = "User not found."
            };
        }

        User user = maybeUser.Value;

        user.ChangeName(request.FirstName,request.LastName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BaseResponse<Result>
        {
            Data =await Result.Success(),
            Description = "Change name.",
            StatusCode = StatusCode.Ok
        };
    }
}