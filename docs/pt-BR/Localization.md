# Localização

O sistema de localização da ABP é perfeitamente integrado ao `Microsoft.Extensions.Localization`pacote e compatível com a [documentação de localização da Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) . Ele adiciona alguns recursos e aprimoramentos úteis para facilitar o uso em cenários de aplicativos da vida real.

## Pacote Volo.Abp.Localization

> Este pacote já está instalado por padrão com o modelo de inicialização. Portanto, na maioria das vezes, você não precisa instalá-lo manualmente.

Volo.Abp.Localization é o pacote principal do sistema de localização. Instale-o no seu projeto usando o console do gerenciador de pacotes (PMC):

```
Install-Package Volo.Abp.Localization
```

Em seguida, você pode adicionar a dependência **AbpLocalizationModule** ao seu módulo:

```csharp
using Volo.Abp.Modularity;
using Volo.Abp.Localization;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

## Criando um recurso de localização

Um recurso de localização é usado para agrupar cadeias de localização relacionadas e separá-las de outras cadeias de localização do aplicativo. Um [módulo](Module-Development-Basics.md) geralmente define seu próprio recurso de localização. O recurso de localização é apenas uma classe simples. Exemplo:

```csharp
public class TestResource
{
}
```

Em seguida, deve ser adicionado usando `AbpLocalizationOptions`como mostrado abaixo:

```csharp
[DependsOn(typeof(AbpLocalizationModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MyModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            //Define a new localization resource (TestResource)
            options.Resources
                .Add<TestResource>("en")
                .AddVirtualJson("/Localization/Resources/Test");
        });
    }
}
```

Neste exemplo;

- Adicionado um novo recurso de localização com "en" (inglês) como a cultura padrão.
- Arquivos JSON usados para armazenar as sequências de localização.
- Os arquivos JSON são incorporados ao assembly usando `AbpVirtualFileSystemOptions`(consulte [sistema de arquivos virtual](Virtual-File-System.md) ).

Os arquivos JSON estão localizados na pasta do projeto "/ Localização / Recursos / Teste", como mostrado abaixo:

![localization-resource-json-files](images/localization-resource-json-files.png)

Um conteúdo do arquivo de localização JSON é mostrado abaixo:

```json
{
  "culture": "en",
  "texts": {
    "HelloWorld": "Hello World!"
  }
}
```

- Todo arquivo de localização deve definir o `culture`código para o arquivo (como "en" ou "en-US").
- `texts` A seção contém apenas a coleção de valores-chave das sequências de localização (as chaves também podem ter espaços).

### Nome Curto do Recurso de Localização

Os recursos de localização também estão disponíveis no lado do cliente (JavaScript). Portanto, definir um nome abreviado para o recurso de localização facilita o uso de textos de localização. Exemplo:

```csharp
[LocalizationResourceName("Test")]
public class TestResource
{
}
```

Consulte a seção Obtendo teste localizado / lado do cliente abaixo.

### Herdar de outros recursos

Um recurso pode herdar de outros recursos, o que possibilita reutilizar cadeias de localização existentes sem fazer referência ao recurso existente. Exemplo:

```csharp
[InheritResource(typeof(AbpValidationResource))]
public class TestResource
{
}
```

Herança alternativa configurando o `AbpLocalizationOptions`:

```csharp
services.Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Add<TestResource>("en") //Define the resource by "en" default culture
        .AddVirtualJson("/Localization/Resources/Test") //Add strings from virtual json files
        .AddBaseTypes(typeof(AbpValidationResource)); //Inherit from an existing resource
});
```

- Um recurso pode herdar de vários recursos.
- Se o novo recurso definir a mesma sequência localizada, ele substituirá a sequência.

### Estendendo o Recurso Existente

Herdar de um recurso cria um novo recurso sem modificar o existente. Em alguns casos, convém não criar um novo recurso, mas estender diretamente um recurso existente. Exemplo:

```csharp
services.Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<TestResource>()
        .AddVirtualJson("/Localization/Resources/Test/Extensions");
});
```

- Se um arquivo de extensão define a mesma sequência localizada, ele substitui a sequência.

## Obtendo textos localizados

### Lado do servidor

Obter o texto localizado no lado do servidor é bastante padrão.

#### Uso mais simples de uma classe

```csharp
public class MyService
{
    private readonly IStringLocalizer<TestResource> _localizer;

    public MyService(IStringLocalizer<TestResource> localizer)
    {
        _localizer = localizer;
    }

    public void Foo()
    {
        var str = _localizer["HelloWorld"];
    }
}
```

#### Uso mais simples em uma vista / página do Razor

```csharp
@inject IHtmlLocalizer<TestResource> Localizer

<h1>@Localizer["HelloWorld"]</h1>
```

Consulte a [documentação de localização da Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) para obter detalhes sobre o uso da localização no lado do servidor.

### Lado do Cliente

A ABP fornece serviços JavaScript para usar os mesmos textos localizados no lado do cliente.

Obtenha um recurso de localização:

```js
var testResource = abp.localization.getResource('Test');
```

Localize uma sequência:

```js
var str = testResource('HelloWorld');
```


  