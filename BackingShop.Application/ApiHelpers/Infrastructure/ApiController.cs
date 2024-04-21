using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackingShop.Application.ApiHelpers.Contracts;
using BackingShop.Application.ApiHelpers.Policy;
using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Application.Core.Helpers.JWT;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Primitives;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Identity.Entities;

namespace BackingShop.Application.ApiHelpers.Infrastructure;

/// <summary>
/// Represents the api controller class.
/// </summary>
[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Create the new instance of <see cref="ApiController"/>.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="identifierProvider">The user identifier provider.</param>
    protected ApiController(
        ISender sender,
        IUserRepository userRepository,
        IUserIdentifierProvider identifierProvider)
    {
        Sender = sender;
        UserRepository = userRepository;
        UserId = identifierProvider.UserId;
    }

    protected ISender Sender { get; }

    protected Maybe<Guid> UserId { get; }

    protected IUserRepository UserRepository { get; }

    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
    /// response based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));
    
    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
    /// response based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    protected IActionResult Unauthorized(Error error) => Unauthorized(new ApiErrorResponse(new[] { error }));

    /// <summary>
    /// Creates an <see cref="OkObjectResult"/> that produces a <see cref="StatusCodes.Status200OK"/>.
    /// </summary>
    /// <returns>The created <see cref="OkObjectResult"/> for the response.</returns>
    /// <returns></returns>
    protected new IActionResult Ok(object value) => base.Ok(value);

    /// <summary>
    /// Creates an <see cref="NotFoundResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/>.
    /// </summary>
    /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
    protected new IActionResult NotFound() => base.NotFound();
}