# ABP CLI

ABP CLI (Command Line Interface) je nástroj v příkazovém řádku k provádění některých běžných úkonů v řešeních založených na ABP.

## Instalace

ABP CLI je [dotnet global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools). Nainstalujete jej pomocí okna příkazového řádku:

````bash
dotnet tool install -g Volo.Abp.Cli
````

Aktualizace stávající instalace:

````bash
dotnet tool update -g Volo.Abp.Cli
````

## Příkazy

### new

Vygeneruje nové řešení založené na ABP [startovací šabloně](Startup-Templates/Index.md).

Základní použití:

````bash
abp new <název-řešení> [možnosti]
````

Příklad:

````bash
abp new Acme.BookStore
````

* Acme.BookStore je tady název řešení.
* Běžná konvence je nazvat řešení stylem *VašeSpolečnost.VášProjekt*. Nicméně můžete použít i jiné pojmenování jako *VášProjekt* (jednostupňový jmenný prostor) nebo *VašeSpolečnost.VášProdukt.VášModul* (třístupňový jmenný prostor).

#### Možnosti

* `--template` nebo `-t`: Určuje název šablony. Výchozí šablona je `mvc`. Dostupné šablony:
  * `mvc` (výchozí): ASP.NET Core [MVC aplikační šablona](Startup-Templates/Mvc.md). Dodatečné možnosti:
    * `--database-provider` nebo `-d`: Určuje poskytovatele databáze. Vychozí poskytovatel je `ef`. Dostupní poskytovatelé:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--tiered`: Vytvoří stupňovité řešení, kde jsou vrstvy Web a Http API fyzicky odděleny. Pokud není uvedeno tak vytvoří vrstvené řešení, které je méně složité a vhodné pro většinu scénářů.
  *  `mvc-module`: ASP.NET Core [MVC modulová šablona](Startup-Templates/Mvc-Module.md). Dodatečné možnosti:
    * `--no-ui`: Určuje, že nebude zahrnuto uživatelské rozhraní. To umožňuje vytvářet moduly pouze pro služby (a.k.a. mikroslužby - bez UI).
* `--output-folder` nebo `-o`: Určuje výstupní složku. Výchozí hodnota je aktuální adresář.

### add-package

Přidá nový balíček ABP do projektu pomocí,

* Přidání souvisejícícho nuget balíčku jako závislost do projektu.
* Přidáním `[DependsOn(...)]` atributu k modulové tříde v projektu (podívejte se na [dokument vývoje modulu](Module-Development-Basics.md)).

> Všimněte si, že přidaný modul může vyžadovat další konfiguraci, která je obecně uvedena v dokumentaci příslušného balíčku.

Základní použití:

````bash
abp add-package <název-balíčku> [možnosti]
````

Příklad:

````
abp add-package Volo.Abp.MongoDB
````

* Tento příklad přidá do projektu balíček Volo.Abp.MongoDB.

#### Možnosti

* `--project` nebo `-p`: Určuje cestu k projektu (.csproj). Pokud není zadáno, CLI se pokusí najít soubor .csproj v aktuálním adresáři.

### add-module

Přidá více-balíčkový modul k řešení tím, že najde všechny balíčky modulu, vyhledá související projekty v řešení a přidá každý balíček do odpovídajícího projektu v řešení.

> Modul se obecně skládá z několika balíčků (z důvodu vrstvení, různých možností poskytovatele databáze nebo jiných důvodů). Použití příkazu `add-module` dramaticky zjednodušuje přidání modulu do řešení. Každý modul však může vyžadovat další konfiguraci, která je obecně uvedena v dokumentaci příslušného modulu.

Základní použití:

````bash
abp add-module <název-modulu> [možnosti]
````

Příklad:

```bash
abp add-module Volo.Blogging
```

* Tento příklad přidá do projektu modul Volo.Blogging.

#### Možnosti

* `--solution` nebo `-s`: Určuje cestu k řešení (.sln). Pokud není zadáno, CLI se pokusí najít soubor .sln v aktuálním adresáři.
* `--skip-db-migrations`: Pro poskytovatele databáze EF Core automaticky přidá nový kód první migrace (`Add-Migration`) a v případě potřeby aktualizuje databázi (`Update-Database`). Tuto možnost určete k vynechání této operace.

### update

Aktualizace všech balíčků souvisejících s ABP může být únavná, protože existuje mnoho balíčků frameworku a modulů. Tento příkaz automaticky aktualizuje na poslední verze všechny související ABP NuGet a NPM balíčky v řešení nebo projektu.

Použití:

````bash
abp update [možnosti]
````

* Pokud spouštíte v adresáři se souborem .sln, aktualizuje všechny balíčky všech projektů v řešení souvisejících s ABP na nejnovější verze.
* Pokud spouštíte v adresáři se souborem .csproj, aktualizuje všechny balíčky v projektu na nejnovější verze.

#### Možnosti

* `--include-previews` nebo `-p`: Zahrne náhledové, beta a rc balíčky při kontrole nových verzí.

### help

Vypíše základní informace k používání CLI.

Použítí:

````bash
abp help [název-příkazu]
````

Příklady:

````bash
abp help        # Zobrazí obecnou nápovědu.
abp help new    # Zobrazí nápovědu k příkazu "new".
````

