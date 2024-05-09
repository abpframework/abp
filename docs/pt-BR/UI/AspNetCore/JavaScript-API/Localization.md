# ASP.NET Core MVC / Razor Pages UI: API de Localização JavaScript

A API de localização permite que você reutilize os recursos de localização do lado do servidor no lado do cliente.

> Este documento explica apenas a API JavaScript. Consulte o [documento de localização](../../../Localization.md) para entender o sistema de localização do ABP.

## Uso básico

A função `abp.localization.getResource(...)` é usada para obter um recurso de localização:

````js
var testResource = abp.localization.getResource('Test');
````

Em seguida, você pode localizar uma string com base nesse recurso:

````js
var str = testResource('HelloWorld');
````

A função `abp.localization.localize(...)` é um atalho onde você pode especificar tanto o nome do texto quanto o nome do recurso:

````js
var str = abp.localization.localize('HelloWorld', 'Test');
````

`HelloWorld` é o texto a ser localizado, onde `Test` é o nome do recurso de localização aqui.

### Lógica de fallback

Se os textos fornecidos não forem localizados, o método de localização retorna a chave fornecida como resultado da localização.

### Recurso de Localização Padrão

Se você não especificar o nome do recurso de localização, ele usará o **recurso de localização padrão** definido nas `AbpLocalizationOptions` (consulte o [documento de localização](../../../Localization.md)).

**Exemplo: Usando o recurso de localização padrão**

````js
var str = abp.localization.localize('HelloWorld'); // usa o recurso padrão
````

### Argumentos de Formato

Se sua string localizada contiver argumentos, como `Olá {0}, bem-vindo!`, você pode passar argumentos para os métodos de localização. Exemplos:

````js
var testSource = abp.localization.getResource('Test');
var str1 = testSource('HelloWelcomeMessage', 'John');
var str2 = abp.localization.localize('HelloWelcomeMessage', 'Test', 'John');
````

Supondo que `HelloWelcomeMessage` seja localizado como `Olá {0}, bem-vindo!`, ambos os exemplos acima produzem a saída `Olá John, bem-vindo!`.

## Outras Propriedades e Métodos

### abp.localization.resources

A propriedade `abp.localization.resources` armazena todos os recursos de localização, chaves e seus valores.

### abp.localization.isLocalized

Retorna um booleano indicando se o texto fornecido foi localizado ou não.

**Exemplo**

````js
abp.localization.isLocalized('ProductName', 'MyResource');
````

Retorna `true` se o texto `ProductName` foi localizado para o recurso `MyResource`. Caso contrário, retorna `false`. Você pode deixar o nome do recurso vazio para usar o recurso de localização padrão.

### abp.localization.defaultResourceName

`abp.localization.defaultResourceName` pode ser definido para alterar o recurso de localização padrão. Normalmente, você não define isso, pois o ABP Framework o define automaticamente com base na configuração do lado do servidor.

### abp.localization.currentCulture

`abp.localization.currentCulture` retorna um objeto para obter informações sobre o **idioma atualmente selecionado**.

Um exemplo de valor desse objeto é mostrado abaixo:

````js
{
  "displayName": "Inglês",
  "englishName": "Inglês",
  "threeLetterIsoLanguageName": "eng",
  "twoLetterIsoLanguageName": "en",
  "isRightToLeft": false,
  "cultureName": "en",
  "name": "en",
  "nativeName": "Inglês",
  "dateTimeFormat": {
    "calendarAlgorithmType": "Calendário Solar",
    "dateTimeFormatLong": "dddd, d 'de' MMMM 'de' yyyy",
    "shortDatePattern": "dd/MM/yyyy",
    "fullDateTimePattern": "dddd, d 'de' MMMM 'de' yyyy HH:mm:ss",
    "dateSeparator": "/",
    "shortTimePattern": "HH:mm",
    "longTimePattern": "HH:mm:ss"
  }
}
````

### abp.localization.languages

Usado para obter a lista de todos os **idiomas disponíveis** na aplicação. Um exemplo de valor desse objeto é mostrado abaixo:

````js
[
  {
    "cultureName": "en",
    "uiCultureName": "en",
    "displayName": "Inglês",
    "flagIcon": null
  },
  {
    "cultureName": "fr",
    "uiCultureName": "fr",
    "displayName": "Francês",
    "flagIcon": null
  },
  {
    "cultureName": "pt-BR",
    "uiCultureName": "pt-BR",
    "displayName": "Português",
    "flagIcon": null
  },
  {
    "cultureName": "tr",
    "uiCultureName": "tr",
    "displayName": "Turco",
    "flagIcon": null
  },
  {
    "cultureName": "zh-Hans",
    "uiCultureName": "zh-Hans",
    "displayName": "Chinês Simplificado",
    "flagIcon": null
  }
]
````

## Veja também

* [Tutorial em vídeo](https://abp.io/video-courses/essentials/localization)