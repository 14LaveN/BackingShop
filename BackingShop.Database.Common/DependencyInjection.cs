﻿using BackingShop.Database.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackingShop.Database.Common.Interceptors;
using BackingShop.Domain.Identity.Entities;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BackingShop.Database.Common;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBaseDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var connectionString = configuration.GetConnectionString("BSGenericDb");

        if (connectionString is not null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString);
        
        services.AddDbContext<BaseDbContext>((sp, o) =>
            o.UseNpgsql(connectionString, act
                    =>
            {
                act.EnableRetryOnFailure(3);
                act.CommandTimeout(30);
                act.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables))
                .LogTo(Console.WriteLine)
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        return services;
    }
}