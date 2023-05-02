using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IT120P.Filters
{
    public class StringPropertySchemaFilter : ISchemaFilter
    {
        private const string DefaultValue = "";

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            foreach (var property in schema.Properties.Where(p => p.Value.Type == "string"))
            {
                property.Value.Default = new OpenApiString(DefaultValue);
            }
        }
    }
}
