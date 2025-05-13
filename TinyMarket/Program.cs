using Microsoft.OpenApi.Models;
using TinyMarketData.Extensions;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();

builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TinyMarket API",
        Version = "v1",
        Description = "API para gestión de productos de MiniMarket"
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TinyMarket API v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
