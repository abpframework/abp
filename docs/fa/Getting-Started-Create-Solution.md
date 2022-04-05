# آغاز به کار

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> این مستند فرض می‌کند که ترجیح می‌دهید از **{{ UI_Value }}** به عنوان فریم ورک رابط کاربری و **{{ DB_Value }}** به عنوان ارائه‌دهنده پایگاه داده استفاده کنید. برای گزینه های دیگر تنظیمات بالا را تغییر دهید.

## ایجاد پروژه جدید

جهت ایجاد پروژه جدید از ABP CLI استفاده میکنیم

> همچنین میتوانید از طریق لیک **ایجاد و دانلود** پروژه خود را از طریق  [وب سایت فریم ورک ABP](https://abp.io/get-started) ایجاد و بر اساس تنظیمات مورد نظر خود تولید کنید

با استفاده از دستور `new` در ABP CLI میتوانید پروژه را ایجاد نمایید :

````shell
abp new Acme.BookStore{{if UI == "NG"}} -u angular{{else if UI == "Blazor"}} -u blazor{{else if UI == "BlazorServer"}} -u blazor-server{{end}}{{if DB == "Mongo"}} -d mongodb{{end}}{{if Tiered == "Yes"}}{{if UI == "MVC" || UI == "BlazorServer"}} --tiered{{else}} --separate-identity-server{{end}}{{end}}
````

*میتوانید از نام گذاری های دلخواه خود استفاده نمایید مثال BookStore, Acme.BookStore و یا Acme.Retail.BookStore.* 

{{ if Tiered == "Yes" }}

{{ if UI == "MVC" || UI == "BlazorServer" }}

* `--tiered` با استفاده از این گذینه میتوانید از مدل چند لایه استفاده نمایید که لایه Authentıcatıon، لایه API و واست کاربری مجزا باشند.

{{ else }}

* `--separate-identity-server` با استفاده از این گذینه میتوانید Identity Server را از API جدا نمایید. در صورتی که از این پارامتر استفاده نشود، پروژه دارای یک endpoint خواهد بود.

{{ end }}

{{ end }}

> [ABP CLI document](./CLI.md) تمام اطلاعات مربوط به دستور ها در این لینک موجود میباشد.

## توسعه نرم افزار موبایل

جهت استفاده از ساختار های [React Native](https://reactnative.dev/) در پروژه خود از دستور  `-m react-native` (و یا `--mobile react-native`) استفاده نمایید. این دستور اجازه میدهد تمپلیت اولیه مربوط به ری اکت نیتیو مناسب تولید نرم افزار های تحت موبایل به پروژه اضافه شود


بخش  [آغاز به کار با ری اکت ](Getting-Started-React-Native.md) را جهت راه اندازی پروژه های مربوط به ری اکت بررسی نمایید.

### ساختار سلوشن

ساختار سلوشن به صورت لایه بندی میباشد (بر اساس معماری  [Domain Driven Design](Domain-Driven-Design.md)) و شامل یونیت تست میباشد. لینک  [application template document](Startup-Templates/Application.md) to را جهت آموزش و استفاده بررسی نمایید

{{ if DB == "Mongo" }}

#### پایگاه داده مانگو دی بی

ساختار  [startup template](Startup-templates/Index.md) **به صورت پیش فرض** تراکنش های مربوط به `.MongoDB` را غیر فعال مینماید. اگر پروژه شما از تراکنش های مانگو دی بی استفاده میکند میتوانید از طریق کلاس *YourProjectMongoDbModule* و متد `ConfigureServices` تنظیم نمایید:

  ```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
});
  ```

> البته نیازی به این خط از کد نخواهید داشت چرا که `Auto` به صورت پیش فرض فعال میباشد.

{{ end }}

## مرحله بعدی

* [راه اندازی پروژه](Getting-Started-Running-Solution.md)