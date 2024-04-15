﻿using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;

namespace BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;

/// <summary>
/// Represents the change password command record class.
/// </summary>
/// <param name="UserId">The user identifier./</param>
/// <param name="Password">The password.</param>
public sealed record ChangePasswordCommand(
    Guid UserId,
    string Password) : ICommand<Result>;