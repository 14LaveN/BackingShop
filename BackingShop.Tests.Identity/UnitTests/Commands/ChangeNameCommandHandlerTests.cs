using AutoFixture;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Tests.Common.Abstractions;
using BackingShop.Micro.Identity.Mediatr.Commands.ChangeName;
using FluentAssertions;

namespace BackingShop.Tests.Identity.UnitTests.Commands;

/// <summary>
/// Represents the <see cref="ChangeNameCommand"/> handler tests class.
/// </summary>
public sealed class ChangeNameCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    /// <summary>
    /// ChangeName user and returns <see cref="StatusCode"/> OK.
    /// </summary>
    public async Task ChangeName_AssertWithStatusCodeOk()
    {
        ChangeNameCommand command = Fixture.Create<ChangeNameCommand>();

        var result = await Sender.Send(command);
        
        result.StatusCode.Should().Be(StatusCode.Ok);
    }
    
    /// <summary>
    /// ChangeName user and returns <see cref="Result"/> Is success.
    /// </summary>
    public async Task ChangeName_AssertWithResultIsSuccess()
    {
        ChangeNameCommand command = Fixture.Create<ChangeNameCommand>();

        var result = await Sender.Send(command);

        result.Data.IsSuccess.Should().BeTrue();
    }
}