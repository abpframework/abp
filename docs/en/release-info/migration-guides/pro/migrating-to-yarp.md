# Migrating API Gateway from Ocelot to YARP

This guide provides guidance for migrating your existing microservice application's API Gateway from [Ocelot](https://github.com/ThreeMammals/Ocelot) to [YARP](https://github.com/microsoft/reverse-proxy). Since YARP is available with ABP v8.0+, you will need to update your existing application in order to apply YARP changes.

## History

Until this version, ABP was using the [Ocelot](https://github.com/ThreeMammals/Ocelot) for the API Gateway, in the [Microservice Startup Template](https://docs.abp.io/en/commercial/latest/startup-templates/microservice/index). Since the **Ocelot** library is not actively maintained, we have searched for an alternative and decided to switch from Ocelot to [YARP](https://github.com/microsoft/reverse-proxy) for the API Gateway. YARP is maintained by Microsoft and is actively being developed and seems a better alternative than Ocelot and provides the same feature stack and even more.

## YARP Migration Steps

You should update various files in different projects to upgrade your the API Gateway from Ocelot to YARP. All of the required changes are listed below in different sections, please apply the following steps to upgrade from Ocelot to YARP.

> Alternatively, you can create a Microservice Startup Template and compare your changes with the microservice template. It's recommended approach to ensure all the required changes have been done.

### Shared Hosting Gateways Project

* Remove the Ocelot packages and add `YARP` packages as follows:

```diff
    <ItemGroup>
-        <PackageReference Include="Ocelot" Version="17.0.0" />
-        <PackageReference Include="Ocelot.Provider.Polly" Version="17.0.0" />
+        <PackageReference Include="Yarp.ReverseProxy" Version="2.0.0" />
    </ItemGroup>
```

* Open the `*SharedHostingGatewaysModule.cs` and make the following changes:

```diff
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var env = context.Services.GetHostingEnvironment();

-      var ocelotBuilder = context.Services.AddOcelot(configuration)
-           .AddPolly();

-       if (!env.IsProduction())
-       {
-           ocelotBuilder.AddDelegatingHandler<AbpRemoveCsrfCookieHandler>(true);
-       }

+       context.Services.AddReverseProxy()
+           .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }
```

* Update the `GatewayHostBuilderExtensions.cs` file as follows:

```csharp
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting;

public static class AbpHostingHostBuilderExtensions
{
    public const string AppYarpJsonPath = "yarp.json";

    public static IHostBuilder AddYarpJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true,
        string path = AppYarpJsonPath)
    {
        return hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            builder.AddJsonFile(
                path: AppYarpJsonPath,
                optional: optional,
                reloadOnChange: reloadOnChange
            );
        });
    }
}
```

* Delete the `OcelotConfiguration.cs` file from the solution.

### Public Web Gateway Project

* Remove the **ocelot.json** file and instead create a new **yarp.json** file and update its content as follows (in the root directory of the `PublicWebGateway` project):

```json
{
  "ReverseProxy": {
    "Routes": {
      "AbpApi": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/abp/{**catch-all}"
        }
      },
      "AdministrationSwagger": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/swagger-json/Administration/swagger/v1/swagger.json"
        },
        "Transforms": [
          {
            "PathRemovePrefix": "/swagger-json/Administration"
          }
        ]
      },
      "Account": {
        "ClusterId": "AuthServer",
        "Match": {
          "Path": "/api/account/{**catch-all}"
        }
      },
      "AuthServerSwagger": {
        "ClusterId": "AuthServer",
        "Match": {
          "Path": "/swagger-json/AuthServer/swagger/v1/swagger.json"
        },
        "Transforms": [
          {
            "PathRemovePrefix": "/swagger-json/AuthServer"
          }
        ]
      },
      "Product": {
        "ClusterId": "Product",
        "Match": {
          "Path": "/api/product-service/{**catch-all}"
        }
      },
      "ProductSwagger": {
        "ClusterId": "Product",
        "Match": {
          "Path": "/swagger-json/Product/swagger/v1/swagger.json"
        },
        "Transforms": [
          {
            "PathRemovePrefix": "/swagger-json/Product"
          }
        ]
      }
    },
    "Clusters": {
      "AuthServer": {
        "Destinations": {
          "AuthServer": {
            "Address": "https://localhost:44322/"
          }
        }
      },
      "Administration": {
        "Destinations": {
          "Administration": {
            "Address": "https://localhost:44367/"
          }
        }
      },
      "Product": {
        "Destinations": {
          "Product": {
            "Address": "https://localhost:44361/"
          }
        }
      }
    }
  }
}
```

* Open the `Program.cs` file and make the following changes:

```diff
    builder.Host
        .AddAppSettingsSecretsJson()
-       .AddOcelotJson()
+       .AddYarpJson() 
        .UseAutofac()
        .UseSerilog();
```

* Open the module class of the `PublicWebGateway` project and update the `OnApplicationInitialization` method as follows:

```diff
+   using Yarp.ReverseProxy.Configuration;
    
    //other configurations...
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.GetConfiguration();
+       var proxyConfig = app.ApplicationServices.GetRequiredService<IProxyConfigProvider>().GetConfig();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
+       app.UseCors();
+       app.UseRouting();
+       app.UseAuthorization();
        app.UseSwagger();
+       app.UseAbpSwaggerUI(options => { ConfigureSwaggerUI(proxyConfig, options, configuration); });
        app.UseAbpSerilogEnrichers();
+       app.UseRewriter(CreateSwaggerRewriteOptions());
        app.UseEndpoints(endpoints => endpoints.MapReverseProxy());
    }

+    private static void ConfigureSwaggerUI(
+        IProxyConfig proxyConfig,
+        SwaggerUIOptions options,
+        IConfiguration configuration)
+    {
+        foreach (var cluster in proxyConfig.Clusters)
+        {
+            options.SwaggerEndpoint($"/swagger-json/{cluster.ClusterId}/swagger/v1/swagger.json", $"{cluster.ClusterId} API");
+        }

+        options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
+        options.OAuthScopes(
+            "AdministrationService",
+            "AccountService",
+            "ProductService"
+        );
+    }

+    private static RewriteOptions CreateSwaggerRewriteOptions()
+    {
+        var rewriteOptions = new RewriteOptions();
+        rewriteOptions.AddRedirect("^(|\\|\\s+)$", "/swagger"); // Regex for "/" and "" (whitespace)
+        return rewriteOptions;
+    }
```

### Web Gateway Project

* Remove the **ocelot.json** file and instead create a new **yarp.json** file and update its content as follows (in the root directory of the `WebGateway` project):

```json
{
  "ReverseProxy": {
    "Routes": {
      "AbpApi": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/abp/{**catch-all}"
        }
      },
      "SettingManagement": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/setting-management/{**catch-all}"
        }
      },
      "FeatureManagement": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/feature-management/{**catch-all}"
        }
      },
      "PermissionManagement": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/permission-management/{**catch-all}"
        }
      },
      "AuditLogging": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/audit-logging/{**catch-all}"
        }
      },
      "LanguageManagement": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/language-management/{**catch-all}"
        }
      },
      "TextTemplateManagement": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/text-template-management/{**catch-all}"
        }
      },
      "LeptonThemeManagement": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/lepton-theme-management/{**catch-all}"
        }
      },
      "GDPR": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/api/gdpr/{**catch-all}"
        }
      },
      "AdministrationSwagger": {
        "ClusterId": "Administration",
        "Match": {
          "Path": "/swagger-json/Administration/swagger/v1/swagger.json"
        },
        "Transforms": [
          { "PathRemovePrefix": "/swagger-json/Administration" }
        ]
      },
      "Account": {
        "ClusterId": "AuthServer",
        "Match": {
          "Path": "/api/account/{**catch-all}"
        }
      },
      "AccountAdmin": {
        "ClusterId": "AuthServer",
        "Match": {
          "Path": "/api/account-admin/{**catch-all}"
        }
      },
      "AuthServerSwagger": {
        "ClusterId": "AuthServer",
        "Match": {
          "Path": "/swagger-json/AuthServer/swagger/v1/swagger.json"
        },
        "Transforms": [
          { "PathRemovePrefix": "/swagger-json/AuthServer" }
        ]
      },
      "Identity": {
        "ClusterId": "Identity",
        "Match": {
          "Path": "/api/identity/{**catch-all}"
        }
      },
      "OpenIddict": {
        "ClusterId": "Identity",
        "Match": {
          "Path": "/api/openiddict/{**catch-all}"
        }
      },
      "IdentitySwagger": {
        "ClusterId": "Identity",
        "Match": {
          "Path": "/swagger-json/Identity/swagger/v1/swagger.json"
        },
        "Transforms": [
          { "PathRemovePrefix": "/swagger-json/Identity" }
        ]
      },
      "Saas": {
        "ClusterId": "Saas",
        "Match": {
          "Path": "/api/saas/{**catch-all}"
        }
      },
      "Payment": {
        "ClusterId": "Saas",
        "Match": {
          "Path": "/api/payment/{**catch-all}"
        }
      },
      "PaymentAdmin": {
        "ClusterId": "Saas",
        "Match": {
          "Path": "/api/payment-admin/{**catch-all}"
        }
      },
      "SaasSwagger": {
        "ClusterId": "Saas",
        "Match": {
          "Path": "/swagger-json/Saas/swagger/v1/swagger.json"
        },
        "Transforms": [
          { "PathRemovePrefix": "/swagger-json/Saas" }
        ]
      },
      "Product": {
        "ClusterId": "Product",
        "Match": {
          "Path": "/api/product-service/{**catch-all}"
        }
      },
      "ProductSwagger": {
        "ClusterId": "Product",
        "Match": {
          "Path": "/swagger-json/Product/swagger/v1/swagger.json"
        },
        "Transforms": [
          { "PathRemovePrefix": "/swagger-json/Product" }
        ]
      }
    },
    "Clusters": {
      "AuthServer": {
        "Destinations": {
          "AuthServer": {
            "Address": "https://localhost:44322/"
          }
        }
      },
      "Administration": {
        "Destinations": {
          "Administration": {
            "Address": "https://localhost:44367/"
          }
        }
      },
      "Identity": {
        "Destinations": {
          "Identity": {
            "Address": "https://localhost:44388/"
          }
        }
      },
      "Saas": {
        "Destinations": {
          "Saas": {
            "Address": "https://localhost:44381/"
          }
        }
      },
      "Product": {
        "Destinations": {
          "Product": {
            "Address": "https://localhost:44361/"
          }
        }
      }
    }
  }
}
```

* Open the `Program.cs` file and make the change in the below:

```diff
    builder.Host
        .AddAppSettingsSecretsJson()
-       .AddOcelotJson()
+       .AddYarpJson() 
        .UseAutofac()
        .UseSerilog();
```

* Open the module class of the `WebGateway` project and update the `OnApplicationInitialization` method as follows:

```diff
+   using Yarp.ReverseProxy.Configuration;
    
    //other configurations...
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.GetConfiguration();
+       var proxyConfig = app.ApplicationServices.GetRequiredService<IProxyConfigProvider>().GetConfig();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
+       app.UseCors();
+       app.UseRouting();
+       app.UseAuthorization();
        app.UseSwagger();
+       app.UseAbpSwaggerUI(options => { ConfigureSwaggerUI(proxyConfig, options, configuration); });
        app.UseAbpSerilogEnrichers();
+       app.UseRewriter(CreateSwaggerRewriteOptions());
        app.UseEndpoints(endpoints => endpoints.MapReverseProxy());
    }

+    private static void ConfigureSwaggerUI(
+        IProxyConfig proxyConfig,
+        SwaggerUIOptions options,
+        IConfiguration configuration)
+    {
+        foreach (var cluster in proxyConfig.Clusters)
+        {
+            options.SwaggerEndpoint($"/swagger-json/{cluster.ClusterId}/swagger/v1/swagger.json", $"{cluster.ClusterId} API");
+        }

+        options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
+        options.OAuthScopes(
+            "AdministrationService",
+            "AccountService",
+            "IdentityService",
+            "SaasService",
+            "ProductService"
+        );
+    }

+    private static RewriteOptions CreateSwaggerRewriteOptions()
+    {
+        var rewriteOptions = new RewriteOptions();
+        rewriteOptions.AddRedirect("^(|\\|\\s+)$", "/swagger"); // Regex for "/" and "" (whitespace)
+        return rewriteOptions;
+    }
```

### Chart Updates

* Update the `gateway-web-public-configmap.yaml` as follows (_etc/k8s/<project-name>/charts/gateway-web-public/templates/gateway-web-public-configmap.yaml_):

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: {%{{{ .Release.Name }}}%}-{%{{{ .Chart.Name }}}%}-configmap
data:
  yarp.json: |-
    {
      "ReverseProxy": {
        "Routes": {
          "AbpApi": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/abp/{**catch-all}"
            }
          },
          "AdministrationSwagger": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/swagger-json/Administration/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/Administration" }
            ]
          },
          "Account": {
            "ClusterId": "AuthServer",
            "Match": {
              "Path": "/api/account/{**catch-all}"
            }
          },
          "AuthServerSwagger": {
            "ClusterId": "AuthServer",
            "Match": {
              "Path": "/swagger-json/AuthServer/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/AuthServer" }
            ]
          },
          "Product": {
            "ClusterId": "Product",
            "Match": {
              "Path": "/api/product-service/{**catch-all}"
            }
          },
          "ProductSwagger": {
            "ClusterId": "Product",
            "Match": {
              "Path": "/swagger-json/Product/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/Product" }
            ]
          }
        },
        "Clusters": {
          "AuthServer": {
            "Destinations": {
              "AuthServer": {
                "Address": "{%{{{ .Values.reRoutes.accountService.url }}}%}"
              }
            }
          },
          "Administration": {
            "Destinations": {
              "Administration": {
                "Address": "{%{{{ .Values.reRoutes.administrationService.url }}}%}"
              }
            }
          },
          "Product": {
            "Destinations": {
              "Product": {
                "Address": "{%{{{ .Values.reRoutes.productService.url }}}%}"
              }
            }
          }
        }
      }
    }
