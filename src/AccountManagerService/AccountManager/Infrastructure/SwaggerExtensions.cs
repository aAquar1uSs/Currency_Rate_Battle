using Microsoft.OpenApi.Models;

namespace AccountManager.Infrastructure;

public static class SwaggerExtensions
{
    public static void ConfigureSwagger(this IServiceCollection service)
    {
        service.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CBR API", Version = "v1" });
            opt.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
