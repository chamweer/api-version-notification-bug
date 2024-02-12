using Asp.Versioning;

namespace Api.Versioning.Notification.Bug.Api.Attributes;

/// <summary>
/// Source: https://github.com/dotnet/aspnet-api-versioning/wiki/Custom-Attributes
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class V1Attribute : ApiVersionAttribute
{
    public V1Attribute() : base(new ApiVersion(1.0))
    {
        // No implementation
    }
}