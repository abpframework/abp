# ASP.NET Core MVC Bundling & Minification

Existem várias maneiras de agrupar e minificar recursos do lado do cliente (arquivos JavaScript e CSS). As formas mais comuns são:

* Usando a extensão do Visual Studio [Bundler & Minifier](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier) ou o [pacote NuGet](https://www.nuget.org/packages/BuildBundlerMinifier/).
* Usando os gerenciadores de tarefas [Gulp](https://gulpjs.com/)/[Grunt](https://gruntjs.com/) e seus plugins.

O ABP oferece uma maneira simples, dinâmica, poderosa, modular e integrada.

## Pacote Volo.Abp.AspNetCore.Mvc.UI.Bundling

> Este pacote já está instalado por padrão nos modelos de inicialização. Portanto, na maioria das vezes, você não precisa instalá-lo manualmente.

Se você não estiver usando um modelo de inicialização, pode usar o [ABP CLI](../../CLI.md) para instalá-lo em seu projeto. Execute o seguinte comando na pasta que contém o arquivo .csproj do seu projeto:

````
abp add-package Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

> Se você ainda não o fez, primeiro precisa instalar o [ABP CLI](../../CLI.md). Para outras opções de instalação, consulte [a página de descrição do pacote](https://abp.io/package-detail/Volo.Abp.AspNetCore.Mvc.UI.Bundling).

## Tag Helpers de Agrupamento Razor

A maneira mais simples de criar um pacote é usar os tag helpers `abp-script-bundle` ou `abp-style-bundle`. Exemplo:

````html
<abp-style-bundle name="MyGlobalBundle">
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    <abp-style src="/styles/my-global-style.css" />
</abp-style-bundle>
````

Este pacote define um pacote de estilo com um **nome único**: `MyGlobalBundle`. É muito fácil entender como usá-lo. Vamos ver como ele *funciona*:

* O ABP cria o pacote como **lazy** a partir dos arquivos fornecidos quando é **solicitado pela primeira vez**. Para as chamadas subsequentes, ele é retornado do **cache**. Isso significa que, se você adicionar condicionalmente os arquivos ao pacote, ele será executado apenas uma vez e quaisquer alterações na condição não afetarão o pacote para as próximas solicitações.
* O ABP adiciona os arquivos do pacote **individualmente** à página para o ambiente `development`. Ele agrupa e minifica automaticamente para outros ambientes (`staging`, `production`...). Consulte a seção *Modo de Agrupamento* para alterar esse comportamento.
* Os arquivos do pacote podem ser arquivos **físicos** ou [**virtuais/embutidos**](../../Virtual-File-System.md).
* O ABP adiciona automaticamente uma **string de consulta de versão** ao URL do arquivo do pacote para evitar que os navegadores armazenem em cache quando o pacote está sendo atualizado. (como ?_v=67872834243042 - gerado a partir da última data de alteração dos arquivos relacionados). A versão funciona mesmo se os arquivos do pacote forem adicionados individualmente à página (no ambiente de desenvolvimento).

### Importando os Tag Helpers de Agrupamento

> Isso já é importado por padrão nos modelos de inicialização. Portanto, na maioria das vezes, você não precisa adicioná-lo manualmente.

Para usar os tag helpers de pacote, você precisa adicioná-los ao seu arquivo `_ViewImports.cshtml` ou à sua página:

````
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

### Pacotes sem nome

O nome é **opcional** para os tag helpers de pacote razor. Se você não definir um nome, ele será **calculado** automaticamente com base nos nomes dos arquivos do pacote usados (eles são **concatenados** e **hashados**). Exemplo:

````html
<abp-style-bundle>
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    @if (ViewBag.IncludeCustomStyles != false)
    {
        <abp-style src="/styles/my-global-style.css" />
    }
</abp-style-bundle>
````

Isso pode criar **dois pacotes diferentes** (um inclui o `my-global-style.css` e o outro não).

Vantagens de pacotes **sem nome**:

* É possível **adicionar itens condicionalmente** ao pacote. Mas isso pode levar a várias variações do pacote com base nas condições.

Vantagens de pacotes **com nome**:

* Outros **módulos podem contribuir** para o pacote pelo nome (consulte as seções abaixo).

### Arquivo Único

Se você precisa apenas adicionar um único arquivo à página, pode usar a tag `abp-script` ou `abp-style` sem envolvê-la na tag `abp-script-bundle` ou `abp-style-bundle`. Exemplo:

````xml
<abp-script src="/scripts/my-script.js" />
````

O nome do pacote será *scripts.my-scripts* para o exemplo acima ("/" é substituído por "."). Todos os recursos de agrupamento funcionam como esperado para pacotes de arquivo único também.

## Opções de Agrupamento

Se você precisa usar o mesmo pacote em **múltiplas páginas** ou deseja usar alguns recursos mais **poderosos**, você pode configurar pacotes **por código** em sua classe de [módulo](../../Module-Development-Basics.md).

### Criando um Novo Pacote

Exemplo de uso:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
public class MyWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .Add("MyGlobalBundle", bundle => {
                    bundle.AddFiles(
                        "/libs/jquery/jquery.js",
                        "/libs/bootstrap/js/bootstrap.js",
                        "/libs/toastr/toastr.min.js",
                        "/scripts/my-global-scripts.js"
                    );
                });                
        });
    }
}
````

