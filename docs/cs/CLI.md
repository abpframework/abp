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

* `Acme.BookStore` je tady název řešení.
* Běžná konvence je nazvat řešení stylem *VaseSpolecnost.VasProjekt*. Nicméně můžete použít i jiné pojmenování jako *VasProjekt* (jednostupňový jmenný prostor) nebo *VaseSpolecnost.VasProjekt.VasModul* (třístupňový jmenný prostor).

#### Možnosti

* `--template` nebo `-t`: Určuje název šablony. Výchozí šablona je `app`, která generuje webovou aplikaci. Dostupné šablony:
  * `app` (výchozí): [Aplikační šablona](Startup-Templates/Application.md). Dodatečné možnosti:
    * `--ui` nebo `-u`: Určuje UI framework. Výchozí framework je `mvc`. Dostupné frameworky:
      * `mvc`: ASP.NET Core MVC. Pro tuto šablonu jsou dostupné dodatečné možnosti:
        * `--tiered`: Vytvoří stupňovité řešení, kde jsou vrstvy Web a Http API fyzicky odděleny. Pokud není uvedeno, tak vytvoří vrstvené řešení, které je méně složité a vhodné pro většinu scénářů.
      * `angular`: Angular. Pro tuto šablonu jsou dostupné dodatečné možnosti:
        * `--separate-identity-server`: Oddělí identity server aplikaci od API host aplikace. Pokud není uvedeno, bude na straně serveru jediný koncový bod.
      * `none`: Bez UI. Pro tuto šablonu jsou dostupné dodatečné možnosti:
        * `--separate-identity-server`: Oddělí identity server aplikaci od API host aplikace. Pokud není uvedeno, bude na straně serveru jediný koncový bod.
    * `--database-provider` nebo `-d`: Určuje poskytovatele databáze. Výchozí poskytovatel je `ef`. Dostupní poskytovatelé:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
  *  `module`: [Šablona modulu](Startup-Templates/Module.md). Dodatečné možnosti:
      * `--no-ui`: Určuje nezahrnutí uživatelského rozhraní. Umožňuje vytvořit moduly pouze pro služby (a.k.a. mikroslužby - bez uživatelského rozhraní).
* `--output-folder` nebo `-o`: Určuje výstupní složku. Výchozí hodnota je aktuální adresář.
* `--version` nebo `-v`: Určuje verzi ABP & šablony. Může to být [štítek vydání](https://github.com/abpframework/abp/releases) nebo [název větve](https://github.com/abpframework/abp/branches). Pokud není uvedeno, používá nejnovější vydání. Většinou budete chtít použít nejnovější verzi.


### add-package

Přidá ABP balíček do projektu,

* Přidáním souvisejícícho nuget balíčku jako závislost do projektu.
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

Přidá [více-balíčkový aplikační modul](Modules/Index) k řešení tím, že najde všechny balíčky modulu, vyhledá související projekty v řešení a přidá každý balíček do odpovídajícího projektu v řešení.

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
* `-sp` nebo `--startup-project`: Relativní cesta ke složce spouštěcího projektu. Výchozí hodnota je aktuální adresář.
* `--with-source-code`: Místo balíčků NuGet/NPM přidejte zdrojový kód modulu.

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
* `--npm`:  Aktualizuje pouze balíčky NPM.
* `--nuget`: Aktualizuje pouze balíčky NuGet.

### login

Některé funkce CLI vyžadují přihlášení k platformě abp.io. Chcete-li se přihlásit pomocí svého uživatelského jména, napište

```bash
abp login <username>
```

Všimněte si, že nové přihlášení s již aktivní relací ukončí předchozí relaci a vytvoří novou.

### logout

Odhlásí vás odebráním tokenu relace z počítače.

```
abp logout
```

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

