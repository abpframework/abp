# Přepnutí na EF Core PostgreSQL providera

Tento dokument vysvětluje, jak přepnout na poskytovatele databáze **PostgreSQL** pro **[spouštěcí šablonu aplikace](Startup-Templates/Application.md)**, která je dodávána s předem nakonfigurovaným SQL poskytovatelem.

## Výměna balíku Volo.Abp.EntityFrameworkCore.SqlServer

Projekt `.EntityFrameworkCore` v řešení závisí na NuGet balíku [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer). Odstraňte tento balík a přidejte stejnou verzi balíku [Volo.Abp.EntityFrameworkCore.PostgreSql](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.PostgreSql).

## Nahrazení závislosti modulu

Najděte třídu ***YourProjectName*EntityFrameworkCoreModule** v projektu `.EntityFrameworkCore`, odstraňte `typeof(AbpEntityFrameworkCoreSqlServerModule)` z atributu `DependsOn`, přidejte `typeof(AbpEntityFrameworkCorePostgreSqlModule)` (také nahraďte `using Volo.Abp.EntityFrameworkCore.SqlServer;` za `using Volo.Abp.EntityFrameworkCore.PostgreSql;`).

## UseNpgsql()

Najděte volání `UseSqlServer()` v *YourProjectName*EntityFrameworkCoreModule.cs uvnitř projektu `.EntityFrameworkCore` a nahraďte za `UseNpgsql()`.

Najděte volání `UseSqlServer()` v *YourProjectName*MigrationsDbContextFactory.cs uvnitř projektu `.EntityFrameworkCore.DbMigrations` a nahraďte za `UseNpgsql()`.

> V závislosti na struktuře řešení můžete najít více volání `UseSqlServer()`, které je třeba změnit.

## Změna connection stringů

PostgreSql connection stringy se od těch pro SQL Server liší. Je proto potřeba zkontrolovat všechny soubory `appsettings.json` v řešení a connection stringy v nich nahradit. Podívejte se na [connectionstrings.com](https://www.connectionstrings.com/postgresql/) pro více detailů o možnostech PostgreSql connection stringů.

Typicky je potřeba změnit `appsettings.json` v projektech `.DbMigrator` a `.Web` projects, ale to záleží na vaší struktuře řešení.

## Regenerace migrací

Startovací šablona používá [Entity Framework Core Code First migrace](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/). EF Core migrace závisí na zvoleném DBMS poskytovateli. Tudíž změna DBMS poskytovatele způsobí selhání migrace.
* Smažte složku Migrations v projektu `.EntityFrameworkCore.DbMigrations` and znovu sestavte řešení.
* Spusťte `Add-Migration "Initial"` v Package Manager Console (je nutné zvolit `.DbMigrator`  (nebo `.Web`) projekt jako startovací projekt v Solution Explorer a zvolit projekt `.EntityFrameworkCore.DbMigrations` jako výchozí v Package Manager Console).

Tímto vytvoříte migraci databáze se všemi nakonfigurovanými databázovými objekty (tabulkami).

Spusťte projekt `.DbMigrator` k vytvoření databáze a vložení počátečních dat.

## Spuštění aplikace

Vše je připraveno. Stačí už jen spustit aplikaci a užívat si kódování.