> Você pode usar o mesmo nome (*MyGlobalBundle* aqui) para um pacote de script e estilo, pois eles são adicionados a coleções diferentes (`ScriptBundles` e `StyleBundles`).

Após definir um pacote desse tipo, ele pode ser incluído em uma página usando os mesmos tag helpers definidos acima. Exemplo:

````html
<abp-script-bundle name="MyGlobalBundle" />
````

Desta vez, nenhum arquivo é definido na definição do tag helper porque os arquivos do pacote são definidos pelo código.

### Configurando um Pacote Existente

O ABP também oferece suporte à [modularidade](../../Module-Development-Basics.md) para agrupamento. Um módulo pode modificar um pacote existente criado por um módulo dependente. Exemplo:

````C#
[DependsOn(typeof(MyWebModule))]
public class MyWebExtensionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .Configure("MyGlobalBundle", bundle => {
                    bundle.AddFiles(
                        "/scripts/my-extension-script.js"
                    );
                });
        });
    }
}
````

Você também pode usar o método `ConfigureAll` para configurar todos os pacotes existentes:

````C#
[DependsOn(typeof(MyWebModule))]
public class MyWebExtensionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .ConfigureAll(bundle => {
                    bundle.AddFiles(
                        "/scripts/my-extension-script.js"
                    );
                });
        });
    }
}
````

## Contribuidores de Pacotes

Adicionar arquivos a um pacote existente é útil. E se você precisar **substituir** um arquivo no pacote ou quiser adicionar arquivos **condicionalmente**? Definir um contribuidor de pacote fornece poder extra para esses casos.

Um exemplo de contribuidor de pacote que substitui o bootstrap.css por uma versão personalizada:

````C#
public class MyExtensionGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.ReplaceOne(
            "/libs/bootstrap/css/bootstrap.css",
            "/styles/extensions/bootstrap-customized.css"
        );
    }
}
````

Em seguida, você pode usar esse contribuidor da seguinte maneira:

````C#
services.Configure<AbpBundlingOptions>(options =>
{
    options
        .ScriptBundles
        .Configure("MyGlobalBundle", bundle => {
            bundle.AddContributors(typeof(MyExtensionGlobalStyleContributor));
        });
});
````

> Você também pode adicionar contribuidores ao criar um novo pacote.

Os contribuidores também podem ser usados nos tag helpers de pacote. Exemplo:

````xml
<abp-style-bundle>
    <abp-style type="@typeof(BootstrapStyleContributor)" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
</abp-style-bundle>
````

As tags `abp-style` e `abp-script` podem receber atributos `type` (em vez de atributos `src`) como mostrado neste exemplo. Quando você adiciona um contribuidor de pacote, suas dependências também são adicionadas automaticamente ao pacote.

### Dependências de Contribuidores

Um contribuidor de pacote pode ter uma ou mais dependências de outros contribuidores. 
Exemplo:

````C#
[DependsOn(typeof(MyDependedBundleContributor))] //Define a dependência
public class MyExtensionStyleBundleContributor : BundleContributor
{
    //...
}
````

Quando um contribuidor de pacote é adicionado, suas dependências são adicionadas **automaticamente e recursivamente**. As dependências são adicionadas pela **ordem de dependência** evitando **duplicatas**. As duplicatas são evitadas mesmo que estejam em pacotes separados. O ABP organiza todos os pacotes em uma página e elimina duplicações.

Criar contribuidores e definir dependências é uma maneira de organizar a criação de pacotes em diferentes módulos.

### Extensões de Contribuidores

Em alguns cenários avançados, você pode querer fazer alguma configuração adicional sempre que um contribuidor de pacote for usado. As extensões de contribuidor funcionam perfeitamente quando o contribuidor estendido é usado.

O exemplo abaixo adiciona alguns estilos para a biblioteca prism.js:

````csharp
public class MyPrismjsStyleExtension : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/prismjs/plugins/toolbar/prism-toolbar.css");
    }
}
````

Em seguida, você pode configurar `AbpBundleContributorOptions` para estender o `PrismjsStyleBundleContributor` existente.

````csharp
Configure<AbpBundleContributorOptions>(options =>
{
    options
        .Extensions<PrismjsStyleBundleContributor>()
        .Add<MyPrismjsStyleExtension>();
});
````

