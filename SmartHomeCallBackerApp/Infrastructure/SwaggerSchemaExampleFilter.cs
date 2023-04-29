using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Infrastructure;

public class SwaggerSchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if(context.MemberInfo != null)
        {
            var schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaExampleAttribute>()
                .FirstOrDefault();
            if (schemaAttribute != null)
                ApplySchemaAttribute(schema, schemaAttribute);
        }
    }

    private void ApplySchemaAttribute(OpenApiSchema schema, SwaggerSchemaExampleAttribute schemaAttribute)
    {
        if (schemaAttribute.Example != null)
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString(schemaAttribute.Example);
        }
    }
}



public class SwaggerSchemaDeprecatedFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if(context.MemberInfo != null)
        {
            var schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaDeprecatedAttribute>()
                .FirstOrDefault();
            if (schemaAttribute != null)
                ApplySchemaAttribute(schema, schemaAttribute);
        }
    }

    private void ApplySchemaAttribute(OpenApiSchema schema, SwaggerSchemaDeprecatedAttribute schemaAttribute)
    {
        if (schemaAttribute.IsDeprecated)
        {
            schema.Deprecated = schemaAttribute.IsDeprecated;
        }
    }
}