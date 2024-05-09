# ASP.NET Core MVC / Razor Pages: O Tema Básico

O Tema Básico é uma implementação de tema para a interface do usuário do ASP.NET Core MVC / Razor Pages. É um tema minimalista que não adiciona nenhum estilo além do [Bootstrap](https://getbootstrap.com/) básico. Você pode usar o Tema Básico como o **tema base** e construir seu próprio tema ou estilo em cima dele. Veja a seção *Customização*.

O Tema Básico possui suporte para idiomas da direita para a esquerda (RTL).

> Se você está procurando um tema profissional e pronto para uso empresarial, você pode conferir o [Tema Lepton](https://commercial.abp.io/themes), que faz parte do [ABP Commercial](https://commercial.abp.io/).

> Veja o documento [Theming](Theming.md) para aprender sobre temas.

## Instalação

Se você precisa instalar manualmente este tema, siga os passos abaixo:

* Instale o pacote NuGet [Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic) em seu projeto web.
* Adicione `AbpAspNetCoreMvcUiBasicThemeModule` no atributo `[DependsOn(...)]` para a sua [classe de módulo](../../Module-Development-Basics.md) no projeto web.
* Instale o pacote NPM [@abp/aspnetcore.mvc.ui.theme.basic](https://www.npmjs.com/package/@abp/aspnetcore.mvc.ui.theme.basic) em seu projeto web (por exemplo, `npm install @abp/aspnetcore.mvc.ui.theme.basic` ou `yarn add @abp/aspnetcore.mvc.ui.theme.basic`).
* Execute o comando `abp install-libs` em um terminal de linha de comando na pasta do projeto web.

## Layouts

O Tema Básico implementa os layouts padrão. Todos os layouts implementam as seguintes partes:

* [Bundles](Bundling-Minification.md) globais
* [Alertas de página](Page-Alerts.md)
* [Hooks de layout](Layout-Hooks.md)
* Recursos de [widget](Widgets.md)

### O Layout da Aplicação

![basic-theme-application-layout](../../images/basic-theme-application-layout.png)

O Layout da Aplicação implementa as seguintes partes, além das partes comuns mencionadas acima:

* Marca
* [Menu](Navigation-Menu.md) principal
* [Toolbar](Toolbars.md) principal com seleção de idioma e menu do usuário

### O Layout da Conta

![basic-theme-account-layout](../../images/basic-theme-account-layout.png)

O Layout da Conta implementa as seguintes partes, além das partes comuns mencionadas acima:

* Marca
* [Menu](Navigation-Menu.md) principal
* [Toolbar](Toolbars.md) principal com seleção de idioma e menu do usuário
* Área de troca de inquilino

### Layout Vazio

O layout vazio é vazio, como o nome sugere. No entanto, ele implementa as partes comuns mencionadas acima.

## Customização

Você tem duas opções para personalizar este tema:

### Sobrescrevendo Estilos/Componentes

Nesta abordagem, você continua a usar o tema como pacotes NuGet e NPM e personaliza as partes que precisa. Existem várias maneiras de personalizá-lo;

#### Sobrescrever os Estilos

1. Crie um arquivo CSS na pasta `wwwroot` do seu projeto:

![example-global-styles](../../images/example-global-styles.png)

2. Adicione o arquivo de estilo ao bundle global, no método `ConfigureServices` da sua [classe de módulo](../../Module-Development-Basics.md):

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.StyleBundles.Configure(BasicThemeBundles.Styles.Global, bundle =>
    {
        bundle.AddFiles("/styles/global-styles.css");
    });
});
````

#### Sobrescrever os Componentes

Veja o [Guia de Customização da Interface do Usuário](Customization-User-Interface.md) para aprender como substituir componentes, personalizar e estender a interface do usuário.

### Copiar e Personalizar

Você pode executar o seguinte comando [ABP CLI](../../CLI.md) no diretório do projeto **Web** para copiar o código-fonte para a sua solução:

`abp add-module Volo.BasicTheme --with-source-code --add-to-solution-file`

----

Ou, você pode baixar o [código-fonte](https://github.com/abpframework/abp/tree/dev/modules/basic-theme/src/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic) do Tema Básico, copiar manualmente o conteúdo do projeto para a sua solução, reorganizar as dependências de pacote/módulo (veja a seção de Instalação acima para entender como ele foi instalado no projeto) e personalizar livremente o tema com base nos requisitos da sua aplicação.

## Veja também

* [Theming](Theming.md)