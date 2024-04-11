using BackingShop.Database.Attendee.Data.Interfaces;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Domain.Common.Core.Events;
using BackingShop.Domain.Identity.Events.GroupEvent;

namespace BackingShop.Events.GroupEvent.Events.Events.AddToGroupEventAttendee;

/// <summary>
/// Represents the <see cref="AddToGroupEventAttendeeDomainEvent"/> class handler.
/// </summary>
internal sealed class AddAttendeeOnAddToGroupEventAttendeeDomainEventHandler
    : IDomainEventHandler<AddToGroupEventAttendeeDomainEvent>
{
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAttendeeOnAddToGroupEventAttendeeDomainEventHandler"/> class.
    /// </summary>
    /// <param name="attendeeRepository">The attendee repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public AddAttendeeOnAddToGroupEventAttendeeDomainEventHandler(
        IAttendeeRepository attendeeRepository,
        IUnitOfWork unitOfWork)
    {
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
    }
    
    /// <inheritdoc />
    public async Task Handle(AddToGroupEventAttendeeDomainEvent notification, CancellationToken cancellationToken)
    {
        await _attendeeRepository.Insert(notification.Attendee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}