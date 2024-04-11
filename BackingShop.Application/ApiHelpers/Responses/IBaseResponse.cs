using BackingShop.Domain.Common.Enumerations;

namespace BackingShop.Application.ApiHelpers.Responses;

/// <summary>
/// Represents the base response interface.
/// </summary>
/// <typeparam name="T">The generic type.</typeparam>
public interface IBaseResponse<T>
{
    /// <summary>
    /// Gets or sets status code.
    /// </summary>
    public StatusCode StatusCode { get; set; }

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets data.
    /// </summary>
    public Task<T> Data { get; set; }
}