```

* Make the following changes in the `gateway-web-public-deployment.yaml` file (_etc/k8s/<project-name>/charts/gateway-web-public/templates/gateway-web-public-deployment.yaml_):

```diff
-          mountPath: /app/ocelot.json
+          mountPath: /app/yarp.json
-          subPath: ocelot.json
+          subPath: yarp.json
```

* Update the `values.yaml` file as follows (_etc/k8s/<project-name>/charts/gateway-web-public/values.yaml_):

```yaml
config:
  selfUrl: # https://gateway-public.myprojectname.dev
  corsOrigins: # https://myprojectname-st-gateway-web,https://myprojectname-st-gateway-public-web
  authServer:
    authority: # http://myprojectname-st-authserver
    requireHttpsMetadata: # "false"
    metadataAddress: # https://authserver.myprojectname.dev/.well-known/openid-configuration
    swaggerClientId: # WebGateway_Swagger
  dotnetEnv: Staging 
  redisHost: #
  rabbitmqHost: #
  elasticsearchUrl: #
  AbpLicenseCode: #

reRoutes:
  accountService:
    url: http://myprojectname-st-authserver
  saasService:
    url: http://saas-st-administration
  administrationService:
    url: http://myprojectname-st-administration
  productService:
    url: http://myprojectname-st-product

