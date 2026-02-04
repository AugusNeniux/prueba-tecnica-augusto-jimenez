using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.PracticalTest.API.Configurations;

public static class SwaggerExtension
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // Incluir documentación XML
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, true);

            // Configuración de seguridad para JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese 'Bearer {token}' para autenticarse."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            c.OperationFilter<HeaderParameters>();
        });

        return services;
    }
}

public class HeaderParameters : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
        operation.Parameters ??= [];

        if (hasAnonymous)
        {
            operation.Security.Clear();
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Language preference (optional), default is 'es-MX'.",
            Required = false,
            Examples = new Dictionary<string, OpenApiExample>
            {
                [""] = new OpenApiExample()
                {
                    Value = new OpenApiString(""),
                    Summary = "Default (es-MX)"
                },
                ["es-MX"] = new OpenApiExample()
                {
                    Value = new OpenApiString("es-MX"),
                    Summary = "Spanish (Mexico)"
                },
                ["en-US"] = new OpenApiExample()
                {
                    Value = new OpenApiString("en-US"),
                    Summary = "English (United States)"
                }
            }
        });
    }
}
