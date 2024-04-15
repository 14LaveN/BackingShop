using AutoFixture;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Tests.Common.Abstractions;
using BackingShop.Micro.Identity.Mediatr.Commands.Register;
using FluentAssertions;

namespace BackingShop.Tests.Identity.UnitTests.Commands;

/// <summary>
/// Represents the <see cref="RegisterCommand"/> handler tests class.
/// </summary>
public sealed class RegisterCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    /// <summary>
    /// Register user and returns <see cref="StatusCode"/> OK.
    /// </summary>
    public async Task Register_AssertWithStatusCodeOk()
    {
        RegisterCommand command = Fixture.Create<RegisterCommand>();

        var result = await Sender.Send(command);
        
        result.StatusCode.Should().Be(StatusCode.Ok);
    }
    
    /// <summary>
    /// Register user and returns <see cref="Result"/> Is success.
    /// </summary>
    public async Task Register_AssertWithResultIsSuccess()
    {
        RegisterCommand command = Fixture.Create<RegisterCommand>();

        var result = await Sender.Send(command);

        result.Data.Result.IsSuccess.Should().BeTrue();
    }
}