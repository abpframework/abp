using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Content;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpSwaggerGenServiceCollectionExtensions
{
    public static IServiceCollection AddAbpSwaggerGen(
        this IServiceCollection services,
        Action<SwaggerGenOptions> setupAction = null)
    {
        return services.AddSwaggerGen(
            options =>
            {
                Func<OpenApiSchema> remoteStreamContentSchemaFactory = () => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                };

                options.MapType<RemoteStreamContent>(remoteStreamContentSchemaFactory);
                options.MapType<IRemoteStreamContent>(remoteStreamContentSchemaFactory);

                setupAction?.Invoke(options);
            });
    }

    public static IServiceCollection AddAbpSwaggerGenWithOAuth(
        this IServiceCollection services,
        [NotNull] string authority,
        [NotNull] Dictionary<string, string> scopes,
        Action<SwaggerGenOptions> setupAction = null)
    {
        return services
            .AddAbpSwaggerGen()
            .AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{authority.EnsureEndsWith('/')}connect/authorize"),
                                Scopes = scopes,
                                TokenUrl = new Uri($"{authority.EnsureEndsWith('/')}connect/token")
                            }
                        }
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "oauth2"
                                    }
                                },
                                Array.Empty<string>()
                            }
                    });

                    setupAction?.Invoke(options);
                });
    }
}
