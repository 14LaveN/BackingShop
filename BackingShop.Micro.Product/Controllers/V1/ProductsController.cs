using BackingShop.Application.ApiHelpers.Contracts;
using BackingShop.Application.ApiHelpers.Infrastructure;
using BackingShop.Application.ApiHelpers.Policy;
using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Micro.Product.Contracts.Product;
using BackingShop.Micro.Product.Mediatr.Commands.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackingShop.Micro.Product.Controllers.V1;

/// <summary>
/// Represents the <see cref="Product"/> controller class.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="userRepository">The user repository.</param>
/// <param name="identifierProvider">The user identifier provider.</param>
[Route("api/v1/products")]
public sealed class ProductsController(
    ISender sender,
    IUserRepository userRepository,
    IUserIdentifierProvider identifierProvider)
    : ApiController(sender, userRepository, identifierProvider)
{
    #region Commands.

    /// <summary>
    /// Create product.
    /// </summary>
    /// <param name="request">The <see cref="CreateProductRequest"/> class.</param>
    /// <returns>Return result after creating product.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Product.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(createProductRequest => new CreateProductCommand(
                createProductRequest.Title,
                createProductRequest.Description,
                createProductRequest.Tag,
                createProductRequest.CompanyName,
                createProductRequest.ProductType,
                createProductRequest.Price,
                UserId))
            .Bind(command => Task.FromResult(BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data))
            .Match(Ok, Unauthorized);

    #endregion
}