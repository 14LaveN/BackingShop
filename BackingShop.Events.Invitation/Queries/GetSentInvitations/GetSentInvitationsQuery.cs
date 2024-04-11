using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Events.Invitation.Contracts;

namespace BackingShop.Events.Invitation.Queries.GetSentInvitations;

/// <summary>
/// Represents the query for getting the sent invitations for the user identifier.
/// </summary>
public sealed class GetSentInvitationsQuery : IQuery<Maybe<SentInvitationsListResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSentInvitationsQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier provider.</param>
    public GetSentInvitationsQuery(Guid userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}