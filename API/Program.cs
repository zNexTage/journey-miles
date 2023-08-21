using System.Reflection;
using API;
using API.ExternalServices;
using API.Service;
using API.Service.Providers;
using API.Utils;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSwaggerGen(c =>
{
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
  $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddDbContext<AppDbContext>();

// Injeção de dependência.
// builder.Services.AddScoped<DepositionService>();
builder.Services.AddScoped<IDepositionService, DepositionService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<FileManager>();

var gptApiKey = builder.Configuration["OpenAI:GPTApiKey"];
var gptApiOrganizationKey = builder.Configuration["OpenAI:GPTOrganizationKey"];

if (string.IsNullOrEmpty(gptApiKey))
{
    throw new ArgumentNullException(gptApiKey, "Must set GPTApiKey in user-secrets");
}

if (string.IsNullOrEmpty(gptApiOrganizationKey))
{
    throw new ArgumentNullException(gptApiOrganizationKey, "Must set GPTOrganizationKey in user-secrets");
}

builder.Services.AddScoped<ChatBotService>(_ => new(gptApiKey, gptApiOrganizationKey));

// Permite acessar UrlHelper nos services;
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
//Configuração do CORS
// Ref: https://www.infoworld.com/article/3327562/how-to-enable-cors-in-aspnet-core.html
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
