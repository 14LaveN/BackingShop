using BackingShop.Database.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace BackingShop.Tests.Common.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.0")
        .Build();
    
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("BSGenericDb")
        .WithUsername("postgres")
        .WithCommand("1111")
        .Build();
    
    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptorType =
                typeof(DbContextOptions<BaseDbContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BaseDbContext>(o => 
                o.UseNpgsql(_databaseContainer.GetConnectionString(), act 
                        =>
                    {
                        act.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        act.EnableRetryOnFailure(3);
                        act.CommandTimeout(30);
                    })
                    .LogTo(Console.WriteLine)
                    .EnableServiceProviderCaching()
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            services.RemoveAll(typeof(RedisCacheOptions));

            services.AddStackExchangeRedisCache(options =>
                options.Configuration = _redisContainer.GetConnectionString());
        });
    }

    public new async Task DisposeAsync()
    {
        await _databaseContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}