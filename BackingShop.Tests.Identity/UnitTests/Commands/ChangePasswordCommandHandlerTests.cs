using AutoFixture;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Tests.Common.Abstractions;
using BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;
using FluentAssertions;

namespace BackingShop.Tests.Identity.UnitTests.Commands;

/// <summary>
/// Represents the <see cref="ChangePasswordCommand"/> handler tests class.
/// </summary>
public sealed class ChangePasswordCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    /// <summary>
    /// ChangePassword user and returns <see cref="Result"/> Is success.
    /// </summary>
    public async Task ChangePassword_AssertWithResultIsSuccess()
    {
        ChangePasswordCommand command = Fixture.Create<ChangePasswordCommand>();

        var result = await Sender.Send(command);

        result.IsSuccess.Should().BeTrue();
    }
}