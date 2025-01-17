﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using BackingShop.Database.Attendee.Data.Interfaces;
using BackingShop.Database.Common;
using BackingShop.Domain.Identity.Entities;

namespace BackingShop.Database.Attendee.Data.Repositories;

/// <summary>
/// Represents the attendee repository.
/// </summary>
internal sealed class AttendeeRepository : GenericRepository<Domain.Identity.Entities.Attendee>, IAttendeeRepository
{
    private readonly BaseDbContext _groupEventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttendeeRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="groupEventDbContext">The group event database context.</param>
    public AttendeeRepository(
        BaseDbContext dbContext,
        BaseDbContext groupEventDbContext)
        : base(dbContext)
    {
        _groupEventDbContext = groupEventDbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Domain.Identity.Entities.Attendee>> GetUnprocessedAsync(int take) =>
        await DbContext.Set<Domain.Identity.Entities.Attendee>()
            .Where(new UnprocessedAttendeeSpecification())
            .OrderBy(attendee => attendee.CreatedOnUtc)
            .Take(take)
            .ToArrayAsync();

    /// <inheritdoc />
    public async Task<(string Email, string Name)[]> GetEmailsAndNamesForGroupEvent(GroupEvent groupEvent)
    {
        if (groupEvent.Id == Guid.Empty)
        {
            return Array.Empty<(string, string)>();
        }

        var attendeeEmailsAndNames = await(
            from @event in _groupEventDbContext.Set<GroupEvent>()
            join attendee in DbContext.Set<Domain.Identity.Entities.Attendee>()
                on groupEvent.Id equals attendee.EventId
            join user in _groupEventDbContext.Set<User>()
                on attendee.UserId equals user.Id
            where @event.Id == groupEvent.Id && attendee.UserId != groupEvent.UserId
            select new
            {
                Email = user.EmailAddress.Value,
                Name = user.FirstName.Value + " " + user.LastName.Value
            }).ToArrayAsync();

        return attendeeEmailsAndNames.Select(x => (x.Email, x.Name)).ToArray();
    }

    /// <inheritdoc />
    public async Task MarkUnprocessedForGroupEventAsync(GroupEvent groupEvent, DateTime utcNow)
    {
        const string sql = @"
                UPDATE Attendee
                SET Processed = 0, ModifiedOnUtc = @ModifiedOn
                WHERE EventId = @EventId AND Deleted = 0";

        SqlParameter[] parameters =
        {
            new("@ModifiedOn", utcNow),
            new("@EventId", groupEvent.Id)
        };

        await DbContext.ExecuteSqlAsync(sql, parameters);
    }

    /// <inheritdoc />
    public async Task RemoveAttendeesForGroupEventAsync(GroupEvent groupEvent, DateTime utcNow)
    {
        const string sql = @"
                UPDATE Attendee
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