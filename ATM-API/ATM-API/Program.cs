using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using BAL.Shared;
using MODEL;
using MODEL.DTOs; // Ensure that AppSettings is in the correct namespace

var builder = WebApplication.CreateBuilder(args);
var appSettings = new AppSettings();
// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
ServiceManager.SetServiceInfo(builder.Services, appSettings);
builder.Services.AddControllers();

// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
