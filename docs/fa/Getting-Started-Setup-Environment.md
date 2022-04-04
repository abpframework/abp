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

> این مستند فرض می‌کند که ترجیح می‌دهید از **{{ UI_Value }}** به عنوان فریم ورک رابط کاربری و **{{ DB_Value }}** به عنوان ارائه‌دهنده پایگاه داده استفاده کنید. برای گزینه های دیگر تنظیمات بالا را تغییر دهید.

## تنظیمات محیط توسعه و برنامه نویسی

بیایید قبل از ایجاد پروژه، محیط توسعه خود را تنظیم کنیم.

### پیش نیازها

ابزار ها و برنامه های ذکر شده در زیر را باید داشته باشید:

* محیط برنامه نویسی (مثال: [Visual Studio](https://visualstudio.microsoft.com/vs/)) با قابلیت پشتیبانی از  [.NET 6.0+](https://dotnet.microsoft.com/download/dotnet) جهت توسعه.
{{ if UI != "Blazor" }}
* [Node v12 or v14](https://nodejs.org/)
* [Yarn v1.20+ (not v2)](https://classic.yarnpkg.com/en/docs/install) <sup id="a-yarn">[1](#f-yarn)</sup> و یا npm v6+ (به صورت پیشفرض با نود نصب میشود)
{{ end }}
{{ if Tiered == "Yes" }}
* [Redis](https://redis.io/) (پروژه اصلی از رادیس استفاده خواهد نمود [distributed cache](Caching.md)).
{{ end }}

{{ if UI != "Blazor" }}

<sup id="f-yarn"><b>1</b></sup> _Yarn نسخه دو فعالیت متفاوتی داشته و فعلا پشتیبانی نمیشود_ <sup>[↩](#a-yarn)</sup>

{{ end }}

### نصب ABP CLI

[کنسول ABP CLI](./CLI.md) یک رابط دستوری است که به ایجاد فایل های اصلی پروژه را اتوماسیون مینماید و بر بستر فریم ورک ABP استفاده می شود. ابتدا باید ABP CLI را با استفاده از دستور زیر نصب کنید:

````shell
dotnet tool install -g Volo.Abp.Cli
````

در صورتی که از قبل نصب شده است، میتونید از طریق دستور زیر بروز رسانی نمایید:

````shell
dotnet tool update -g Volo.Abp.Cli
````

## مرحله بعدی

* [ایجاد پروژه جدید](Getting-Started-Create-Solution.md)

</div>