﻿using Microsoft.EntityFrameworkCore;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Events.Invitation.Contracts;
using BackingShop.Events.Invitation.Queries.GetPendingInvitations;

namespace BackingShop.Events.Invitation.Queries.GetSentInvitations;

/// <summary>
/// Represents the <see cref="GetPendingInvitationsQuery"/> handler.
/// </summary>
internal sealed class GetSentInvitationsQueryHandler
    : IQueryHandler<GetSentInvitationsQuery, Maybe<SentInvitationsListResponse>>
{
    private readonly BaseDbContext _invitationDbContext;
    private readonly BaseDbContext _groupEventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSentInvitationsQueryHandler"/> class.
    /// </summary>
    /// <param name="invitationDbContext">The invitations database context.</param>
    /// <param name="userDbContext">The users database context.</param>
    /// <param name="groupEventDbContext">The group events database context.</param>
    public GetSentInvitationsQueryHandler(
        BaseDbContext invitationDbContext,
        BaseDbContext groupEventDbContext)
    {
        _invitationDbContext = invitationDbContext;
        _groupEventDbContext = groupEventDbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<SentInvitationsListResponse>> Handle(
        GetSentInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<SentInvitationsListResponse>.None;
        }

        SentInvitationsListResponse.SentInvitationModel[] invitations = await (
            from invitation in _invitationDbContext.Set<Domain.Identity.Entities.Invitation>().AsNoTracking()
            join friend in _invitationDbContext.Set<User>().AsNoTracking()
                on invitation.UserId equals friend.Id
            join groupEvent in _groupEventDbContext.Set<GroupEvent>().AsNoTracking()
                on invitation.EventId equals groupEvent.Id
            join user in _invitationDbContext.Set<User>().AsNoTracking()
                on groupEvent.UserId equals user.Id
            where user.Id == request.UserId &&
                  groupEvent.UserId == request.UserId &&
                  invitation.CompletedOnUtc == null
            select new SentInvitationsListResponse.SentInvitationModel
            {
                Id = invitation.Id,
                FriendId = friend.Id,
                FriendName = friend.FirstName.Value + " " + friend.LastName.Value,
                EventName = groupEvent.Name.Value,
                EventDateTimeUtc = groupEvent.DateTimeUtc,
                CreatedOnUtc = invitation.CreatedOnUtc
            }).ToArrayAsync(cancellationToken);

        var response = new SentInvitationsListResponse(invitations);

        return response;
    }
}