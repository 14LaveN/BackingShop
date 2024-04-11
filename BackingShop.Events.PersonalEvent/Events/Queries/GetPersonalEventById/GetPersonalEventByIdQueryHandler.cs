using Microsoft.EntityFrameworkCore;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Identity;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Identity.Enumerations;
using BackingShop.Events.PersonalEvent.Contracts.PersonalEvents;

namespace BackingShop.Events.PersonalEvent.Events.Queries.GetPersonalEventById;

/// <summary>
/// Represents the <see cref="GetPersonalEventByIdQuery"/> handler.
/// </summary>
internal sealed class GetPersonalEventByIdQueryHandler : IQueryHandler<GetPersonalEventByIdQuery, Maybe<DetailedPersonalEventResponse>>
{
    private readonly IDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonalEventByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GetPersonalEventByIdQueryHandler(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<DetailedPersonalEventResponse>> Handle(
        GetPersonalEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PersonalEventId == Guid.Empty)
        {
            return Maybe<DetailedPersonalEventResponse>.None;
        }

        DetailedPersonalEventResponse? response = await (
            from personalEvent in _dbContext.Set<Domain.Identity.Entities.PersonalEvent>().AsNoTracking()
            join user in _dbContext.Set<User>().AsNoTracking()
                on personalEvent.UserId equals user.Id
            where user.Id == request.UserId &&
                  personalEvent.Id == request.PersonalEventId &&
                  !personalEvent.Cancelled
            select new DetailedPersonalEventResponse
            {
                Id = personalEvent.Id,
                Name = personalEvent.Name.Value,
                CategoryId = personalEvent.Category.Value,
                CreatedBy = user.FirstName.Value + " " + user.LastName.Value,
                DateTimeUtc = personalEvent.DateTimeUtc,
                CreatedOnUtc = personalEvent.CreatedOnUtc
            }).FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Maybe<DetailedPersonalEventResponse>.None;
        }

        response.Category = Category.FromValue(response.CategoryId).Value.Name;
            
        return response;
    }
}