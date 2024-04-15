using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.GroupEvent.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;

namespace BackingShop.Events.GroupEvent.Events.Commands.UpdateGroupEvent;

/// <summary>
/// Represents the <see cref="UpdateGroupEventCommand"/> handler.
/// </summary>
public sealed class UpdateGroupEventCommandHandler : ICommandHandler<UpdateGroupEventCommand, Result>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public UpdateGroupEventCommandHandler(
        IGroupEventRepository groupEventRepository,
        IUnitOfWork unitOfWork,
        IDateTime dateTime)
    {
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateGroupEventCommand request, CancellationToken cancellationToken)
    {
        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return await  Result.Failure(DomainErrors.GroupEvent.DateAndTimeIsInThePast);
        }

        Maybe<Domain.Identity.Entities.GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId);

        if (maybeGroupEvent.HasNoValue)
        {
            return await  Result.Failure(DomainErrors.GroupEvent.NotFound);
        }

        Domain.Identity.Entities.GroupEvent groupEvent = maybeGroupEvent.Value;

        if (groupEvent.UserId != request.UserId)
        {
            return await  Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return await  Result.Failure(nameResult.Error);
        }

        groupEvent.ChangeName(nameResult.Value);

        groupEvent.ChangeDateAndTime(request.DateTimeUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
            
        return await Result.Success();
    }
}