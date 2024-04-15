using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Product.DTO;
using BackingShop.Domain.Product.Enumerations;

namespace BackingShop.Database.Product.Data.Interfaces;

/// <summary>
/// Represents the <see cref="Product"/> repository interface.
/// </summary>
public interface IProductsRepository
{
    /// <summary>
    /// Gets the product with the specified identifier.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>The maybe instance that may contain the product with the specified identifier.</returns>
    Task<Maybe<Domain.Product.Entities.Product>> GetByIdAsync(Guid productId);
    
    /// <summary>
    /// Gets the product with the specified title.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <returns>The maybe instance that may contain the product with the specified title.</returns>
    Task<Maybe<Domain.Product.Entities.Product>> GetByTitleAsync(Name title);
    
    /// <summary>
    /// Inserts the specified product to the database.
    /// </summary>
    /// <param name="product">The product to be inserted to the database.</param>
    Task<Result> Insert(Domain.Product.Entities.Product product);

    /// <summary>
    /// Remove the specified product entity to the database.
    /// </summary>
    /// <param name="product">The product to be inserted to the database.</param>
    Task<Result> Remove(Domain.Product.Entities.Product product);
    
    /// <summary>
    /// Update the specified product entity to the database.
    /// </summary>
    /// <param name="product">The product to be inserted to the database.</param>
    /// <returns>The result instance that may contain the product entity with the specified message class.</returns>
    Task<Result<Domain.Product.Entities.Product>> UpdateProduct(Domain.Product.Entities.Product product);

    /// <summary>
    /// Gets the enumerable products by product type with the specified product type.
    /// </summary>
    /// <param name="productType">The product type.</param>
    /// <returns>The maybe instance that may contain the enumerable product DTO with the specified product class.</returns>
    Task<IOrderedQueryable<ProductDto>> GetProductsByProductType(ProductType productType);

    /// <summary>
    /// Gets the enumerable products by product type with the specified product type and batch size.
    /// </summary>
    /// <param name="productType">The product type.</param>
    /// <param name="batchSize">The batch size.</param>
    /// <returns>The maybe instance that may contain the enumerable product DTO with the specified product class.</returns>
    Task<IOrderedQueryable<ProductDto>> GetProductsByProductType(ProductType productType, int batchSize);
    
    /// <summary>
    /// Gets the enumerable products by company name with the specified company name.
    /// </summary>
    /// <param name="companyName">The company name.</param>
    /// <returns>The maybe instance that may contain the enumerable product DTO with the specified product class.</returns>
    Task<IQueryable<ProductDto>> GetProductsByCompanyName(string companyName);
}