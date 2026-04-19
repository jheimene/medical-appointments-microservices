using Email.Api;
using Email.Api.Models;
using Email.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEmailApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/send-email", async (SendEmailRequest request, IEmailSender emailSender, CancellationToken cancellationToken) =>
{
    await emailSender.SendEmailAsync(request.To, request.Subject, request.HtmlBody, request.PlainTextBody, cancellationToken);
    return Results.Ok(new { Message = "Email enviado" });
})
    .WithName("SendEmail")
    .WithTags("Email")
    .Produces(StatusCodes.Status200OK);

app.Run();
