﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using BackingShop.Database.Common;
using BackingShop.Database.Invitation.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Entities;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Identity.Events.Invitation;

namespace BackingShop.Database.Invitation.Data.Repositories;

/// <summary>
/// Represents the invitation repository.
/// </summary>
internal sealed class InvitationRepository : GenericRepository<Domain.Identity.Entities.Invitation>, IInvitationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public InvitationRepository(BaseDbContext dbContext)
        : base(dbContext) { }

    /// <inheritdoc />
    public async Task<Result<Domain.Identity.Entities.Invitation>> InviteAsync(
        GroupEvent groupEvent,
        User user)
    {
        if (await CheckIfInvitationAlreadySentAsync(groupEvent, user))
        {
            return Result.Failure<Domain.Identity.Entities.Invitation>(DomainErrors.GroupEvent.InvitationAlreadySent);
        }

        var invitation = new Domain.Identity.Entities.Invitation(groupEvent, user);

        invitation.AddDomainEvent(new InvitationSentDomainEvent(invitation));

        return invitation;
    }


    /// <inheritdoc />
    public async Task<bool> CheckIfInvitationAlreadySentAsync(GroupEvent groupEvent, User user) =>
        await AnyAsync(new PendingInvitationSpecification(groupEvent, user));

    /// <inheritdoc />
    public async Task RemoveInvitationsForGroupEventAsync(GroupEvent groupEvent, DateTime utcNow)
    {
        const string sql = @"
                UPDATE Invitation
                SET DeletedOnUtc = @DeletedOn, Deleted = @Deleted
                WHERE EventId = @EventId AND Deleted = 0";

        SqlParameter[] parameters =
        {
            new("@DeletedOn", utcNow),
            new("@Deleted", true),
            new("@EventId", groupEvent.Id)
        };

        await DbContext.ExecuteSqlAsync(sql, parameters);
    }
}