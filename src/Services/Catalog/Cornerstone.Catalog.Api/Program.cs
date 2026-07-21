using Cornerstone.Catalog.Api.Endpoints;
using Cornerstone.Catalog.Api.Middleware;
using Cornerstone.Catalog.Application;
using Cornerstone.Catalog.Infrastructure;
using Cornerstone.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCatalogApplication();
builder.Services.AddCatalogInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogDb")!);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<CatalogDbContext>().Database.MigrateAsync();

    app.MapOpenApi();
    app.MapScalarApiReference(string.Empty);
}

app.UseHttpsRedirection();

app.MapProductEndpoints();
app.MapHealthChecks("/health");

app.Run();