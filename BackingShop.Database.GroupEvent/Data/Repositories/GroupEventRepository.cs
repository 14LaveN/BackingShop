using Microsoft.EntityFrameworkCore;
using BackingShop.Database.Common;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Common.Specifications;
using BackingShop.Database.GroupEvent.Data.Interfaces;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Entities;
using BackingShop.Domain.Identity.Entities;

namespace BackingShop.Database.GroupEvent.Data.Repositories;

/// <summary>
/// Represents the group event repository.
/// </summary>
internal sealed class GroupEventRepository : GenericRepository<Domain.Identity.Entities.GroupEvent>, IGroupEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GroupEventRepository(BaseDbContext dbContext)
        : base(dbContext) { }

    /// <inheritdoc/>
    public async Task<Maybe<Domain.Identity.Entities.GroupEvent>> GetGroupEventByName(string name) =>
        (await DbContext.Set<Domain.Identity.Entities.GroupEvent>().FirstOrDefaultAsync(x => x.Name == name))!;
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Domain.Identity.Entities.GroupEvent>> GetForAttendeesAsync(IReadOnlyCollection<Domain.Identity.Entities.Attendee> attendees) =>
        attendees.Count is not 0
            ? await DbContext.Set<Domain.Identity.Entities.GroupEvent>()
                .Where(new GroupEventForAttendeesSpecification(attendees))
                .ToArrayAsync()
            : Array.Empty<Domain.Identity.Entities.GroupEvent>();
}