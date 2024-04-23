using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Product.Entities;
using BackingShop.Domain.Product.Enumerations;

namespace BackingShop.Micro.Product.Mediatr.Commands.CreateProduct;

/// <summary>
/// Represents the create product commands record.
/// </summary>
/// <param name="Title">The title.</param>
/// <param name="Description">The description.</param>
/// <param name="Tag">The tag.</param>
/// <param name="CompanyName">The company name.</param>
/// <param name="ProductType">The product type.</param>
/// <param name="Price">The price.</param>
/// <param name="AuthorId">The author identifier.</param>
public sealed record CreateProductCommand(
    string Title,
    string Description,
    string Tag,
    string CompanyName,
    ProductType ProductType,
    decimal Price,
    Guid AuthorId)
    : ICommand<IBaseResponse<Result<Domain.Product.Entities.Product>>>
{
    /// <summary>
    /// Create the new product from <see cref="CreateProductCommand"/> class.
    /// </summary>
    /// <param name="request">The create product request.</param>
    /// <returns>Returns the new product.</returns>
    public static implicit operator Domain.Product.Entities.Product(CreateProductCommand request)
    {
        return new Domain.Product.Entities.Product
        {
            Title =request.Title,
            Description = request.Description,
            Price = request.Price,
            Tag = request.Tag,
            CompanyName = request.CompanyName,
            ProductType = request.ProductType,
            UserId = request.AuthorId,
            CreatedAt = DateTime.UtcNow
        };
    }
}