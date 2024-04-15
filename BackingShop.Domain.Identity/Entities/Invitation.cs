using BackingShop.Domain.Common.Core.Abstractions;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Core.Utility;
using BackingShop.Domain.Identity.Events.Invitation;

namespace BackingShop.Domain.Identity.Entities;

/// <summary>
/// Represents the invitation to a group event.
/// </summary>
public sealed class Invitation : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Invitation"/> class.
    /// </summary>
    /// <param name="event">The group event.</param>
    /// <param name="user">The user.</param>
    public Invitation(GroupEvent @event, User user)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(@event, "The event is required.", nameof(@event));
        Ensure.NotNull(user, "The event is required.", nameof(user));

        EventId = @event.Id;
        UserId = user.Id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Invitation"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Invitation()
    {
    }

    /// <summary>
    /// Gets the event identifier.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the value indicating whether or not the invitation has been accepted.
    /// </summary>
    public bool Accepted { get; private set; }

    /// <summary>
    /// Gets the value indicating whether or not the invitation has been rejected.
    /// </summary>
    public bool Rejected { get; private set; }

    /// <summary>
    /// Gets the date and time the invitation was completed on in UTC format.
    /// </summary>
    public DateTime? CompletedOnUtc { get; private set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }

    /// <inheritdoc />
    public bool Deleted { get; }

    /// <summary>
    /// Accepts the invitation.
    /// </summary>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    /// <returns>The success result or an error.</returns>
    public Result Accept(DateTime utcNow)
    {
        if (Accepted)
        {
            return Result.Failure(DomainErrors.Invitation.AlreadyAccepted).GetAwaiter().GetResult();
        }

        if (Rejected)
        {
            return Result.Failure(DomainErrors.Invitation.AlreadyRejected).GetAwaiter().GetResult();
        }

        Accepted = true;

        CompletedOnUtc = utcNow;

        AddDomainEvent(new InvitationAcceptedDomainEvent(this));

        return Result.Success().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Rejects the invitation.
    /// </summary>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    /// <returns>The success result or an error.</returns>
    public Result Reject(DateTime utcNow)
    {
        if (Accepted)
        {
            return Result.Failure(DomainErrors.Invitation.AlreadyAccepted).GetAwaiter().GetResult();
        }

        if (Rejected)
        {
            return Result.Failure(DomainErrors.Invitation.AlreadyRejected).GetAwaiter().GetResult();
        }

        Rejected = true;

        CompletedOnUtc = utcNow;

        AddDomainEvent(new InvitationRejectedDomainEvent(this));

        return Result.Success().GetAwaiter().GetResult();
    }
}