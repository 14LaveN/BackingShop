using BackingShop.Database.Common;
using BackingShop.Database.Product.Data.Interfaces;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Product.DTO;
using BackingShop.Domain.Product.Enumerations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BackingShop.Database.Product.Data.Repositories;

/// <summary>
/// Represents the <see cref="Product"/> repository class.
/// </summary>
internal sealed class ProductsRepository
    (BaseDbContext dbContext)
    : GenericRepository<Domain.Product.Entities.Product>(dbContext), IProductsRepository
{
    /// <inheritdoc />
    public async Task<Maybe<Domain.Product.Entities.Product>> GetByTitleAsync(Name title) =>
        await dbContext.Set<Domain.Product.Entities.Product>().FirstOrDefaultAsync(p => p.Title == title)
        ?? throw new AggregateException(nameof(title));

    /// <inheritdoc />
    public new async Task Remove(Domain.Product.Entities.Product product) =>
        await DbContext.Set<Domain.Product.Entities.Product>()
            .Where(p=> p.Id == product.Id)
            .ExecuteDeleteAsync();

    /// <inheritdoc />
    public async Task<Result<Domain.Product.Entities.Product>> UpdateProduct(Domain.Product.Entities.Product product)
    {
        const string sql = """
                                           UPDATE dbo.products
                                           SET ModifiedOnUtc = @ModifiedOnUtc,
                                               Description = @Description,
                                               Title = @Title,
                                               Price = @Price,
                                               Tag = @Tag
                                           WHERE Id = @Id AND Deleted = 0
                           """;
        
        SqlParameter[] parameters =
        {
            new("@ModifiedOnUtc", DateTime.UtcNow),
            new("@Id", product.Id),
            new("@Title", product.Title),
            new("@Price", product.Price),
            new("@Tag", product.Tag),
            new("@Description", product.Description)
        };
        
        int result = await DbContext.ExecuteSqlAsync(sql, parameters);
        
        return result is not 0 ? product : throw new ArgumentException();
    }

    /// <inheritdoc />
    public async Task<IOrderedQueryable<ProductDto>> GetProductsByProductType(ProductType productType) =>
        await GetProductsByProductTypeDelegate(dbContext, productType);

    private static readonly Func<BaseDbContext, ProductType, Task<IOrderedQueryable<ProductDto>>>
        GetProductsByProductTypeDelegate =
            EF.CompileAsyncQuery(
                (BaseDbContext context, ProductType productType) =>
                    (IOrderedQueryable<ProductDto>)context.Set<Domain.Product.Entities.Product>()
                        .AsSingleQuery()
                        .AsNoTrackingWithIdentityResolution()
                        .Where(p => p.ProductType == productType)
                        .OrderByDescending(p => p.CreatedOnUtc));

    /// <inheritdoc />
    public async Task<IQueryable<ProductDto>> GetProductsByCompanyName(string companyName) =>
        await GetProductsByCompanyNameDelegate(dbContext, companyName);
    
    private static readonly Func<BaseDbContext, string, Task<IOrderedQueryable<ProductDto>>>
        GetProductsByCompanyNameDelegate =
            EF.CompileAsyncQuery(
                (BaseDbContext context, string companyName) =>
                    (IOrderedQueryable<ProductDto>)context.Set<Domain.Product.Entities.Product>()
                        .AsSingleQuery()
                        .AsNoTrackingWithIdentityResolution()
                        .Where(p => p.CompanyName == companyName)
                        .OrderByDescending(p => p.CreatedOnUtc));
}