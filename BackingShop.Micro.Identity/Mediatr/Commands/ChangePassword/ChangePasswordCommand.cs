using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;

namespace BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;

/// <summary>
/// Represents the change password command record class.
/// </summary>
/// <param name="Password">The password.</param>
public sealed record ChangePasswordCommand(
    string Password) : ICommand<Result>;