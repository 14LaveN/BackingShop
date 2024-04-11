using Microsoft.EntityFrameworkCore;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Domain.Identity.Enumerations;
using BackingShop.Events.GroupEvent.Contracts.GroupEvents;
using BackingShop.Contracts.Common;

namespace BackingShop.Events.GroupEvent.Events.Queries.GetGroupEvents;

/// <summary>
/// Represents the <see cref="GetGroupEventsQuery"/> handler.
/// </summary>
internal sealed class GetGroupEventsQueryHandler : IQueryHandler<GetGroupEventsQuery, Maybe<PagedList<GroupEventResponse>>>
{
    private readonly IDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupEventsQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GetGroupEventsQueryHandler(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Maybe<PagedList<GroupEventResponse>>> Handle(GetGroupEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Maybe<PagedList<GroupEventResponse>>.None;
        }

        bool shouldSearchCategory = request.CategoryId != null && Category.ContainsValue(request.CategoryId.Value);

        IQueryable<GroupEventResponse> groupEventResponsesQuery =
            from groupEvent in _dbContext.Set<Domain.Identity.Entities.GroupEvent>().AsNoTracking()
            join user in _dbContext.Set<User>().AsNoTracking()
                on groupEvent.UserId equals user.Id
            where groupEvent.UserId == request.UserId &&
                  !groupEvent.Cancelled &&
                  (!shouldSearchCategory || groupEvent.Category.Value == request.CategoryId) &&
                  (request.Name == null || request.Name == "" || groupEvent.Name.Value.Contains(request.Name)) &&
                  (request.StartDate == null || groupEvent.DateTimeUtc >= request.StartDate) &&
                  (request.EndDate == null || groupEvent.DateTimeUtc <= request.EndDate)
            orderby groupEvent.DateTimeUtc descending
            select new GroupEventResponse
            {
                Id = groupEvent.Id,
                Name = groupEvent.Name.Value,
                CategoryId = groupEvent.Category.Value,
                DateTimeUtc = groupEvent.DateTimeUtc,
                CreatedOnUtc = groupEvent.CreatedOnUtc
            };

        int totalCount = await groupEventResponsesQuery.CountAsync(cancellationToken);

        GroupEventResponse[] groupEventResponsesPage = await groupEventResponsesQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        foreach (GroupEventResponse groupEventResponse in groupEventResponsesPage)
        {
            groupEventResponse.Category = Category.FromValue(groupEventResponse.CategoryId).Value.Name;
        }

        return new PagedList<GroupEventResponse>(groupEventResponsesPage, request.Page, request.PageSize, totalCount);
    }
}