using System.Text;
using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Database.Product.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Domain.Identity.Entities;

namespace BackingShop.Micro.Product.Mediatr.Commands.CreateProduct;

/// <summary>
/// Represents the <see cref="CreateProductCommand"/> handler class.
/// </summary>
/// <param name="productsRepository">The <see cref="Product"/> repository.</param>
/// <param name="logger">The logger.</param>
/// <param name="identifierProvider">The <see cref="User"/> identifier provider.</param>
internal sealed class CreateProductCommandHandler(
    IProductsRepository productsRepository,
    ILogger<CreateProductCommandHandler> logger,
    IUserIdentifierProvider identifierProvider)
    : ICommandHandler<CreateProductCommand, IBaseResponse<Result<Domain.Product.Entities.Product>>>
{
    /// <inheritdoc />
    public async Task<IBaseResponse<Result<Domain.Product.Entities.Product>>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for create the product - {request.Title} {DateTime.UtcNow}");
            
            if (identifierProvider.UserId != request.AuthorId && identifierProvider.UserId == Guid.Empty)
            {
                logger.LogWarning("You don't authorized");
                throw new Exception(DomainErrors.User.InvalidPermissions);
            }

            Maybe<Domain.Product.Entities.Product> maybeProduct = await productsRepository.GetByTitleAsync(request.Title);

            if (maybeProduct.HasValue)
            {
                logger.LogWarning(DomainErrors.Product.HasAlready);
                return new BaseResponse<Result<Domain.Product.Entities.Product>>
                {
                    Description = DomainErrors.Product.HasAlready,
                    StatusCode = StatusCode.BadRequest
                };
            }
            
            Domain.Product.Entities.Product product = request;
            
            Result domainEventResult = await product.CreateDomainEvent();

            if (domainEventResult.IsFailure)
            {
               logger.LogWarning(DomainErrors.Event.EventHasPassed);
               return new BaseResponse<Result<Domain.Product.Entities.Product>>
               {
                   Description = DomainErrors.Event.EventHasPassed,
                   StatusCode = StatusCode.BadRequest
               };
            }
            
            await productsRepository.Insert(product);
            
            logger.LogInformation($"Product created - {product.Title} {product.CreatedOnUtc} {product.Id}");

            return new BaseResponse<Result<Domain.Product.Entities.Product>>
            {
                Data = Result.Create(product, DomainErrors.General.UnProcessableRequest),
                StatusCode = StatusCode.Ok,
                Description = "Product created"
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[CreateProductCommandHandler]: {exception.Message}");
            return new BaseResponse<Result<Domain.Product.Entities.Product>>
            {
                StatusCode = StatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}