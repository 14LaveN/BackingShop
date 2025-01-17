﻿namespace BackingShop.Events.PersonalEvent.Contracts.PersonalEvents;

/// <summary>
/// Represents the personal event response.
/// </summary>
public sealed class PersonalEventResponse
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category.
    /// </summary>
    public string Category { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time in UTC format.
    /// </summary>
    public DateTime DateTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets the created on date and time in UTC format.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
}