using Cornerstone.Ordering.Api;
using Cornerstone.Ordering.Api.Endpoints;
using Cornerstone.Ordering.Application;
using Cornerstone.Ordering.Infrastructure;
using Cornerstone.Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOrderingApplication();
builder.Services.AddOrderingInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("OrderingDb")!);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<OrderingDbContext>().Database.MigrateAsync();

    app.MapOpenApi();
    app.MapScalarApiReference(string.Empty);
}

app.UseHttpsRedirection();

app.MapOrderEndpoints();
app.MapHealthChecks("/health");

app.Run();