using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;

namespace BackingShop.Events.GroupEvent.Events.Commands.CancelGroupEvent;

/// <summary>
/// Represents the cancel group event command.
/// </summary>
public sealed record CancelGroupEventCommand(
        Guid GroupEventId,
        Guid UserId) : ICommand<Result>;