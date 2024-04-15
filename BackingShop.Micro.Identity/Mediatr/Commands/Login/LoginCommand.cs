using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;

namespace BackingShop.Micro.Identity.Mediatr.Commands.Login;

/// <summary>
/// Represents the login command record class.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Password">The password.</param>
public sealed record LoginCommand(
        string UserName,
        Password Password)
    : ICommand<LoginResponse<Result>>;