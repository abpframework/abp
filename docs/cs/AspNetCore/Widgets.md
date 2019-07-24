# Widgety

ABP poskytuje model a infastrukturu k vytváření **znovu použitelných widgetů**. Systém widgetů je rozšíření pro [ASP.NET Core pohledové komponenty](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components). Widgety jsou zvláště užitečné, když chcete;

* Definovat widgety v znovu použitelných **[modulech](../Module-Development-Basics.md)**.
* Mít závislosti na **skriptech & stylech** ve vašem widgetu.
* Vytvářet **[řídící panely](Dashboards.md)** za použítí widgetů.
* Spolupráci widgetů s **[authorizačními](../Authorization.md)** a **[svazovacími](Bundling-Minification.md)** systémy.

## Základní definice widgetu

### Tvorba pohledové komponenty

Jako první krok, vytvoříme běžnou ASP.NET Core pohledovou komponentu:

![widget-basic-files](../images/widget-basic-files.png)

**MySimpleWidgetViewComponent.cs**:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

Dědění z `AbpViewComponent` není vyžadováno. Můžeme dědit ze standardního ASP.NET Core `ViewComponent`. `AbpViewComponent` pouze definuje pár základních a užitečných vlastnosti.

**Default.cshtml**:

```xml
<div class="my-simple-widget">
    <h2>My Simple Widget</h2>
    <p>This is a simple widget!</p>
</div>
```

### Definice widgetu

Přidáme atribut `Widget` k třídě `MySimpleWidgetViewComponent` pro označení této pohledové komponenty jako widgetu:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

## Vykreslení widgetu

Vykreslení widgetu je vcelku standardní. Použijeme metodu `Component.InvokeAsync` v razor pohledu/stránce jako s kteroukoliv jinou pohledovou komponentou. Příklady:

````xml
@await Component.InvokeAsync("MySimpleWidget")
@await Component.InvokeAsync(typeof(MySimpleWidgetViewComponent))
````

První přístup používá název widgetu, zatímco druhý používá typ pohledové komponenty.

## Název widgetu

Výchozí název pohledových komponent je vypočítán na základě názvu typu pohledové komponenty. Pokud je typ pohledové komponenty `MySimpleWidgetViewComponent` potom název widgetu bude `MySimpleWidget` (odstraní se `ViewComponent` postfix). Takto ASP.NET Core vypočítává název pohledové komponenty.

Chceme-li přizpůsobit název widgetu, stačí použít standardní atribut `ViewComponent` z ASP.NET Core:

```csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget]
    [ViewComponent(Name = "MyCustomNamedWidget")]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/Components/MySimpleWidget/Default.cshtml");
        }
    }
}
```

ABP bude respektovat přizpůsobený název při zpracování widgetu.

> Pokud jsou názvy pohledové komponenty a složky, která pohledovou komponentu obsahuje rozdílné, pravděpodobně budete muset ručně uvést cestu pohledu tak jako je to provedeno v tomto příkladu.

### Zobrazovaný název

Můžeme také definovat čitelný & lokalizovatelný zobrazovaný název pro widget. Tento zobrazovaný název může být využít na uživatelském rozhraní kdykoliv je to potřeba. Zobrazovaný název je nepovinný a lze ho definovat pomocí vlastností atributu `Widget`:

````csharp
using DashboardDemo.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(
        DisplayName = "MySimpleWidgetDisplayName", // Lokalizační klíč
        DisplayNameResource = typeof(DashboardDemoResource) // Lokalizační zdroj
        )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

Podívejte se na [dokument lokalizace](../Localization.md) pro více informací o lokalizačních zdrojích a klíčích.

## Závislosti na stylech & skriptech

Problémy když má widget soubory skriptů a stylů;

* Každý stránka, která používá widget musí také přidat soubory **skriptů & stylů** tohoto widgetu.
* Stránka se také musí postarat o **závislé knihovny/soubory** widgetu.

ABP tyto problémy řeší, když správně propojíme zdroje s widgetem. O závislosti widgetu se při jeho používání nestaráme.

### Definování jednoduchých cest souborů

Níže uvedený příklad widgetu přidá stylové a skriptové soubory:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(
        StyleFiles = new[] { "/Pages/Components/MySimpleWidget/Default.css" },
        ScriptFiles = new[] { "/Pages/Components/MySimpleWidget/Default.js" }
        )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

ABP bere v úvahu tyto závislosti a správně je přidává do pohledu/stránky při použití widgetu. Stylové/skriptové soubory mohou být **fyzické nebo virtuální**. Plně integrováno do [virtuálního systému souborů](../Virtual-File-System.md).

### Definování přispěvatelů balíku

Všechny zdroje použité ve widgetech na stránce jsou přidány jako **svazek** (svázány & minifikovány v produkci pokud nenastavíte jinak). Kromě přidání jednoduchého souboru můžete využít plnou funkčnost přispěvatelů balíčků.

Níže uvedený ukázkový kód provádí totéž co výše uvedený kód, ale definuje a používá přispěvatele balíků:

````csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(
        StyleTypes = new []{ typeof(MySimpleWidgetStyleBundleContributor) },
        ScriptTypes = new[]{ typeof(MySimpleWidgetScriptBundleContributor) }
        )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }

    public class MySimpleWidgetStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files
              .AddIfNotContains("/Pages/Components/MySimpleWidget/Default.css");
        }
    }

    public class MySimpleWidgetScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files
              .AddIfNotContains("/Pages/Components/MySimpleWidget/Default.js");
        }
    }
}

````

Systém přispěvatelů balíků je velmi schopný. Pokud váš widget používá k vykreslení grafu JavaScript knihovnu, můžete ji deklarovat jako závislost, díky tomu se knihovna pokud nebyla dříve přidána automaticky přidá na stránku Tímto způsobem se stránka využívající váš widget nestará o závislosti.

Podívejte se na dokumentaci [svazování & minifikace](Bundling-Minification.md) pro více informací o tomto systému.

## Autorizace

Některé widgety budou pravděpodobně muset být dostupné pouze pro ověřené nebo autorizované uživatele. V tomto případě použijte následující vlastnosti atributu `Widget`:

* `RequiresAuthentication` (`bool`): Nastavte na true, aby byl tento widget použitelný pouze pro ověřené uživatele (uživatel je přihlášen do aplikace).
* `RequiredPolicies` (`List<string>`): Seznam názvů zásad k autorizaci uživatele. Další informace o zásadách naleznete v [dokumentu autorizace](../Authorization.md).

Příklad:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(RequiredPolicies = new[] { "MyPolicyName" })]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

## Možnosti widgetu

Jako alternativu k atributu `Widget` můžete ke konfiguraci widgetů použít `WidgetOptions`:

```csharp
Configure<WidgetOptions>(options =>
{
    options.Widgets.Add<MySimpleWidgetViewComponent>();
});
```

Toto vepište do metody `ConfigureServices` vašeho [modulu](../Module-Development-Basics.md). Veškerá konfigurace udělaná přes atribut `Widget` je dostupná i za pomoci `WidgetOptions`. Příklad konfigurace, která přidává styl pro widget:

````csharp
Configure<WidgetOptions>(options =>
{
    options.Widgets
        .Add<MySimpleWidgetViewComponent>()
        .WithStyles("/Pages/Components/MySimpleWidget/Default.css");
});
````

> Tip: `WidgetOptions` lze také použít k získání existujícího widgetu a ke změně jeho konfigurace. To je obzvláště užitečné, pokud chcete změnit konfiguraci widgetu uvnitř modulu používaného vaší aplikací. Použíjte `options.Widgets.Find` k získání existujícího `WidgetDefinition`.