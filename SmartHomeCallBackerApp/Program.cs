using Database;
using Microsoft.OpenApi.Models;
using Handlers;
using Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddDbContext<Context>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Home Call Backer", Version = "v1" }); });
builder.Services.AddLogging( b=> b.AddConsole());
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<Context>();
builder.Services.AddHostedService<RepeatingService>();
builder.Services.AddHealthChecks();

var app = builder.Build();
CallbackHandlers.Setup(app.MapGroup("/callbacks"));
app.UseSwagger();
app.UseSwaggerUI(c =>
{

    c.SwaggerEndpoint("v1/swagger.json", "Smart Home Call Backer v1");
    
});


StaticLoggerFactory.Initialize(app.Services.GetRequiredService<ILoggerFactory>());
TheConfiguration.Setup(Environment.GetEnvironmentVariables());


app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapHealthChecks("/health");
app.Run();

