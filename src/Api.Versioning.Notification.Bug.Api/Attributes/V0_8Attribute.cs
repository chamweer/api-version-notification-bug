using Asp.Versioning;

namespace Api.Versioning.Notification.Bug.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
#pragma warning disable S101 // Types should be named in PascalCase
public sealed class V0_8Attribute : ApiVersionAttribute
#pragma warning restore S101 // Types should be named in PascalCase
{
    public V0_8Attribute() : base(new ApiVersion(0.8))
    {
        Deprecated = true;
    }
}