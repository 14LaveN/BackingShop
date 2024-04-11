using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Events.Invitation.Contracts;

namespace BackingShop.Events.Invitation.Queries.GetPendingInvitations;

/// <summary>
/// Represents the query for getting the pending invitations for the user identifier.
/// </summary>
public sealed class GetPendingInvitationsQuery : IQuery<Maybe<PendingInvitationsListResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPendingInvitationsQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier provider.</param>
    public GetPendingInvitationsQuery(Guid userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}