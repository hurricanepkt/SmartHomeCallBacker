using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Infrastructure;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.Enum,
    AllowMultiple = false)]
public class SwaggerSchemaExampleAttribute : Attribute
{
    public SwaggerSchemaExampleAttribute(string example)
    {
        Example = example;
    }

    public string Example { get; set; }
}


[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.Enum,
    AllowMultiple = false)]
public class SwaggerSchemaDeprecatedAttribute : Attribute
{
    public SwaggerSchemaDeprecatedAttribute(bool isDeprecated)
    {
        IsDeprecated = isDeprecated;
    }

    public SwaggerSchemaDeprecatedAttribute() {
        IsDeprecated = true;
    }

    public bool IsDeprecated { get; set; }
}
