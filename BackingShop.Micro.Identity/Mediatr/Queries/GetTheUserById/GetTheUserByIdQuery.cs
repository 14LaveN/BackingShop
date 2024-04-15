using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Micro.Identity.Contracts.Users.Get;

namespace BackingShop.Micro.Identity.Mediatr.Queries.GetTheUserById;

/// <summary>
/// Represents the get user by id query record.
/// </summary>
/// <param name="UserId">The user identifier.</param>
public sealed record GetTheUserByIdQuery(Guid UserId)
    : ICachedQuery<Maybe<List<GetUserResponse>>>
{
    public string Key { get; } = $"get-user-by-{UserId}";
    
    public TimeSpan? Expiration { get; } = TimeSpan.FromMinutes(6);
}