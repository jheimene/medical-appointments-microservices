using Microsoft.OpenApi.Models;
using PaymentService.Api.EndPoints;
using PaymentService.Application;
using PaymentService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" }));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPaymentEndPoints();

app.Run();

