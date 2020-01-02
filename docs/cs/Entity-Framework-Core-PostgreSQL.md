## Entity Framework Core PostgreSQL integrace

> Podívejte se na [Entity Framework Core integrační dokument](../Entity-Framework-Core.md) pro základy integrace EF Core.

### Aktualizace projektu EntityFrameworkCore

- V projektu `Acme.BookStore.EntityFrameworkCore` nahraďte balík `Volo.Abp.EntityFrameworkCore.SqlServer` za `Volo.Abp.EntityFrameworkCore.PostgreSql` 
- Aktualizace pro použití PostgreSQL v `BookStoreEntityFrameworkCoreModule`
  - Nahraďte `AbpEntityFrameworkCoreSqlServerModule` za `AbpEntityFrameworkCorePostgreSqlModule`
  - Nahraďte `options.UseSqlServer()` za `options.UsePostgreSql()`
- V jiných projektech aktualizujte PostgreSQL connection string v nezbytných `appsettings.json` souborech
  - Více informací v [PostgreSQL connection strings](https://www.connectionstrings.com/postgresql/), v tomto dokumentu věnujte pozornost sekci `Npgsql`

### Aktualizace projektu EntityFrameworkCore.DbMigrations
- Aktualizace pro použití PostgreSQL v `XXXMigrationsDbContextFactory`
  - Nahraďte `new DbContextOptionsBuilder<XXXMigrationsDbContext>().UseSqlServer()` za `new DbContextOptionsBuilder<XXXMigrationsDbContext>().UseNpgsql()`


### Odstranění stávajících migrací

Smažte všechny stavající migrační soubory (včetně `DbContextModelSnapshot`)

![postgresql-delete-initial-migrations](images/postgresql-delete-initial-migrations.png)

### Znovu vygenerujte počáteční migraci

Nastavte správný spouštěcí projekt (obvykle web projekt)

![set-as-startup-project](../images/set-as-startup-project.png)

Otevřete **Package Manager Console** (Tools -> Nuget Package Manager -> Package Manager Console), zvolte `.EntityFrameworkCore.DbMigrations` jako **Default project** a proveďte následující příkaz:

Proveďte příkaz `Add-Migration`:
````
PM> Add-Migration Initial
````

### Aktualizace databáze

K vytvoření databáze máte dvě možnosti.

#### Použití DbMigrator aplikace

Řešení obsahuje konzolovou aplikaci (v tomto příkladu nazvanou `Acme.BookStore.DbMigrator`), která může vytvářet databáze, aplikovat migrace a vkládat seed data. Je užitečná jak pro vývojové, tak pro produkční prostředí.

> Projekt `.DbMigrator` má vlastní `appsettings.json`. Takže pokud jste změnili connection string uvedený výše, musíte změnit také tento.

Klikněte pravým na projekt `.DbMigrator` a vyberte **Set as StartUp Project**:

![set-as-startup-project](images/set-as-startup-project.png)

Zmáčkněte F5 (nebo Ctrl+F5) ke spuštění aplikace. Výstup bude vypadat následovně:

![set-as-startup-project](images/db-migrator-app.png)

#### Použití EF Core Update-Database příkazu

Ef Core má `Update-Database` příkaz, který v případě potřeby vytvoří databázi a aplikuje čekající migrace.

Nastavte správný spouštěcí projekt (obvykle web projekt)

![set-as-startup-project](../images/set-as-startup-project.png)

Otevřete **Package Manager Console** (Tools -> Nuget Package Manager -> Package Manager Console), vyberte projekt `.EntityFrameworkCore.DbMigrations` jako **Default Project** and spusťte následující příkaz:

````
PM> Update-Database
````

Dojde k vytvoření nové databáze na základě nakonfigurovaného connection stringu.

![postgresql-update-database](images/postgresql-update-database.png)

> Použití nástroje `.DbMigrator` je doporučený způsob, jelikož zároveň vloží seed data nutné k správnému běhu webové aplikace.
