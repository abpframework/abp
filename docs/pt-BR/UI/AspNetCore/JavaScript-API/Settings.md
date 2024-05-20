# ASP.NET Core MVC / Razor Pages UI: API de Configuração JavaScript

A API de localização permite que você obtenha os valores das configurações no lado do cliente. Você pode ler o valor atual de uma configuração apenas no lado do cliente se isso for permitido pela definição da configuração (no lado do servidor).

> Este documento explica apenas a API JavaScript. Consulte o [documento de configurações](../../../Settings.md) para entender o sistema de configurações do ABP.

## Uso básico

````js
// Obtém um valor como string.
var language = abp.setting.get('Abp.Localization.DefaultLanguage');

// Obtém um valor inteiro.
var requiredLength = abp.setting.getInt('Abp.Identity.Password.RequiredLength');

// Obtém um valor booleano.
var requireDigit = abp.setting.getBoolean('Abp.Identity.Password.RequireDigit');
````

## Todos os valores

`abp.setting.values` pode ser usado para obter todos os valores de configuração como um objeto, onde as propriedades do objeto são os nomes das configurações e os valores das propriedades são os valores das configurações.

Um exemplo de valor desse objeto é mostrado abaixo:

````js
{
  Abp.Localization.DefaultLanguage: "en",
  Abp.Timing.TimeZone: "UTC",
  ...
}
````