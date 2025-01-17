﻿using Microsoft.EntityFrameworkCore;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Events.Invitation.Contracts;

namespace BackingShop.Events.Invitation.Queries.GetInvitationById;

/// <summary>
/// Represents the <see cref="GetInvitationByIdQuery"/> handler.
/// </summary>
internal sealed class GetInvitationByIdQueryHandler : IQueryHandler<GetInvitationByIdQuery, Maybe<InvitationResponse>>
{
    private readonly BaseDbContext _invitationDbContext;
    private readonly BaseDbContext _groupEventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInvitationByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="invitationDbContext">The invitations database context.</param>
    /// <param name="groupEventDbContext">The group events database context.</param>
    public GetInvitationByIdQueryHandler(
        BaseDbContext invitationDbContext,
        BaseDbContext groupEventDbContext)
    {
        _invitationDbContext = invitationDbContext;
        _groupEventDbContext = groupEventDbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<InvitationResponse>> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.InvitationId == Guid.Empty)
        {
            return Maybe<InvitationResponse>.None;
        }

        InvitationResponse? response = await (
            from invitation in _invitationDbContext.Set<Domain.Identity.Entities.Invitation>().AsNoTracking()
            join user in _invitationDbContext.Set<User>().AsNoTracking()
                on invitation.UserId equals user.Id
            join groupEvent in _groupEventDbContext.Set<GroupEvent>().AsNoTracking()
                on invitation.EventId equals groupEvent.Id
            join friend in _invitationDbContext.Set<User>().AsNoTracking()
                on groupEvent.UserId equals friend.Id
            where invitation.Id == request.InvitationId &&
                  invitation.UserId == request.UserId &&
                  invitation.CompletedOnUtc == null
            select new InvitationResponse
            {
                Id = invitation.Id,
                EventName = groupEvent.Name.Value,
                EventDateTimeUtc = groupEvent.DateTimeUtc,
                FriendName = friend.FirstName.Value + " " + friend.LastName.Value,
                CreatedOnUtc = invitation.CreatedOnUtc
            }).FirstOrDefaultAsync(cancellationToken);
            
        return response!;
    }
}