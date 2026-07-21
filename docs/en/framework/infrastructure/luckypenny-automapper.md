```json
//[doc-seo]
{
    "Description": "Learn how to use the Volo.Abp.LuckyPenny.AutoMapper package to integrate the commercial AutoMapper (LuckyPenny) with ABP Framework."
}
```

# LuckyPenny AutoMapper Integration

## Introduction

[AutoMapper](https://automapper.org/) became a commercial product starting from version 15.x. The free open-source version (14.x) contains a [security vulnerability (GHSA-rvv3-g6hj-g44x)](https://github.com/advisories/GHSA-rvv3-g6hj-g44x) — a DoS (Denial of Service) vulnerability — and no patch will be released for the 14.x series. The patched version is only available in the commercial editions (15.x and later).

The existing [Volo.Abp.AutoMapper](https://www.nuget.org/packages/Volo.Abp.AutoMapper) package uses AutoMapper 14.x and remains available for existing users. If you hold a valid [LuckyPenny AutoMapper commercial license](https://automapper.io/), the `Volo.Abp.LuckyPenny.AutoMapper` package provides the same ABP AutoMapper integration built on the patched commercial version.

> If you don't need to use AutoMapper, you can migrate to [Mapperly](object-to-object-mapping.md#mapperly-integration), which is free and open-source. See the [AutoMapper to Mapperly migration guide](../../release-info/migration-guides/AutoMapper-To-Mapperly.md).

## Installation

Install the `Volo.Abp.LuckyPenny.AutoMapper` NuGet package to your project:

````bash
dotnet add package Volo.Abp.LuckyPenny.AutoMapper
````

Then add `AbpLuckyPennyAutoMapperModule` to your module's `[DependsOn]` attribute, replacing the existing `AbpAutoMapperModule`:

````csharp
[DependsOn(typeof(AbpLuckyPennyAutoMapperModule))]
public class MyModule : AbpModule
{
    // ...
}
````

> **Note:** `Volo.Abp.LuckyPenny.AutoMapper` and `Volo.Abp.AutoMapper` should **not** be used together in the same application. They are mutually exclusive — choose one or the other.

## Usage

`Volo.Abp.LuckyPenny.AutoMapper` is a drop-in replacement for `Volo.Abp.AutoMapper`. All the same APIs, options, and extension methods are available. Refer to the [AutoMapper Integration](object-to-object-mapping.md#automapper-integration) section of the Object to Object Mapping documentation for full usage details.

The only difference from a user perspective is the module class name:

| | Package | Module class |
|---|---|---|
| Free (14.x, has security vulnerability) | `Volo.Abp.AutoMapper` | `AbpAutoMapperModule` |
| Commercial (patched) | `Volo.Abp.LuckyPenny.AutoMapper` | `AbpLuckyPennyAutoMapperModule` |

## License Configuration

The commercial AutoMapper uses an honor-system license. Without a configured key, everything works normally but a warning is written to the logs under the `LuckyPennySoftware.AutoMapper.License` category. To configure your license key, use `AbpAutoMapperOptions.Configurators`:

````csharp
[DependsOn(typeof(AbpLuckyPennyAutoMapperModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.Configurators.Add(ctx =>
            {
                ctx.MapperConfiguration.LicenseKey = "YOUR_LICENSE_KEY";
            });
        });
    }
}
````

It is recommended to read the key from configuration rather than hardcoding it:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var licenseKey = context.Configuration["AutoMapper:LicenseKey"];

    Configure<AbpAutoMapperOptions>(options =>
    {
        options.Configurators.Add(ctx =>
        {
            ctx.MapperConfiguration.LicenseKey = licenseKey;
        });
    });
}
````

````json
{
  "AutoMapper": {
    "LicenseKey": "YOUR_LICENSE_KEY"
  }
}
````

To suppress the license warning in non-production environments (e.g. unit tests or local development), filter the log category in `Program.cs`:

````csharp
builder.Logging.AddFilter("LuckyPennySoftware.AutoMapper.License", LogLevel.None);
````

Or in `appsettings.Development.json`:

````json
{
  "Logging": {
    "LogLevel": {
      "LuckyPennySoftware.AutoMapper.License": "None"
    }
  }
}
````

> **Client-side applications** (Blazor WebAssembly, MAUI, WPF, etc.) should **not** set the license key to avoid exposing it on the client. Use the log filter above to silence the warning instead.

## Obtaining a License

AutoMapper offers a **free Community License** and several paid plans.

### Community License (Free)

A free license is available to organizations that meet **all** of the following criteria:

- Annual gross revenue under **$5,000,000 USD**
- Never received more than **$10,000,000 USD** in outside capital (private equity or venture capital)
- Registered non-profits with an annual budget under **$5,000,000 USD** also qualify

> Government and quasi-government agencies do **not** qualify for the Community License.

Register for the Community License at: [https://luckypennysoftware.com/community](https://luckypennysoftware.com/community)

### Paid Plans

For organizations that do not meet the Community License criteria, paid plans are available at [https://luckypennysoftware.com/purchase](https://luckypennysoftware.com/purchase). For questions, contact [sales@luckypennysoftware.com](mailto:sales@luckypennysoftware.com).

## Migration from Volo.Abp.AutoMapper

To migrate an existing project from `Volo.Abp.AutoMapper` to `Volo.Abp.LuckyPenny.AutoMapper`:

1. Replace the NuGet package reference in all `*.csproj` files:
   ````diff
   -<PackageReference Include="Volo.Abp.AutoMapper" />
   +<PackageReference Include="Volo.Abp.LuckyPenny.AutoMapper" />
   ````

2. Replace the module dependency in all `*.cs` files:
   ````diff
   -[DependsOn(typeof(AbpAutoMapperModule))]
   +[DependsOn(typeof(AbpLuckyPennyAutoMapperModule))]
   ````

3. No other code changes are required. All types (`AbpAutoMapperOptions`, `IMapperAccessor`, `AutoMapperExpressionExtensions`, etc.) remain in the same namespaces.
