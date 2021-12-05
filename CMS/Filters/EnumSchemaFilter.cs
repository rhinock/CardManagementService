using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace CMS.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            var description = Enum
                .GetNames(context.Type)
                .Aggregate("", (acc, c) =>
                    acc + $"{Convert.ToInt32(Enum.Parse(context.Type, c))} - {c} ");

            schema.Description = $"{schema.Description} : {description}";
        }
    }
}