ingress:
  host: gateway-public.myprojectname.dev
  tlsSecret: myprojectname-tls

image:
  repository: mycompanyname/myprojectname-gateway-web-public
  tag: latest
  pullPolicy: IfNotPresent

env: {}
```

* Update the `gateway-web-configmap.yaml` as follows (_etc/k8s/<project-name>/charts/gateway-web/templates/gateway-web-configmap.yaml_):

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: {%{{{ .Release.Name }}}%}-{%{{{ .Chart.Name }}}%}-configmap
data:
  yarp.json: |-
    {
      "ReverseProxy": {
        "Routes": {
          "AbpApi": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/abp/{**catch-all}"
            }
          },
          "SettingManagement": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/setting-management/{**catch-all}"
            }
          },
          "FeatureManagement": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/feature-management/{**catch-all}"
            }
          },
          "PermissionManagement": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/permission-management/{**catch-all}"
            }
          },
          "AuditLogging": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/audit-logging/{**catch-all}"
            }
          },
          "LanguageManagement": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/language-management/{**catch-all}"
            }
          },
          "TextTemplateManagement": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/text-template-management/{**catch-all}"
            }
          },
          "LeptonThemeManagement": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/lepton-theme-management/{**catch-all}"
            }
          },
          "GDPR": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/api/gdpr/{**catch-all}"
            }
          },
          "AdministrationSwagger": {
            "ClusterId": "Administration",
            "Match": {
              "Path": "/swagger-json/Administration/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/Administration" }
            ]
          },
          "Account": {
            "ClusterId": "AuthServer",
            "Match": {
              "Path": "/api/account/{**catch-all}"
            }
          },
          "AccountAdmin": {
            "ClusterId": "AuthServer",
            "Match": {
              "Path": "/api/account-admin/{**catch-all}"
            }
          },
          "AuthServerSwagger": {
            "ClusterId": "AuthServer",
            "Match": {
              "Path": "/swagger-json/AuthServer/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/AuthServer" }
            ]
          },
          "Identity": {
            "ClusterId": "Identity",
            "Match": {
              "Path": "/api/identity/{**catch-all}"
            }
          },
          "OpenIddict": {
            "ClusterId": "Identity",
            "Match": {
              "Path": "/api/openiddict/{**catch-all}"
            }
          },
          "IdentitySwagger": {
            "ClusterId": "Identity",
            "Match": {
              "Path": "/swagger-json/Identity/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/Identity" }
            ]
          },
          "Saas": {
            "ClusterId": "Saas",
            "Match": {
              "Path": "/api/saas/{**catch-all}"
            }
          },
          "Payment": {
            "ClusterId": "Saas",
            "Match": {
              "Path": "/api/payment/{**catch-all}"
            }
          },
          "PaymentAdmin": {
            "ClusterId": "Saas",
            "Match": {
              "Path": "/api/payment-admin/{**catch-all}"
            }
          },
          "SaasSwagger": {
            "ClusterId": "Saas",
            "Match": {
              "Path": "/swagger-json/Saas/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/Saas" }
            ]
          },
          "Product": {
            "ClusterId": "Product",
            "Match": {
              "Path": "/api/product-service/{**catch-all}"
            }
          },
          "ProductSwagger": {
            "ClusterId": "Product",
            "Match": {
              "Path": "/swagger-json/Product/swagger/v1/swagger.json"
            },
            "Transforms": [
              { "PathRemovePrefix": "/swagger-json/Product" }
            ]
          }
        },
        "Clusters": {
          "AuthServer": {
            "Destinations": {
              "AuthServer": {
                "Address": "{%{{{ .Values.reRoutes.accountService.url }}}%}"
              }
            }
          },
          "Administration": {
            "Destinations": {
              "Administration": {
                "Address": "{%{{{ .Values.reRoutes.administrationService.url }}}%}"
              }
            }
          },
          "Identity": {
            "Destinations": {
              "Identity": {
                "Address": "{%{{{ .Values.reRoutes.identityService.url }}}%}"
              }
            }
          },
          "Saas": {
            "Destinations": {
              "Saas": {
                "Address": "{%{{{ .Values.reRoutes.saasService.url }}}%}"
              }
            }
          },
          "Product": {
            "Destinations": {
              "Product": {
                "Address": "{%{{{ .Values.reRoutes.productService.url }}}%}"
              }
            }
          }
        }
      }
    }
```

