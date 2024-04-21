
#region BuilderRegion

using System.Reflection;
using App.Metrics.Formatters.Prometheus;
using BackingShop.Application;
using BackingShop.Application.ApiHelpers.Configurations;
using BackingShop.Application.ApiHelpers.Middlewares;
using BackingShop.Application.Core.Settings;
using BackingShop.BackgroundTasks;
using BackingShop.Database.Common;
using BackingShop.Email;
using BackingShop.Micro.Product.Common.DependencyInjection;
using BackingShop.RabbitMq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Prometheus;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.HttpRequestDurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection(MongoSettings.MongoSettingsKey));

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.SmallestSize;
});

builder.Host.UseMetricsWebTracking(options => 
        options.OAuth2TrackingEnabled = true)
    .UseMetricsEndpoints(options =>
    {
        options.EnvironmentInfoEndpointEnabled = true;
        options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        options.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
    });

builder.Services.AddControllers();

builder.Services.AddValidators();

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddBackgroundTasks(builder.Configuration);

builder.Services.AddRabbitBackgroundTasks(builder.Configuration);

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddMediatr();

builder.Services.AddHelpers();

builder.Services.AddSwachbackleService(Assembly.GetExecutingAssembly(), "Product");

builder.Services.AddCaching();

builder.Services.AddApplication();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthorizationExtension(builder.Configuration);

#endregion

#region ApplicationRegion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerApp();
    app.ApplyMigrations();
}

app.UseCors();

UseMetrics();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.MapControllers();

UseCustomMiddlewares();

app.Run();
return;

#endregion

#region UseMiddlewaresRegion

void UseCustomMiddlewares()
{
    if (app is null)
        throw new ArgumentException();

    app.UseMiddleware<RequestLoggingMiddleware>(app.Logger);
    app.UseMiddleware<ResponseCachingMiddleware>();
}

void UseMetrics()
{
    if (app is null)
        throw new ArgumentException();
    
    app.UseMetricServer();
    app.UseHttpMetrics();
    app.UsePrometheusServer();
    app.UsePrometheusRequestDurations();
}

#endregion