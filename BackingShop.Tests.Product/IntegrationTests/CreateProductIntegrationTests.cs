using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Micro.Product.Contracts.Product;
using BackingShop.Micro.Product.Mediatr.Commands.CreateProduct;
using BackingShop.Tests.Common.Abstractions;
using FluentAssertions;

namespace BackingShop.Tests.Product.IntegrationTests;

/// <summary>
/// Represents the create <see cref="Product"/> <see cref="BaseFunctionalTest"/> tests.
/// </summary>
public sealed class CreateProductIntegrationTests(IntegrationTestWebAppFactory factory)
    : BaseFunctionalTest(factory)
{
    /// <summary>
    /// Create product and returns <see cref="StatusCode"/> OK.
    /// </summary>
    [Fact]
    public async Task CreateProduct_AssertWithStatusCodeOk()
    {
        CreateProductRequest request = Fixture.Create<CreateProductRequest>();
        
        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJiN2Y0MjIwYS0xZGZlLTQ4OTQtYTljMC0xOTY5OTU4NzNmNTIiLCJuYW1lIjoiU2RzZ2ZkdHJpbmciLCJlbWFpbCI6InN0cmluZ0BtYWlsLnJ1IiwiZ2l2ZW5fbmFtZSI6IiIsImZhbWlseV9uYW1lIjoic3RyZGZnaW5nIiwiZXhwIjoxNzEzNzYxMjg1fQ.4Wbk28-HMnZlBc8vjfshuvjaqgirQSCzXQiXVOwY-us");

        var result = 
            await HttpClient.PostAsJsonAsync("/api/v1/products/create-product", request);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}