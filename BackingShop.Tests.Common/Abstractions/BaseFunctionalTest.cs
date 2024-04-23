using AutoFixture;
using BackingShop.Database.Common;
using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BackingShop.Tests.Common.Abstractions;

/// <summary>
/// Represents the base function test class.
/// </summary>
public abstract class BaseFunctionalTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    /// <summary>
    /// Initialize the instance of <see cref="BaseFunctionalTest"/>.
    /// </summary>
    /// <param name="factory">The integration test web app factory.</param>
    protected BaseFunctionalTest(IntegrationTestWebAppFactory factory)
    {
        Fixture = new Fixture();
        HttpClient = factory.CreateClient();
    }

    /// <summary>
    /// Gets HttpClient.
    /// </summary>
    protected readonly HttpClient HttpClient;

    /// <summary>
    /// Gets Fixture.
    /// </summary>
    protected readonly Fixture Fixture;

    /// <inheritdoc/>
    public void Dispose()
    {
        HttpClient.Dispose();
    }
}