# Начало работы

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Да", "Нет"]
}
````

> Этот документ предполагает, что вы предпочитаете использовать **{{ UI_Value }}** в качестве UI фреймворка  **{{ DB_Value }}** в качестве провайдера баз данных. Для других вариантов, пожалуйста, измените настройки в нижней части этого документа.

## Создание нового проекта

Мы будем использовать ABP CLI для создания нового проекта ABP.

> В качестве альтернативы вы можете **создать и скачать** проекты с [сайта ABP Framework](https://abp.io/get-started) выбрав нужные опции на странице настройки.

Используйте команду `new` в консоли ABP CLI для создания нового проекта:

````shell
abp new Acme.BookStore{{if UI == "NG"}} -u angular{{else if UI == "Blazor"}} -u blazor{{else if UI == "BlazorServer"}} -u blazor-server{{end}}{{if DB == "Mongo"}} -d mongodb{{end}}{{if Tiered == "Yes"}}{{if UI == "MVC" || UI == "BlazorServer"}} --tiered{{else}} --separate-auth-server{{end}}{{end}}
````

*Вы можете использовать разные уровни пространств имен; например BookStore, Acme.BookStore или Acme.Retail.BookStore.*

{{ if Tiered == "Да" }}

{{ if UI == "MVC" || UI == "BlazorServer" }}

* `--tiered` ргумент используется для создания N-уровневого решения, в котором сервер аутентификации, уровни пользовательского интерфейса и API физически разделены.

{{ else }}

* `--separate-auth-server` Аргумент используется для отделения приложения сервера идентификации от хост-приложения API. Если не указано, у вас будет одна конечная точка на сервере.

{{ end }}

{{ end }}

> [Документация ABP CLI](./CLI.md) охватывает все доступные команды и параметры.

### Мобильная разработка

Если вы хотите включить проект [React Native](https://reactnative.dev/) в ваше решение, добавьте `-m react-native` (или `--mobile react-native`) аргумент к команде создания проекта. Это базовый стартовый шаблон React Native для разработки мобильных приложений, интегрированных в ваши серверные части на основе ABP.

Смотрите документ [Начиная работу с React Native](Getting-Started-React-Native.md) чтобы научиться настраивать и запускать приложение React Native.

## Структура решения

Решение имеет слоистую структуру (основанную на концепции [Domain Driven Design](Domain-Driven-Design.md)) и содержит проекты unit & интеграционных тестов. Смотри  [документацию по шаблонам приложений](Startup-Templates/Application.md) для понимания деталий решения. 

{{ if DB == "Mongo" }}

## MongoDB транзакции

[Стартовый шаблон](Startup-templates/Index.md) **отключает** транзакции в проекте `.MongoDB` по умолчанию. Если ваш сервер MongoDB поддерживает транзакции, вы можете включить его в методе `ConfigureServices` класса *YourProjectMongoDbModule*:

  ```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
});
  ```

> Или вы можете удалить этот код, поскольку `Auto` уже является поведением по умолчанию.

{{ end }}

## Следующий шаг

* [Запуск решения](Getting-Started-Running-Solution.md)