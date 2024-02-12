using Asp.Versioning.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string V0_8Client = "V0.8Client";
const string V0_9Client = "V0.9Client";
const string V1Client = "V1Client";

#pragma warning disable S1075 // URIs should not be hardcoded
const string BaseUrl = "http://localhost:5128/api";
#pragma warning restore S1075 // URIs should not be hardcoded

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// https://github.com/dotnet/aspnet-api-versioning/wiki/Versioned-Clients
builder.Services.AddSingleton<IApiVersionWriter>(new UrlSegmentApiVersionWriter("{version}"));
builder.Services
    .AddHttpClient(V1Client, (provider, client) => client.BaseAddress = new($"{BaseUrl}/v{{version}}/"))
    .AddApiVersion(1.0);

builder.Services
    .AddHttpClient(V0_9Client, (provider, client) => client.BaseAddress = new($"{BaseUrl}/v{{version}}/"))
    .AddApiVersion(0.9);

builder.Services
    .AddHttpClient(V0_8Client, (provider, client) => client.BaseAddress = new($"{BaseUrl}/v{{version}}/"))
    .AddApiVersion(0.8);

using IHost host = builder.Build();

// Execute the program:
IHttpClientFactory factory = host.Services.GetRequiredService<IHttpClientFactory>();
HttpClient v0_8Client = factory.CreateClient(V0_8Client);
HttpResponseMessage v0_8Response = await v0_8Client.GetAsync("products");
v0_8Response.EnsureSuccessStatusCode();

HttpClient v0_9Client = factory.CreateClient(V0_9Client);
HttpResponseMessage v0_9esponse = await v0_9Client.GetAsync("products");
v0_9esponse.EnsureSuccessStatusCode();

HttpClient v1Client = factory.CreateClient(V1Client);
HttpResponseMessage v1esponse = await v1Client.GetAsync("products");
v1esponse.EnsureSuccessStatusCode();

await host.RunAsync();
