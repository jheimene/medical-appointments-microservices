using Security.ApiGateway.Yarp.Common;
using Security.ApiGateway.Yarp.Config;
using Security.ApiGateway.Yarp.Data;
using Segurity.ApiGateway;

var builder = WebApplication.CreateBuilder(args);
var mode = builder.Configuration.GetValue<GatewayConfigMode>("GatewayAdmin:Mode");

if (mode == GatewayConfigMode.AppSettings)
    builder.Configuration.AddJsonFile(path: "yarp.json", optional: false, reloadOnChange: true);  //.AddEnvironmentVariables();

builder.Services.AddPresentationServices();
builder.Services.AddApiGatewayYarp(builder.Configuration);

//builder.Services
//       .AddReverseProxy()
//       //.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
//       .LoadFromMemory(InMemoryDataSeed.Routes, InMemoryDataSeed.Clusters);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

//app.UseAuthentication();

//app.UseAuthorization();

//app.UseRateLimiter();

//app.MapGet("/ratelimited", () => $"Hello from /ratelimited at {DateTime.UtcNow}")
//    .RequireRateLimiting("ipPolicy");

app.MapReverseProxy();

app.MapGet("/", () => "API Gateway YARP");

//app.MapPost("/reload", (YarpProvider provider) =>
//{
//    provider.Reload();
//    return Results.Ok("Rutas recargadas desde la base de datos");
//});

// Endpoint para recargar config desde el panel de admin (protegido)
//app.MapPost("/admin/reload-config", async (DatabaseProxyConfigProvider provider) =>
//{
//    await provider.ReloadAsync();
//    return Results.Ok(new { message = "Config reloaded" });
//})
//.RequireAuthorization(policy => policy.RequireRole("YarpAdmin")); // si usas JWT con role

app.MapControllers();

app.Run();
