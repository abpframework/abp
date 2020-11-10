using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpSwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpSwaggerGenWithOAuth(
            this IServiceCollection services,
            [NotNull] string authority,
            [NotNull] Dictionary<string, string> scopes,
            Action<SwaggerGenOptions> setupAction = null)
        {
            return services.AddSwaggerGen(
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
}
