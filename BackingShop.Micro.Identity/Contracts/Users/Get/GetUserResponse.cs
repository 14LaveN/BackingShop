namespace BackingShop.Micro.Identity.Contracts.Users.Get;

public sealed record GetUserResponse(
    string Description,
    string Name,
    DateTime CreatedAt);