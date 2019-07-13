# Začínáme s ASP.NET Core MVC aplikací

Tento tutoriál vysvětluje jak začít s ABP z ničeho s minimem závislostí. Obvykle chcete začít se **[startovací šablonou](https://abp.io/Templates)**.

## Tvorba nového projektu

1. Vytvořte novou prázdnou AspNet Core Web aplikaci ve Visual Studio:

![](images/create-new-aspnet-core-application.png)

2. Zvolte prázdnou šablonu

![](images/select-empty-web-application.png)

Můžete zvolit i jinou šablonu, ale pro demonstraci je lepší čístý projekt.

## Instalace Volo.Abp.AspNetCore.Mvc balíku

Volo.Abp.AspNetCore.Mvc je AspNet Core MVC integrační balík pro ABP. Takže ho nainstalujeme do projektu:

````
Install-Package Volo.Abp.AspNetCore.Mvc
````

## Tvorba prvního ABP modulu

ABP je modulární framework a proto vyžaduje **spouštěcí (kořenový) modul** což je třída dědící z ``AbpModule``:

````C#
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace BasicAspNetCoreApplication
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class AppModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
````

``AppModule`` je dobrý název pro spouštěcí modul aplikace.

ABP balíky definují modulové třídy a modul může mít závislost na jiný modul. V kódu výše, náš ``AppModule`` má závislost na ``AbpAspNetCoreMvcModule`` (definován v balíku Volo.Abp.AspNetCore.Mvc). Je běžné přidat ``DependsOn`` atribute po instalaci nového ABP NuGet balíku.

Místo třídy Startup, konfigurujeme ASP.NET Core pipeline v této modulové třídě.

## Třída Startup

V dalším kroku upravíme Startup třídu k integraci ABP modulového systému:

````C#
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BasicAspNetCoreApplication
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AppModule>();

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}

````

Změnili jsme metodu ``ConfigureServices`` aby vracela ``IServiceProvider`` místo ``void``. Tato změna nám dovoluje nahradit AspNet Core vkládání závislostí za jiný framework (více v sekci Autofac integrace níže). ``services.AddApplication<AppModule>()`` přidává všechny služby definované ve všech modulech počínaje ``AppModule``.

Volání ``app.InitializeApplication()`` v metodě ``Configure`` inicializuje a spustí aplikaci.

## Ahoj světe!

Aplikace výše zatím nic nedělá. Pojďme proto vytvořit MVC controller, který už něco dělá:

````C#
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BasicAspNetCoreApplication.Controllers
{
    public class HomeController : AbpController
    {
        public IActionResult Index()
        {
            return Content("Hello World!");
        }
    }
}

````

Jakmile spustíte aplikaci, uvidíte na stránce zprávu "Hello World!".

Odvození ``HomeController`` od ``AbpController`` místo standardní třídy ``Controller``. Toto není vyžadováno, ale třída ``AbpController`` má užitečné základní vlastnosti a metody, které usnadňují vývoj.

## Použití Autofac jako frameworku pro vkládání závislostí

Ačkoliv je AspNet Core systém pro vkládání závíslostí (DI) skvělý pro základní požadavky, Autofac poskytuje pokročilé funkce jako injekce vlastností nebo záchyt metod, které jsou v ABP užity k provádění pokročilých funkcí frameworku.

Nahrazení AspNet Core DI systému za Autofac a integrace s ABP je snadná.

1. Nainstalujeme [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) balík

````
Install-Package Volo.Abp.Autofac
````

2. Přidáme ``AbpAutofacModule`` závislost

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))]  // Přidá závislost na AbpAutofacModule
public class AppModule : AbpModule
{
    ...
}
````

3. Změníme řádek ``services.AddApplication<AppModule>();`` v třídě ``Startup`` následovně:

````C#
services.AddApplication<AppModule>(options =>
{
    options.UseAutofac(); // Integrace s Autofac
});
````

4. Upravíme `Program.cs` aby nepoužíval metodu `WebHost.CreateDefaultBuilder()` jelikož ta používá výchozí DI kontejner:

````csharp
public class Program
{
    public static void Main(string[] args)
    {
        /*
            https://github.com/aspnet/AspNetCore/issues/4206#issuecomment-445612167
            CurrentDirectoryHelpers exists in: \framework\src\Volo.Abp.AspNetCore.Mvc\Microsoft\AspNetCore\InProcess\CurrentDirectoryHelpers.cs
            Will remove CurrentDirectoryHelpers.cs when upgrade to ASP.NET Core 3.0.
        */
        CurrentDirectoryHelpers.SetCurrentDirectory();

        BuildWebHostInternal(args).Run();
    }

    public static IWebHost BuildWebHostInternal(string[] args) =>
        new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIIS()
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();
}
````

## Zdrojový kód

Získejte zdrojový kód vzorového projektu vytvořeného v tomto tutoriálů [z tohoto odkazu](https://github.com/abpframework/abp/tree/master/samples/BasicAspNetCoreApplication).

