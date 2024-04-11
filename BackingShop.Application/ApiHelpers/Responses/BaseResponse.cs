using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;

namespace BackingShop.Application.ApiHelpers.Responses;

/// <summary>
/// Represents the base response class.
/// </summary>
/// <typeparam name="T">The generic result class.</typeparam>
public class BaseResponse<T> : IBaseResponse<T>
    where T : Result
{
    /// <inheritdoc />
    public required string Description { get; set; }

    /// <inheritdoc />
    public Task<T> Data { get; set; }

    /// <inheritdoc />
    public required StatusCode StatusCode { get; set; }
}