* Make the following changes in the `gateway-web-deployment.yaml` file (_etc/k8s/<project-name>/charts/gateway-web/templates/gateway-web-deployment.yaml_):

```diff
-          mountPath: /app/ocelot.json
+          mountPath: /app/yarp.json
-          subPath: ocelot.json
+          subPath: yarp.json
```

* Update the `values.yaml` file as follows (_etc/k8s/<project-name>/charts/gateway-web/values.yaml_):

```yaml
config:
  selfUrl: # https://gateway-web.myprojectname.dev
  corsOrigins: # https://myprojectname-st-angular
  globalConfigurationBaseUrl: # http://myprojectname-st-gateway-web
  authServer:
    authority: # http://myprojectname-st-authserver
    requireHttpsMetadata: # "false"
    metadataAddress: # https://authserver.myprojectname.dev/.well-known/openid-configuration
    swaggerClientId: # WebGateway_Swagger
  dotnetEnv: # 
  redisHost: #
  rabbitmqHost: #
  elasticsearchUrl: #
  AbpLicenseCode: #
  
reRoutes:
  accountService:
    url: http://myprojectname-st-authserver
  saasService:
    url: http://saas-st-administration
  administrationService:
    url: http://myprojectname-st-administration
  identityService:
    url: http://myprojectname-st-identity
  productService:
    url: http://myprojectname-st-product
ingress:
  host: # gateway-web.myprojectname.dev
  tlsSecret: myprojectname-tls

image:
  repository: mycompanyname/myprojectname-gateway-web
  tag: latest
  pullPolicy: IfNotPresent

env: {}
```

* Update the `values.yaml` file as follows (_etc/k8s/<project-name>/values.yaml_):

```diff
- globalConfigurationBaseUrl: http://myprojectname-st-gateway-web

reRoutes:
    accountService:
+      url: http://myprojectname-st-authserver
-      dns: https://authserver.myprojectname.dev
-      schema: http
-      host: myprojectname-st-authserver
-      port: 80
    identityService:
+      url: http://myprojectname-st-identity
-      dns: https://identity.myprojectname.dev
-      schema: http
-      host: myprojectname-st-identity
-      port: 80
    administrationService:
+      url: http://myprojectname-st-administration
-      dns: https://administration.myprojectname.dev
-      schema: http
-      host: myprojectname-st-administration
-      port: 80
    saasService:
+      url: http://myprojectname-st-saas
-      dns: https://saas.myprojectname.dev
-      schema: http
-      host: myprojectname-st-saas
-      port: 80
    productService:
+      url: http://myprojectname-st-product
-      dns: https://product.myprojectname.dev
-      schema: http
-      host: myprojectname-st-product
-      port: 80  
```

## See Also

* [ABP Version 8.0 Migration Guide](v8-0.md)
