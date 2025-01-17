﻿using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;

namespace BackingShop.Events.Invitation.Commands.AcceptInvitation;

/// <summary>
/// Represents the accept invitation command.
/// </summary>
public sealed class AcceptInvitationCommand : ICommand<Result>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptInvitationCommand"/> class.
    /// </summary>
    /// <param name="invitationId">The invitation identifier.</param>
    /// <param name="userId">The user identifier.</param>
    public AcceptInvitationCommand(
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