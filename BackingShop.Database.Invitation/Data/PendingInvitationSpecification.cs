using System.Linq.Expressions;
using BackingShop.Database.Common.Specifications;
using BackingShop.Domain.Entities;
using BackingShop.Domain.Identity.Entities;

namespace BackingShop.Database.Invitation.Data;

/// <summary>
/// Represents the specification for determining the pending invitation.
/// </summary>
internal sealed class PendingInvitationSpecification : Specification<Domain.Identity.Entities.Invitation>
{
    private readonly Guid _groupEventId;
    private readonly Guid _userId;

    internal PendingInvitationSpecification(Domain.Identity.Entities.GroupEvent groupEvent, User user)
    {
        _groupEventId = groupEvent.Id;
        _userId = user.Id;
    }

    /// <inheritdoc />
    public override Expression<Func<Domain.Identity.Entities.Invitation, bool>> ToExpression() =>
        invitation => invitation.CompletedOnUtc == null &&
                      invitation.EventId == _groupEventId &&
                      invitation.UserId == _userId;
}