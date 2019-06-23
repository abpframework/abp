# Autofac integrace

Autofac je jedním z nejpoužívanějších frameworků pro .Net pro vkládání závislostí (DI). Poskytuje pokročilejší funkce v porovnáním se standardní .Net Core DI knihovnou, jako dynamickou proxy a injekci vlastností.

## Instalace Autofac integrace

> Všechny startovací šablony a vzorky jsou s Autofac již integrovány. Takže většinou nemusíte tento balíček instalovat ručně.

Nainstalujte do vašeho projektu balíček [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) (pro víceprojektovou aplikaci se doporučuje přidat do spustitelného/webového projektu.)

````
Install-Package Volo.Abp.Autofac
````

Poté přídejte k vašemu modulu závislost na `AbpAutofacModule`:

```csharp
using Volo.Abp.Modularity;
using Volo.Abp.Autofac;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

Nakonec nastavte `AbpApplicationCreationOptions` aby nahradil výchozí služby pro vkládání závislostí na Autofac. Záleží na typu aplikace.

### ASP.NET Core aplikace

Volejte `UseAutofac()` v souboru **Startup.cs** jako je ukázáno níže:

````csharp
public class Startup
{
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<MyWebModule>(options =>
        {
            //Integrace Autofac!
            options.UseAutofac();
        });

        return services.BuildServiceProviderFromFactory();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.InitializeApplication();
    }
}
````

### Konzolová aplikace

Volejte metodu `UseAutofac()` v možnostech `AbpApplicationFactory.Create` jako je ukázáno níže:

````csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace AbpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<AppModule>(options =>
            {
                options.UseAutofac(); //Autofac integrace
            }))
            {
                //...
            }
        }
    }
}
````

