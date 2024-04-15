using AutoFixture;
using BackingShop.Database.Common;
using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BackingShop.Tests.Common.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{ 
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        Faker = new Faker();
        Fixture = new Fixture();
    }

    protected readonly ISender Sender;

    protected readonly BaseDbContext DbContext;

    protected readonly Fixture Fixture;

    protected readonly Faker Faker;

    public void Dispose()
    {
        DbContext.Dispose();
        _scope.Dispose();
    }
}