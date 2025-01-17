﻿using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Events.Invitation.Contracts;

namespace BackingShop.Events.Invitation.Queries.GetInvitationById;

/// <summary>
/// Represents the query for getting the invitation by the identifier.
/// </summary>
public sealed class GetInvitationByIdQuery : IQuery<Maybe<InvitationResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetInvitationByIdQuery"/> class.
    /// </summary>
    /// <param name="invitationId">The invitation identifier.</param>
    /// <param name="userId">The user identifier.</param>
    public GetInvitationByIdQuery(
        Guid invitationId,
        Guid userId)
    {
        InvitationId = invitationId;
        UserId = userId;
    }

    /// <summary>
    /// Gets the invitation identifier.
    /// </summary>
    public Guid InvitationId { get; }
        
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}