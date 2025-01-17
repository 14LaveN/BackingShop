using BackingShop.Database.Common;
using BackingShop.Database.PersonalEvent.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackingShop.Database.PersonalEvent.Data.Repositories;

/// <summary>
/// Represents the attendee repository.
/// </summary>
internal sealed class PersonalEventRepository : GenericRepository<Domain.Identity.Entities.PersonalEvent>, IPersonalEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PersonalEventRepository(BaseDbContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Domain.Identity.Entities.PersonalEvent>> GetUnprocessedAsync(int take) =>
        await DbContext.Set<Domain.Identity.Entities.PersonalEvent>()
            .Where(new UnProcessedPersonalEventSpecification())
            .OrderBy(personalEvent => personalEvent.CreatedOnUtc)
            .Take(take)
            .ToArrayAsync();
}