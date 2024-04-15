using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.PersonalEvent.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;

namespace BackingShop.Events.PersonalEvent.Events.Commands.UpdatePersonalEvent;

/// <summary>
/// Represents the <see cref="UpdatePersonalEventCommand"/> handler.
/// </summary>
internal sealed class UpdatePersonalEventCommandHandler : ICommandHandler<UpdatePersonalEventCommand, Result>
{
    private readonly IPersonalEventRepository _personalEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePersonalEventCommandHandler"/> class.
    /// </summary>
    /// <param name="personalEventRepository">The personal event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public UpdatePersonalEventCommandHandler(
        IPersonalEventRepository personalEventRepository,
        IUnitOfWork unitOfWork,
        IDateTime dateTime)
    {
        _personalEventRepository = personalEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdatePersonalEventCommand request, CancellationToken cancellationToken)
    {
        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return await  Result.Failure(DomainErrors.PersonalEvent.DateAndTimeIsInThePast);
        }

        Maybe<Domain.Identity.Entities.PersonalEvent> maybePersonalEvent = await _personalEventRepository.GetByIdAsync(request.PersonalEventId);

        if (maybePersonalEvent.HasNoValue)
        {
            return await  Result.Failure(DomainErrors.PersonalEvent.NotFound);
        }

        Domain.Identity.Entities.PersonalEvent personalEvent = maybePersonalEvent.Value;

        if (personalEvent.UserId != request.UserId)
        {
            return await  Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return await  Result.Failure(nameResult.Error);
        }

        personalEvent.ChangeName(nameResult.Value);

        personalEvent.ChangeDateAndTime(request.DateTimeUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
            
        return await Result.Success();
    }
}