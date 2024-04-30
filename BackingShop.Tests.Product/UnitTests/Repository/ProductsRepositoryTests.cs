using AutoFixture;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Database.Product.Data.Interfaces;
using BackingShop.Database.Product.Data.Repositories;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Product.Enumerations;
using BackingShop.Tests.Common.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BackingShop.Tests.Product.UnitTests.Repository;

/// <summary>
/// Represents the <see cref="IUserRepository"/> tests class.
/// </summary>
public sealed class ProductsRepositoryTests
    : BaseIntegrationTest
{
    private readonly IProductsRepository _userRepository;
    
    public ProductsRepositoryTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _userRepository = new ProductsRepository(DbContext);
    }

    /// <summary>
    /// Insert product and returns OK result.
    /// </summary>
    [Fact]
    public async Task InsertProduct_AssertWithOk()
    {
        Domain.Product.Entities.Product product = Fixture.Create<Domain.Product.Entities.Product>();
        product.UserId = Guid.Empty;

        var result = await _userRepository.Insert(product); 

        result.IsSuccess.Should().BeTrue();
    }
     
    /// <summary>
    /// Update product and returns OK result.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;

        if (product.HasValue)
        {
            product.Value.Title = "dfdsfdsfsdfdsf";
            
            var result = await _userRepository.UpdateProduct(product);

            result.IsSuccess.Should().BeTrue();
        }
    }
    
    /// <summary>
    /// Get product by specified identifier and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetProductById_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;

        if (product.HasValue)
        {
            var result = await _userRepository.GetByIdAsync(product.Value.Id);

            result.HasValue.Should().BeTrue();
            result.Value.Title.Should().NotBeNull();
        }
    }
    
    /// <summary>
    /// Get products by specified <see cref="ProductType"/> and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetProductsByProductType_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;

        if (product.HasValue)
        {
            var result = await _userRepository.GetProductsByProductType(product.Value.ProductType);

            result.Should().NotBeNullOrEmpty();
        }
    }
    
    /// <summary>
    /// Get products by specified <see cref="ProductType"/> with batch size and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetProductsByProductTypeWithBatchSize_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;

        if (product.HasValue)
        {
            var result = await _userRepository.GetProductsByProductType(product.Value.ProductType, 2);

            result.Should().NotBeNullOrEmpty();
        }
    }
    
    /// <summary>
    /// Get products by specified company name and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetProductsByCompanyName_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;

        if (product.HasValue)
        {
            var result = await _userRepository.GetProductsByCompanyName(product.Value.CompanyName);

            result.Should().NotBeNullOrEmpty();
        }
    }
    
    /// <summary>
    /// Get product by specified title and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetProductByTitle_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;

        if (product.HasValue)
        {
            var result = await _userRepository.GetByTitleAsync(product.Value.Title);

            result.HasValue.Should().BeTrue();
            result.Value.Title.Should().NotBeNull();
        }
    }
    
    /// <summary>
    /// Remove product and returns OK result.
    /// </summary>
    [Fact]
    public async Task RemoveProduct_AssertWithOk()
    {
        Maybe<Domain.Product.Entities.Product> product = (await DbContext.Set<Domain.Product.Entities.Product>()
            .FirstOrDefaultAsync())!;


        if (product.HasValue)
        {
            var result = await _userRepository.Remove(product);

            result.IsSuccess.Should().BeTrue();
        }
    }
}