Sempre que `PrismjsStyleBundleContributor` for adicionado a um pacote, `MyPrismjsStyleExtension` também será adicionado automaticamente.

### Acessando o IServiceProvider

Embora raramente seja necessário, `BundleConfigurationContext` possui uma propriedade `ServiceProvider` que permite resolver dependências de serviço dentro do método `ConfigureBundle`.

### Contribuidores de Pacotes Padrão

Adicionar um recurso específico do pacote NPM (arquivos js, css) a um pacote é bastante simples para esse pacote. Por exemplo, você sempre adiciona o arquivo `bootstrap.css` para o pacote NPM do bootstrap.

Existem contribuidores embutidos para todos os [pacotes NPM padrão](Client-Side-Package-Management.md). Por exemplo, se seu contribuidor depende do bootstrap, você pode apenas declará-lo, em vez de adicionar o bootstrap.css você mesmo.

````C#
[DependsOn(typeof(BootstrapStyleContributor))] //Define a dependência de estilo do bootstrap
public class MyExtensionStyleBundleContributor : BundleContributor
{
    //...
}
````

Usando os contribuidores embutidos para pacotes padrão;

* Impede que você digite **os caminhos de recursos inválidos**.
* Impede a alteração do seu contribuidor se o **caminho do recurso mudar** (o contribuidor dependente lidará com isso).
* Impede que vários módulos adicionem **arquivos duplicados**.
* Gerencia **dependências recursivamente** (adiciona dependências de dependências, se necessário).

#### Pacote Volo.Abp.AspNetCore.Mvc.UI.Packages

> Este pacote já está instalado por padrão nos modelos de inicialização. Portanto, na maioria das vezes, você não precisa instalá-lo manualmente.

Se você não estiver usando um modelo de inicialização, pode usar o [ABP CLI](../../CLI.md) para instalá-lo em seu projeto. Execute o seguinte comando na pasta que contém o arquivo .csproj do seu projeto:

````
abp add-package Volo.Abp.AspNetCore.Mvc.UI.Packages
````

> Se você ainda não o fez, primeiro precisa instalar o [ABP CLI](../../CLI.md). Para outras opções de instalação, consulte [a página de descrição do pacote](https://abp.io/package-detail/Volo.Abp.AspNetCore.Mvc.UI.Packages).

### Herança de Pacotes

Em alguns casos específicos, pode ser necessário criar um **novo** pacote **herdado** de outro(s) pacote(s). Ao herdar de um pacote (recursivamente), todos os arquivos/contribuidores desse pacote são herdados. Em seguida, o pacote derivado pode adicionar ou modificar arquivos/contribuidores **sem modificar** o pacote original. 
Exemplo:

````c#
services.Configure<AbpBundlingOptions>(options =>
{
    options
        .StyleBundles
        .Add("MyTheme.MyGlobalBundle", bundle => {
            bundle
                .AddBaseBundles("MyGlobalBundle") //Pode adicionar vários
                .AddFiles(
                    "/styles/mytheme-global-styles.css"
                );
        });
});
````

## Opções Adicionais

Esta seção mostra outras opções úteis para o sistema de agrupamento e minificação.

### Modo de Agrupamento

O ABP adiciona arquivos de pacote individualmente à página para o ambiente `development`. Ele agrupa e minifica automaticamente para outros ambientes (`staging`, `production`...). Na maioria das vezes, esse é o comportamento desejado. No entanto, você pode configurá-lo manualmente em alguns casos. Existem quatro modos;

* `Auto`: Determina automaticamente o modo com base no ambiente.
* `None`: Sem agrupamento ou minificação.
* `Bundle`: Agrupado, mas não minificado.
* `BundleAndMinify`: Agrupado e minificado.

Você pode configurar `AbpBundlingOptions` no `ConfigureServices` do seu [módulo](../../Module-Development-Basics.md).

**Exemplo:**

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.Mode = BundlingMode.Bundle;
});
````

### Ignorar para Minificação

É possível ignorar um arquivo específico para a minificação.

**Exemplo:**

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.MinificationIgnoredFiles.Add("/scripts/myscript.js");
});
````

O arquivo fornecido ainda é adicionado ao pacote, mas não é minificado neste caso.

### Carregar JavaScript e CSS de forma assíncrona

Você pode configurar `AbpBundlingOptions` para carregar todos ou um único arquivo js/css de forma assíncrona.

**Exemplo:**

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.PreloadStyles.Add("/__bundles/Basic.Global");
    options.DeferScriptsByDefault = true;
});
````

**HTML de saída:**
````html
<link rel="preload" href="/__bundles/Basic.Global.F4FA61F368098407A4C972D0A6914137.css?_v=637697363694828051" as="style" onload="this.rel='stylesheet'"/>

<script defer src="/libs/timeago/locales/jquery.timeago.en.js?_v=637674729040000000"></script>
````

