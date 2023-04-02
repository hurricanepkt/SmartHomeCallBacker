using Microsoft.EntityFrameworkCore;
using Database;
using Microsoft.OpenApi.Models;
using Handlers;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FileContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Home Call Backer", Version = "v1" }); });

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<FileContext>();
builder.Services.AddHostedService<RepeatingService>();
var app = builder.Build();
CallbackHandlers.Setup(app.MapGroup("/callbacks"));
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Smart Home Call Backer v1");
});

 

app.Run();

