using AutoFixture;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Micro.Identity.Mediatr.Commands.Register;
using BackingShop.Micro.Product.Mediatr.Commands.CreateProduct;
using BackingShop.Tests.Common.Abstractions;
using FluentAssertions;

namespace BackingShop.Tests.Product.UnitTests.Commands;

/// <summary>
/// Represents the <see cref="CreateProductCommand"/> handler class tests.
/// </summary>
/// <param name="factory"></param>
public sealed class CreateProductCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    /// <summary>
    /// Create product and returns <see cref="StatusCode"/> OK.
    /// </summary>
    [Fact]
    public async Task CreateProduct_AssertWithStatusCodeOk()
    {
        CreateProductCommand command = Fixture.Create<CreateProductCommand>();

        var result = await Sender.Send(command);
        
        result.StatusCode.Should().Be(StatusCode.Ok);
    }
    
    /// <summary>
    /// Create product and returns <see cref="Result"/> Is success.
    /// </summary>
    [Fact]
    public async Task CreateProduct_AssertWithResultIsSuccess()
    {
        CreateProductCommand command = Fixture.Create<CreateProductCommand>();

        var result = await Sender.Send(command);

        result.Data.Result.IsSuccess.Should().BeTrue();
    }
}