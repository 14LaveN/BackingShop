﻿using Microsoft.EntityFrameworkCore;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Identity.Enumerations;
using BackingShop.Events.PersonalEvent.Contracts.PersonalEvents;
using BackingShop.Contracts.Common;

namespace BackingShop.Events.PersonalEvent.Events.Queries.GetPersonalEvents;

/// <summary>
/// Represents the <see cref="GetPersonalEventsQuery"/> handler.
/// </summary>
internal sealed class GetPersonalEventsQueryHandler : IQueryHandler<GetPersonalEventsQuery, Maybe<PagedList<PersonalEventResponse>>>
{
    private readonly IDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonalEventsQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GetPersonalEventsQueryHandler(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<PagedList<PersonalEventResponse>>> Handle(
        GetPersonalEventsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<PagedList<PersonalEventResponse>>.None;
        }

        bool shouldSearchCategory = request.CategoryId != null && Category.ContainsValue(request.CategoryId.Value);

        IQueryable<PersonalEventResponse> personalEventResponses =
            from personalEvent in _dbContext.Set<Domain.Identity.Entities.PersonalEvent>().AsNoTracking()
            join user in _dbContext.Set<User>().AsNoTracking()
                on personalEvent.UserId equals user.Id
            where user.Id == request.UserId &&
                  personalEvent.UserId == request.UserId &&
                  !personalEvent.Cancelled &&
                  (!shouldSearchCategory || personalEvent.Category.Value == request.CategoryId) &&
                  (request.Name == null || request.Name == "" || personalEvent.Name.Value.Contains(request.Name)) &&
                  (request.StartDate == null || personalEvent.DateTimeUtc >= request.StartDate) &&
                  (request.EndDate == null || personalEvent.DateTimeUtc <= request.EndDate)
            orderby personalEvent.DateTimeUtc descending
            select new PersonalEventResponse
            {
                Id = personalEvent.Id,
                Name = personalEvent.Name.Value,
                CategoryId = personalEvent.Category.Value,
                DateTimeUtc = personalEvent.DateTimeUtc,
                CreatedOnUtc = personalEvent.CreatedOnUtc
            };

        int totalCount = await personalEventResponses.CountAsync(cancellationToken);

        PersonalEventResponse[] personalEventResponsesPage = await personalEventResponses
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        foreach (PersonalEventResponse personalEventResponse in personalEventResponsesPage)
        {
            personalEventResponse.Category = Category.FromValue(personalEventResponse.CategoryId).Value.Name;
        }

        return new PagedList<PersonalEventResponse>(personalEventResponsesPage, request.Page, request.PageSize, totalCount);
    }
}