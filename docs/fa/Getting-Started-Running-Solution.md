<div dir="rtl">
# آغاز به کار

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> این مستند فرض میکند شما میخواهید از **{{ UI_Value }}** به عنوان رابط کاربری و از  **{{ DB_Value }}** به عنوان ارائه دهنده پایگاه داده استفاده نمایید. برای گزینه های دیگر، لطفاً اولویت را در بالای این سند تغییر دهید.

## ایجاد پایگاه داده

### Connection String

تنظیمات مربوطه به **connection string** را در فایل `appsettings.json` درقسمت {{if Tiered == "Yes"}}`.IdentityServer` و `.HttpApi.Host` projects{{else}}{{if UI=="MVC"}}`.Web` project{{else if UI=="BlazorServer"}}`.Blazor` project{{else}}`.HttpApi.Host` project{{end}}{{end}}. بررسی نمایید

{{ if DB == "EF" }}

````json
"ConnectionStrings": {
  "Default": "Server=(LocalDb)\MSSQLLocalDB;Database=BookStore;Trusted_Connection=True"
}
````

> **در خصوص Connection Strings سیستم های مدیریت پایگاه داده**
>
> سلوشن مذکور برای **Entity Framework Core** با **MS SQL Server** میباشد. با این حال، اگر DBMS دیگری را ترجیح میدهید پارامتر با استفاده از `-dbms` در ABP CLI `new` میتوانید تنظیم نمایید (مثال `-dbms MySQL`), تنظیمات connection string میتواند برای شما متفاوت باشد.
>
> EF Core از پایگاه داده های [متنوعی](https://docs.microsoft.com/en-us/ef/core/providers/) پشتیبانی مینماید و شما میتوانید از هر سیستمی که قابل پشتیبانی میباشد استفاده نمایید. بخش [سند یکپارچه سازی Entity Framework](Entity-Framework-Core.md) مطالب مهمی در خصوص [به DBMS دیگری بروید](Entity-Framework-Core-Other-DBMS.md) ارایه مینماید.

### Database Migrations

پروژه از مدل ساختاری [Entity Framework Core Code First Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli). که با کنسول `DbMigrator` میباشد و  **Migration ها را اعمال می کند** و همچنین **داده های اولیه را seed می کند**. در بخش **development** و همچنین **production** بسیار کاربردی است.

>پروژه های `DbMigrator` فایل `appsettings.json`. خود را دارند, اگر Connection String بالا را تغییر داده اید، باید این یکی را نیز تغییر دهید.

### اعمال Migration اولیه

دقت داشته باشید که `.DbMigrator` به صورت اتوماتیک در نخستین اجرا **migration اولیه را ایجاد می کند** 

**اگر از ویژوال استودیو استفاده می کنید، می توانید به آن بروید *Running the DbMigrator* section.** با این حال، IDE های دیگر (به عنوان مثال Rider) ممکن است برای اولین اجرا با مشکل مواجه شوند زیرا مهاجرت اولیه را اضافه می کند و پروژه را کامپایل می کند. در این مورد، یک ترمینال خط فرمان را در پوشه باز کنید `DbMigrator` و دستور را اجرا کنید:

````bash
dotnet run
````

برای دفعه بعد، می توانید آن را همانطور که معمولاً انجام می دهید در IDE خود اجرا کنید.

### اجرای DbMigrator

روی مورد `DbMigrator` کلیک راست کنید  و گزینه **Set as StartUp Project** را انتخاب نمایید

![set-as-startup-project](images/set-as-startup-project.png)

 کلید F5 (یا Ctrl+F5) فشار دهید تا برنامه اجرا شود و صفحه زیر را مشاهده خواهید کرد:

 ![db-migrator-output](images/db-migrator-output.png)

> به ساید داشته باشید که  [seed data](Data-Seeding.md)  باعث ایجاد نام کابری  `admin` با (رمز عبور `1q2w3E*`) در پایگاه داده میشود که برای ورود لازم است. بنابراین از `.DbMigrator` حداقل یک بار استفاده خواهید کرد.

{{ else if DB == "Mongo" }}

````json
"ConnectionStrings": {
  "Default": "mongodb://localhost:27017/BookStore"
}
````

راه حل برای استفاده از **MongoDB** در رایانه محلی شما پیکربندی شده است، بنابراین شما باید یک نمونه سرور MongoDB داشته باشید و در حال اجرا باشید یا Connection String را به سرور MongoDB دیگری تغییر دهید.

### ایجاد داده های اولیه

راه حل با یک برنامه کنسول `.DbMigrator` ارائه می‌شود که **داده‌های اولیه را نشان می‌دهد**. هم در زمان **development** و هم در زمان **production**.

>همچنین `.DbMigrator` فایل مخصوص `appsettings.json`. دارد, اگر Connection String بالا را تغییر داده اید، باید این یکی را نیز تغییر دهید. 

روی `.DbMigrator` کلیک راست کرده و **Set as StartUp Project** را انتخاب نمایید

![set-as-startup-project](images/set-as-startup-project.png)

F5 (یا Ctrl+F5) را بزنید تا برنامه اجرا شود. خروجی مانند شکل زیر خواهد داشت:

 ![db-migrator-output](images/db-migrator-output.png)

> به ساید داشته باشید که  [seed data](Data-Seeding.md)  باعث ایجاد نام کابری  `admin` با (رمز عبور `1q2w3E*`) در پایگاه داده میشود که برای ورود لازم است. بنابراین از `.DbMigrator` حداقل یک بار استفاده خواهید کرد.

{{ end }}

## برنامه را اجرا کنید

> لطفاً دستور `abp install-libs` را برای بازیابی lib های مورد نیاز پروژه وب قبل از اجرای برنامه اجرا کنید.

{{ if UI == "MVC" || UI == "BlazorServer" }}

{{ if Tiered == "Yes" }}

> Tiered solutions use **Redis** as the distributed cache. Ensure that it is installed and running in your local computer. If you are using a remote Redis Server, set the configuration in the `appsettings.json` files of the projects below.

1. Ensure that the `.IdentityServer` project is the startup project. Run this application that will open a **login** page in your browser.

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

You can login, but you cannot enter to the main application here. This is **just the authentication server**.

2. Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a **Swagger UI** in your browser.

![swagger-ui](images/swagger-ui.png)

This is the HTTP API that is used by the web application.

3. Lastly, ensure that the {{if UI=="MVC"}}`.Web`{{else}}`.Blazor`{{end}} project is the startup project and run the application which will open a **welcome** page in your browser

![mvc-tiered-app-home](images/bookstore-home.png)

Click to the **login** button which will redirect you to the *authentication server* to login to the application:

![bookstore-login](images/bookstore-login.png)

{{ else # Tiered != "Yes" }}

Ensure that the {{if UI=="MVC"}}`.Web`{{else}}`.Blazor`{{end}} project is the startup project. Run the application which will open the **login** page in your browser:

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

![bookstore-login](images/bookstore-login.png)

{{ end # Tiered }}

{{ else # UI != MVC || BlazorServer }}

### Running the HTTP API Host (Server Side)

{{ if Tiered == "Yes" }}

> Tiered solutions use Redis as the distributed cache. Ensure that it is installed and running in your local computer. If you are using a remote Redis Server, set the configuration in the `appsettings.json` files of the projects below.

Ensure that the `.IdentityServer` project is the startup project. Run the application which will open a **login** page in your browser.

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

You can login, but you cannot enter to the main application here. This is **just the authentication server**.

Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a Swagger UI:

{{ else # Tiered == "No" }}

Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a Swagger UI:

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

{{ end # Tiered }}

![swagger-ui](images/swagger-ui.png)

You can see the application APIs and test them here. Get [more info](https://swagger.io/tools/swagger-ui/) about the Swagger UI.

{{ end # UI }}

{{ if UI == "Blazor" }}

### Running the Blazor Application (Client Side)

Ensure that the `.Blazor` project is the startup project and run the application.

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

Once the application starts, click to the **Login** link on to header, which redirects you to the authentication server to enter a username and password:

![bookstore-login](images/bookstore-login.png)

{{ else if UI == "NG" }}

### Running the Angular Application (Client Side)

Go to the `angular` folder, open a command line terminal, type the `yarn` command (we suggest to the [yarn](https://yarnpkg.com/) package manager while `npm install` will also work)

```bash
yarn
```

Once all node modules are loaded, execute `yarn start` (or `npm start`) command:

```bash
yarn start
```

It may take a longer time for the first build. Once it finishes, it opens the Angular UI in your default browser with the [localhost:4200](http://localhost:4200/) address.

![bookstore-login](images/bookstore-login.png)

{{ end }}

Enter **admin** as the username and **1q2w3E*** as the password to login to the application. The application is up and running. You can start developing your application based on this startup template.

## See Also

* [Web Application Development Tutorial](Tutorials/Part-1.md)
* [Application Startup Template](Startup-Templates/Application.md)
</div>