using Database;
using Microsoft.OpenApi.Models;
using Handlers;
using Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
TheConfiguration.Setup(Environment.GetEnvironmentVariables());
builder.Services.AddDbContext<Context>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{ 
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Home Call Backer", Version = "v1" }); 
    c.SchemaFilter<SwaggerSchemaExampleFilter>();
    c.SchemaFilter<SwaggerSchemaDeprecatedFilter>();
    // var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyApi.xml");
    // c.IncludeXmlComments(filePath);
});
builder.Services.AddLogging( b=> b.AddConsole());
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddHostedService<RepeatingService>();
builder.Services.AddHealthChecks();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<Context>();
    await ctx.Database.EnsureCreatedAsync();
}


CallbackHandlers.Setup(app.MapGroup("/callbacks"));
app.UseSwagger();

app.UseSwaggerUI(c =>
{

    c.SwaggerEndpoint("v1/swagger.json", "Smart Home Call Backer v1");

});



app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapHealthChecks("/health");
app.Run();

