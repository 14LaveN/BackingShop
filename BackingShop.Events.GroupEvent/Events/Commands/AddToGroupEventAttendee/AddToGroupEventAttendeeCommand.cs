using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Identity.Entities;

namespace BackingShop.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;

/// <summary>
/// Represents the add to group event attendee command record class.
/// </summary>
/// <param name="GroupEventId">The group event identifier.</param>
/// <param name="Attendee">The attendee.</param>
public sealed record AddToGroupEventAttendeeCommand(
    Guid GroupEventId,
    Attendee Attendee) : ICommand<IBaseResponse<Result>>;