### Suporte a Arquivos Externos/CDN

O sistema de agrupamento reconhece automaticamente os arquivos externos/CDN e os adiciona à página sem nenhuma alteração.

#### Usando Arquivos Externos/CDN em `AbpBundlingOptions`

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.StyleBundles
        .Add("MyStyleBundle", configuration =>
        {
            configuration
                .AddFiles("/styles/my-style1.css")
                .AddFiles("/styles/my-style2.css")
                .AddFiles("https://cdn.abp.io/bootstrap.css")
                .AddFiles("/styles/my-style3.css")
                .AddFiles("/styles/my-style4.css");
        });

    options.ScriptBundles
        .Add("MyScriptBundle", configuration =>
        {
            configuration
                .AddFiles("/scripts/my-script1.js")
                .AddFiles("/scripts/my-script2.js")
                .AddFiles("https://cdn.abp.io/bootstrap.js")
                .AddFiles("/scripts/my-script3.js")
                .AddFiles("/scripts/my-script4.js");
        });
});
````

**HTML de saída:**

````html
<link rel="stylesheet" href="/__bundles/MyStyleBundle.EA8C28419DCA43363E9670973D4C0D15.css?_v=638331889644609730" />
<link rel="stylesheet" href="https://cdn.abp.io/bootstrap.css" />
<link rel="stylesheet" href="/__bundles/MyStyleBundle.AC2E0AA6C461A0949A1295E9BDAC049C.css?_v=638331889644623860" />

<script src="/__bundles/MyScriptBundle.C993366DF8840E08228F3EE685CB08E8.js?_v=638331889644937120"></script>
<script src="https://cdn.abp.io/bootstrap.js"></script>
<script src="/__bundles/MyScriptBundle.2E8D0FDC6334D2A6B847393A801525B7.js?_v=638331889644943970"></script>
````

#### Usando Arquivos Externos/CDN em Tag Helpers.

````html
<abp-style-bundle name="MyStyleBundle">
    <abp-style src="/styles/my-style1.css" />
    <abp-style src="/styles/my-style2.css" />
    <abp-style src="https://cdn.abp.io/bootstrap.css" />
    <abp-style src="/styles/my-style3.css" />
    <abp-style src="/styles/my-style4.css" />
</abp-style-bundle>

<abp-script-bundle name="MyScriptBundle">
    <abp-script src="/scripts/my-script1.js" />
    <abp-script src="/scripts/my-script2.js" />
    <abp-script src="https://cdn.abp.io/bootstrap.js" />
    <abp-script src="/scripts/my-script3.js" />
    <abp-script src="/scripts/my-script4.js" />
</abp-script-bundle>
````

**HTML de saída:**

````html
<link rel="stylesheet" href="/__bundles/MyStyleBundle.C60C7B9C1F539659623BB6E7227A7C45.css?_v=638331889645002500" />
<link rel="stylesheet" href="https://cdn.abp.io/bootstrap.css" />
<link rel="stylesheet" href="/__bundles/MyStyleBundle.464328A06039091534650B0E049904C6.css?_v=638331889645012300" />

<script src="/__bundles/MyScriptBundle.55FDCBF2DCB9E0767AE6FA7487594106.js?_v=638331889645050410"></script>
<script src="https://cdn.abp.io/bootstrap.js"></script>
<script src="/__bundles/MyScriptBundle.191CB68AB4F41C8BF3A7AE422F19A3D2.js?_v=638331889645055490"></script>
````

## Temas

Os temas usam os contribuidores de pacotes padrão para adicionar recursos de biblioteca aos layouts de página. Os temas também podem definir alguns pacotes padrão/globais, para que qualquer módulo possa contribuir para esses pacotes padrão/globais. Consulte a documentação de [temas](Theming.md) para mais informações.

## Melhores Práticas e Sugestões

É sugerido definir vários pacotes para uma aplicação, cada um usado para diferentes propósitos.

* **Pacote global**: Pacotes de estilo/script globais são incluídos em todas as páginas da aplicação. Os temas já definem pacotes de estilo e script globais. Seu módulo pode contribuir para eles.
* **Pacotes de layout**: Este é um pacote específico para um layout individual. Contém apenas recursos compartilhados entre todas as páginas que usam o layout. Use os tag helpers de agrupamento para criar o pacote como uma boa prática.
* **Pacotes de módulo**: Para recursos compartilhados entre as páginas de um módulo individual.
* **Pacotes de página**: Pacotes específicos criados para cada página. Use os tag helpers de agrupamento para criar o pacote como uma melhor prática.

Estabeleça um equilíbrio entre desempenho, uso de largura de banda da rede e quantidade de pacotes.

## Veja também

* [Gerenciamento de Pacotes do Lado do Cliente](Client-Side-Package-Management.md)
* [Temas](Theming.md)