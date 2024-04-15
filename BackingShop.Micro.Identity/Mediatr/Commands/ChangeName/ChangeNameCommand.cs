using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;

namespace BackingShop.Micro.Identity.Mediatr.Commands.ChangeName;

/// <summary>
/// Represents the change <see cref="Name"/> command record.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="UserId">The user identifier.</param>
public sealed record ChangeNameCommand(
    FirstName FirstName,
    LastName LastName,
    Guid UserId)
    : ICommand<IBaseResponse<Result>>;