﻿using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Database.PersonalEvent.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Identity.Enumerations;

namespace BackingShop.Events.PersonalEvent.Events.Commands.CreatePersonalEvent;

/// <summary>
/// Represents the <see cref="CreatePersonalEventCommand"/> handler.
/// </summary>
internal sealed class CreatePersonalEventCommandHandler : ICommandHandler<CreatePersonalEventCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePersonalEventCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="personalEventRepository">The personal event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public CreatePersonalEventCommandHandler(
        IUserRepository userRepository,
        IPersonalEventRepository personalEventRepository,
        IUnitOfWork unitOfWork,
        IDateTime dateTime)
    {
        _userRepository = userRepository;
        _personalEventRepository = personalEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CreatePersonalEventCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != Guid.Empty)
        {
            return await  Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return await  Result.Failure(DomainErrors.PersonalEvent.DateAndTimeIsInThePast);
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

        User user = maybeUser.Value;

        Domain.Identity.Entities.PersonalEvent personalEvent = user.CreatePersonalEvent(nameResult.Value, maybeCategory.Value, request.DateTimeUtc);

        await _personalEventRepository.Insert(personalEvent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}