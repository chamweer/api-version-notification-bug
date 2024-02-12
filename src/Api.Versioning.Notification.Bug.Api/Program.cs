using Api.Versioning.Notification.Bug.Api.Swagger;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;

        _ = options.Policies
                .Sunset(0.9)
                .Effective(2025, 4, 1)
                .Link("https://docs.api.com/policy.html?api-version=0.9")
                    .Title("V0.9 Sunset Policy")
                    .Type("text/html");
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });


builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    _ = app.UseSwaggerUI(options =>
    {
        foreach (string groupName in app.DescribeApiVersions().Reverse().Select(x => x.GroupName))
        {
            string url = $"/swagger/{groupName}/swagger.json";
            string name = groupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
