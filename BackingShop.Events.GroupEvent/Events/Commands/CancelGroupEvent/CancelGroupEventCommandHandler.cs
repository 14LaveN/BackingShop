using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.GroupEvent.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;

namespace BackingShop.Events.GroupEvent.Events.Commands.CancelGroupEvent;

/// <summary>
/// Represents the <see cref="CancelGroupEventCommand"/> handler.
/// </summary>
internal sealed class CancelGroupEventCommandHandler : ICommandHandler<CancelGroupEventCommand, Result>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public CancelGroupEventCommandHandler(
        IGroupEventRepository groupEventRepository,
        IUnitOfWork unitOfWork,
        IDateTime dateTime)
    {
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CancelGroupEventCommand request, CancellationToken cancellationToken)
    {
        Maybe<Domain.Identity.Entities.GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId);

        if (maybeGroupEvent.HasNoValue)
        {
            return await Result.Failure(DomainErrors.GroupEvent.NotFound);
        }

        Domain.Identity.Entities.GroupEvent groupEvent = maybeGroupEvent.Value;

        if (groupEvent.UserId != request.UserId)
        {
            return await Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result result = groupEvent.Cancel(_dateTime.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}