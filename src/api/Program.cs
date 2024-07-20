using System.ComponentModel;
using System.Reflection;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static void ConfigureServices(IServiceCollection services)
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddSingleton<IPizzaOrderService, PizzaOrderService>();
    // This allows us to discover different versions, and create a different openapi spec per version
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    // This allows us to override the schema name (getting rid of the version)
    services.AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(x => x.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? x.Name);
    });

    services.AddApiVersioning(options =>
    {
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
        options.ReportApiVersions = true;
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        // Just Major versions for now, proceeded by a lowercase 'v'
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });
}

public partial class Program { }
