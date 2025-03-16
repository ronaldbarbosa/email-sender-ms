using EmailSenderService.DTOs.Request;
using EmailSenderService.Services;
using EmailSenderService.Settings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EmailService>();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}
else
{
    builder.Configuration.AddEnvironmentVariables();
}
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.MapScalarApiReference(options =>
{
    options.Title = "Email Sender Service";
});
app.UseHttpsRedirection();

app.MapPost("/send-email", async (EmailService emailService, EmailRequestDto emailRequestDto) =>
    {
        var result = await emailService.SendEmail(emailRequestDto);
        return result ? Results.Ok() : Results.BadRequest();
    })
    .WithName("SendEmail")
    .WithOpenApi();
app.Run();