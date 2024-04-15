using MediatR;
using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.GroupEvent.Data.Interfaces;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Identity.Enumerations;

namespace BackingShop.Events.GroupEvent.Events.Commands.CreateGroupEvent;

/// <summary>
/// Represents the <see cref="CreateGroupEventCommand"/> handler.
/// </summary>
internal sealed class CreateGroupEventCommandHandler : ICommandHandler<CreateGroupEventCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public CreateGroupEventCommandHandler(
        IUserRepository userRepository,
        IGroupEventRepository groupEventRepository,
        IUnitOfWork unitOfWork,
        IDateTime dateTime)
    {
        _userRepository = userRepository;
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CreateGroupEventCommand request, CancellationToken cancellationToken)
    {
        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return await  Result.Failure(DomainErrors.GroupEvent.DateAndTimeIsInThePast);
        }

        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

        if (maybeUser.HasNoValue)
        {
            return await  Result.Failure(DomainErrors.User.NotFound);
        }

        Maybe<Category> maybeCategory = Category.FromValue(request.CategoryId);

        if (maybeCategory.HasNoValue)
        {
            return await  Result.Failure(DomainErrors.Category.NotFound);
        }

        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return await  Result.Failure(nameResult.Error);
        }

        var groupEvent = Domain.Identity.Entities.GroupEvent.Create(maybeUser.Value, nameResult.Value, maybeCategory.Value, request.DateTimeUtc);

        await _groupEventRepository.Insert(groupEvent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}