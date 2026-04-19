using ProductService.Api;
using ProductService.Application;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Configuration;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

await VaultConfigurationLoader.LoadVaultSecretsInfoConfigurationAsync(builder, CancellationToken.None);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // scalar/v1
}
else
{
    app.UseHsts();
}

// ✅ Esto hace que NO salga el mega detalle del DeveloperExceptionPage
app.UseExceptionHandler(/*new ExceptionHandlerOptions { SuppressDiagnosticsCallback = _ => false }*/);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
