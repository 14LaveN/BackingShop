using AutoFixture;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Tests.Common.Abstractions;
using BackingShop.Micro.Identity.Mediatr.Commands.Login;
using FluentAssertions;

namespace BackingShop.Tests.Identity.UnitTests.Commands;

/// <summary>
/// Represents the <see cref="LoginCommand"/> handler tests class.
/// </summary>
public sealed class LoginCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    /// <summary>
    /// Login user and returns <see cref="StatusCode"/> OK.
    /// </summary>
    public async Task Login_AssertWithStatusCodeOk()
    {
        LoginCommand command = Fixture.Create<LoginCommand>();

        var result = await Sender.Send(command);
        
        result.StatusCode.Should().Be(StatusCode.Ok);
    }
    
    /// <summary>
    /// Login user and returns <see cref="Result"/> Is success.
    /// </summary>
    public async Task Login_AssertWithResultIsSuccess()
    {
        LoginCommand command = Fixture.Create<LoginCommand>();

        var result = await Sender.Send(command);

        result.Data.Result.IsSuccess.Should().BeTrue();
    }
}