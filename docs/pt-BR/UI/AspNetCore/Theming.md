# ASP.NET Core MVC / Razor Pages: Tematização de UI

## Introdução

O ABP Framework fornece um sistema completo de **tematização de UI** com os seguintes objetivos:

* Módulos de aplicativos reutilizáveis são desenvolvidos de forma **independente de tema**, para que possam funcionar com qualquer tema de UI.
* O tema de UI é **decidido pela aplicação final**.
* O tema é distribuído por meio de pacotes NuGet/NPM, para que seja **facilmente atualizável**.
* A aplicação final pode **personalizar** o tema selecionado.

Para alcançar esses objetivos, o ABP Framework:

* Determina um conjunto de **bibliotecas base** usadas e adaptadas por todos os temas. Assim, os desenvolvedores de módulos e aplicativos podem depender e usar essas bibliotecas sem depender de um tema específico.
* Fornece um sistema que consiste em [menus de navegação](../../Navigation-Menu.md), [barras de ferramentas](../../Toolbars.md), [hooks de layout](../../Layout-Hooks.md)... que são implementados por todos os temas. Assim, os módulos e a aplicação contribuem para o layout para compor uma UI de aplicação consistente.

### Temas Atuais

Atualmente, quatro temas são **oficialmente fornecidos**:

* O [Tema Básico](../../Basic-Theme.md) é o tema minimalista com o estilo Bootstrap simples. É **open source e gratuito**.
* O [Tema LeptonX Lite](../../Themes/LeptonXLite/AspNetCore.md) é um tema moderno e elegante de UI Bootstrap. É ideal se você deseja ter um tema de UI pronto para produção. Também é **open source e gratuito**.
* O [Tema Lepton](https://commercial.abp.io/themes) é um tema **comercial** desenvolvido pela equipe central do ABP e faz parte da licença [ABP Commercial](https://commercial.abp.io/).
* O [Tema LeptonX](https://docs.abp.io/en/commercial/latest/themes/lepton-x/index) também é um tema **comercial** desenvolvido pela equipe central do ABP e faz parte da licença [ABP Commercial](https://commercial.abp.io/). Este é o tema padrão após o ABP v6.0.0.

Também existem alguns temas desenvolvidos pela comunidade para o ABP Framework (você pode pesquisar na web).

## Geral

### As Bibliotecas Base

Todos os temas devem depender do pacote NPM [@abp/aspnetcore.mvc.ui.theme.shared](https://www.npmjs.com/package/@abp/aspnetcore.mvc.ui.theme.shared), para que dependam indiretamente das seguintes bibliotecas:

* [Twitter Bootstrap](https://getbootstrap.com/) como o framework HTML/CSS fundamental.
* [JQuery](https://jquery.com/) para manipulação do DOM.
* [DataTables.Net](https://datatables.net/) para grades de dados.
* [JQuery Validation](https://github.com/jquery-validation/jquery-validation) para validação do lado do cliente e [unobtrusive](https://github.com/aspnet/jquery-validation-unobtrusive) validation
* [FontAwesome](https://fontawesome.com/) como a biblioteca fundamental de fontes CSS.
* [SweetAlert](https://sweetalert.js.org/) para exibir mensagens de alerta e caixas de diálogo de confirmação.
* [Toastr](https://github.com/CodeSeven/toastr) para exibir notificações de toast.
* [Lodash](https://lodash.com/) como uma biblioteca de utilitários.
* [Luxon](https://moment.github.io/luxon/) para operações de data/hora.
* [JQuery Form](https://github.com/jquery-form/form) para formulários AJAX.
* [bootstrap-datepicker](https://github.com/uxsolutions/bootstrap-datepicker) para exibir seletores de data.
* [Select2](https://select2.org/) para caixas de seleção/combo melhores.
* [Timeago](http://timeago.yarp.com/) para exibir carimbos de data/hora fuzzy atualizados automaticamente.
* [malihu-custom-scrollbar-plugin](https://github.com/malihu/malihu-custom-scrollbar-plugin) para barras de rolagem personalizadas.

Essas bibliotecas são selecionadas como as bibliotecas base e estão disponíveis para as aplicações e módulos.

#### Abstrações / Wrappers

Existem algumas abstrações no ABP Framework para tornar seu código independente de algumas dessas bibliotecas também. Exemplos:

* [Tag Helpers](../../Tag-Helpers/Index.md) facilitam a geração de UIs do Bootstrap.
* As APIs JavaScript [Message](../../JavaScript-API/Message.md) e [Notification](../../JavaScript-API/Notify.md) fornecem abstrações para usar o Sweetalert e o Toastr.
* O sistema de [Forms & Validation](../../Forms-Validation.md) manipula automaticamente a validação, então você geralmente não digita diretamente nenhum código de validação.

### Os Layouts Padrão

A principal responsabilidade de um tema é fornecer os layouts. Existem **três layouts predefinidos que devem ser implementados por todos os temas**:

* **Application**: O layout padrão usado pelas páginas principais do aplicativo.
* **Account**: Geralmente usado pelo [módulo de conta](../../Modules/Account.md) para páginas de login, registro, esqueci minha senha...
* **Empty**: O layout mínimo que não possui componentes de layout.

Os nomes dos layouts são constantes definidas na classe `Volo.Abp.AspNetCore.Mvc.UI.Theming.StandardLayouts`.

#### O Layout de Aplicativo

Este é o layout padrão usado pelas páginas principais do aplicativo. A imagem a seguir mostra a página de gerenciamento de usuários no layout de aplicativo do [Tema Básico](../../Basic-Theme.md):

![basic-theme-application-layout](../../images/basic-theme-application-layout.png)

E a mesma página é mostrada abaixo com o layout de aplicativo do [Tema Lepton](https://commercial.abp.io/themes):

![lepton-theme-application-layout](../../images/lepton-theme-application-layout.png)

Como você pode ver, a página é a mesma, mas a aparência é completamente diferente nos temas acima.

O layout de aplicativo normalmente inclui as seguintes partes:

* Um [menu principal](../../Navigation-Menu.md)
* Uma [barra de ferramentas](../../Toolbars.md) principal com os seguintes componentes:
  * Menu do usuário
  * Dropdown de troca de idioma
* [Alertas de página](../../Page-Alerts.md)
* O conteúdo da página (chamado de `RenderBody()`)
* [Hooks de layout](../../Layout-Hooks.md)

Alguns temas podem fornecer mais partes, como breadcrumbs, cabeçalho e barra de ferramentas da página... etc. Veja a seção *Partes do Layout*.

#### O Layout de Conta

O layout de conta é geralmente usado pelo [módulo de conta](../../Modules/Account.md) para páginas de login, registro, esqueci minha senha...

![basic-theme-account-layout](../../images/basic-theme-account-layout.png)

Este layout normalmente fornece as seguintes partes:

* Dropdown de troca de idioma
* Área de troca de locatário (se a aplicação for [multi-tenant](../../Multi-Tenancy.md) e o locatário atual for resolvido pelo cookie)
* [Alertas de página](../../Page-Alerts.md)
* O conteúdo da página (chamado de `RenderBody()`)
* [Hooks de layout](../../Layout-Hooks.md)

O [Tema Básico](../../Basic-Theme.md) também renderiza a barra de navegação superior para este layout (como mostrado acima).

Aqui está o layout de conta do Tema Lepton:

![lepton-theme-account-layout](../../images/lepton-theme-account-layout.png)

O [Tema Lepton](https://commercial.abp.io/themes) mostra o logotipo do aplicativo e o rodapé neste layout.

> Você pode substituir completamente ou parcialmente os layouts do tema em um aplicativo para [personalizá-lo](../../Customization-User-Interface.md).

#### O Layout Vazio

O layout vazio fornece uma página vazia. Normalmente inclui as seguintes partes:

* [Alertas de página](../../Page-Alerts.md)
* O conteúdo da página (chamado de `RenderBody()`)
* [Hooks de layout](../../Layout-Hooks.md)

## Implementando um Tema

### A Forma Mais Fácil

A forma mais fácil de criar um novo tema é adicionar o módulo [Código-fonte do Tema Básico](https://github.com/abpframework/abp/tree/dev/modules/basic-theme) com os códigos-fonte e personalizá-lo.

```bash
abp add-package Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic --with-source-code --add-to-solution-file
```

### A Interface ITheme

A interface `ITheme` é usada pelo ABP Framework para selecionar o layout para a página atual. Um tema deve implementar esta interface para fornecer o caminho do layout solicitado.

Esta é a implementação da `ITheme` do [Tema Básico](../../Basic-Theme.md).

````csharp
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
{
    [ThemeName(Name)]
    public class BasicTheme : ITheme, ITransientDependency
    {
        public const string Name = "Basic";

        public virtual string GetLayout(string name, bool fallbackToDefault = true)
        {
            switch (name)
            {
                case StandardLayouts.Application:
                    return "~/Themes/Basic/Layouts/Application.cshtml";
                case StandardLayouts.Account:
                    return "~/Themes/Basic/Layouts/Account.cshtml";
                case StandardLayouts.Empty:
                    return "~/Themes/Basic/Layouts/Empty.cshtml";
                default:
                    return fallbackToDefault
                        ? "~/Themes/Basic/Layouts/Application.cshtml"
                        : null;
            }
        }
    }
}
````

* O atributo `[ThemeName]` é obrigatório e um tema deve ter um nome único, `Basic` neste exemplo.
* O método `GetLayout` deve retornar um caminho se o layout solicitado (`name`) for fornecido pelo tema. *Os Layouts Padrão* devem ser implementados se o tema for destinado a ser usado por um aplicativo padrão. Ele pode implementar layouts adicionais.

Depois que o tema implementa a interface `ITheme`, ele deve adicionar o tema às `AbpThemingOptions` no método `ConfigureServices` do [módulo](../../Module-Development-Basics.md).

````csharp
Configure<AbpThemingOptions>(options =>
{
    options.Themes.Add<BasicTheme>();
});
````

#### O Serviço IThemeSelector

O ABP Framework permite usar vários temas juntos. É por isso que `options.Themes` é uma lista. O serviço `IThemeSelector` seleciona o tema em tempo de execução. O desenvolvedor da aplicação pode definir `AbpThemingOptions.DefaultThemeName` para definir o tema a ser usado ou substituir a implementação do serviço `IThemeSelector` (a implementação padrão é `DefaultThemeSelector`) para controlar completamente a seleção do tema em tempo de execução.

### Bundles

O [sistema de agrupamento](../../Bundling-Minification.md) fornece uma maneira padrão de importar arquivos de estilo e script nas páginas. Existem dois pacotes padrão definidos pelo ABP Framework:

* `StandardBundles.Styles.Global`: O pacote global que inclui os arquivos de estilo usados em todas as páginas. Normalmente, inclui os arquivos CSS das Bibliotecas Base.
* `StandardBundles.Scripts.Global`: O pacote global que inclui os arquivos de script usados em todas as páginas. Normalmente, inclui os arquivos JavaScript das Bibliotecas Base.

Um tema geralmente estende esses pacotes padrão adicionando arquivos CSS/JavaScript específicos do tema.

A melhor maneira de definir novos pacotes é herdar dos pacotes padrão e adicioná-los às `AbpBundlingOptions`, como mostrado abaixo (este código é do [Tema Básico](../../Basic-Theme.md)):

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options
        .StyleBundles
        .Add(BasicThemeBundles.Styles.Global, bundle =>
        {
            bundle
                .AddBaseBundles(StandardBundles.Styles.Global)
                .AddContributors(typeof(BasicThemeGlobalStyleContributor));
        });

    options
        .ScriptBundles
        .Add(BasicThemeBundles.Scripts.Global, bundle =>
        {
            bundle
                .AddBaseBundles(StandardBundles.Scripts.Global)
                .AddContributors(typeof(BasicThemeGlobalScriptContributor));
        });
});
````

`BasicThemeGlobalStyleContributor` e `BasicThemeGlobalScriptContributor` são contribuidores de pacotes. Por exemplo, `BasicThemeGlobalStyleContributor` é definido como mostrado abaixo:

```csharp
public class BasicThemeGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/themes/basic/layout.css");
    }
}
```

Em seguida, o tema pode renderizar esses pacotes em um layout. Por exemplo, você pode renderizar os Estilos Globais como mostrado abaixo:

````html
<abp-style-bundle name="@BasicThemeBundles.Styles.Global" />
````

Consulte o documento [Bundle & Minification](../../Bundling-Minification.md) para entender melhor o sistema de agrupamento.

### Partes do Layout

Um layout típico consiste em várias partes. O tema deve incluir as partes necessárias em cada layout.

**Exemplo: O Tema Básico tem as seguintes partes para o Layout de Aplicativo**

![basic-theme-application-layout-parts](../../images/basic-theme-application-layout-parts.png)

O código do aplicativo e dos módulos só pode mostrar conteúdo na parte de Conteúdo da Página. Se eles precisarem alterar as outras partes (adicionar um item de menu, adicionar um item de barra de ferramentas, alterar o nome do aplicativo na área de marcação...), eles devem usar as APIs do ABP Framework.

As seções a seguir explicam as partes fundamentais pré-definidas pelo ABP Framework e que podem ser implementadas pelos temas.

> É uma boa prática dividir o layout em componentes/partials, para que o aplicativo final possa substituí-los parcialmente para fins de personalização.

#### Marcação

O serviço `IBrandingProvider` deve ser usado para obter o nome e a URL do logotipo do aplicativo para renderizar na parte de Marcação.

O [Modelo de Inicialização do Aplicativo](../../Startup-Templates/Application.md) tem uma implementação dessa interface para definir os valores pelo desenvolvedor da aplicação.

#### Menu Principal

O serviço `IMenuManager` é usado para obter os itens do menu principal e renderizá-los no layout.

**Exemplo: Obter o Menu Principal para renderizar em um componente de visualização**

```csharp
public class MainNavbarMenuViewComponent : AbpViewComponent
{
    private readonly IMenuManager _menuManager;

    public MainNavbarMenuViewComponent(IMenuManager menuManager)
    {
        _menuManager = menuManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menu = await _menuManager.GetAsync(StandardMenus.Main);
        return View("~/Themes/Basic/Components/Menu/Default.cshtml", menu);
    }
}
```

Consulte o documento [Navegação / Menus](../../Navigation-Menu.md) para saber mais sobre o sistema de navegação.

#### Barra de Ferramentas Principal

O serviço `IToolbarManager` é usado para obter os itens da Barra de Ferramentas Principal e renderizá-los no layout. Cada item desta barra de ferramentas é um componente de visualização, então ele pode incluir qualquer tipo de elemento de UI. Injete o `IToolbarManager` e use o `GetAsync` para obter os itens da barra de ferramentas:

````csharp
var toolbar = await _toolbarManager.GetAsync(StandardToolbars.Main);
````

> Consulte o documento [Barras de Ferramentas](../../Toolbars.md) para saber mais sobre o sistema de barras de ferramentas.

O tema tem a responsabilidade de adicionar dois itens pré-definidos à barra de ferramentas principal: Seleção de Idioma e Menu do Usuário. Para fazer isso, crie uma classe que implemente a interface `IToolbarContributor` e adicione-a às `AbpToolbarOptions`, como mostrado abaixo:

```csharp
Configure<AbpToolbarOptions>(options =>
{
    options.Contributors.Add(new BasicThemeMainTopToolbarContributor());
});
```

##### Seleção de Idioma

O item de barra de ferramentas Seleção de Idioma é geralmente um dropdown que é usado para alternar entre idiomas. O `ILanguageProvider` é usado para obter a lista de idiomas disponíveis e `CultureInfo.CurrentUICulture` é usado para saber o idioma atual.

O endpoint `/Abp/Languages/Switch` pode ser usado para alternar o idioma. Este endpoint aceita os seguintes parâmetros de string de consulta:

* `culture`: O idioma selecionado, como `en-US` ou `en`.
* `uiCulture`: O idioma de IU selecionado, como `en-US` ou `en`.
* `returnUrl` (opcional): Pode ser usado para retornar uma determinada URL após a troca de idioma.

`culture` e `uiCulture` devem corresponder a um dos idiomas disponíveis. O ABP Framework define um cookie de cultura no endpoint `/Abp/Languages/Switch`.

##### Menu do Usuário

O menu do usuário inclui links relacionados à conta do usuário. O `IMenuManager` é usado da mesma forma que o Menu Principal, mas desta vez com o parâmetro `StandardMenus.User`, como mostrado abaixo:

````csharp
var menu = await _menuManager.GetAsync(StandardMenus.User);
````

Os serviços [ICurrentUser](../../CurrentUser.md) e [ICurrentTenant](../../Multi-Tenancy.md) podem ser usados para obter os nomes do usuário e do locatário atual.

#### Alertas de Página

O serviço `IAlertManager` é usado para obter os alertas de página atuais para renderizar no layout. Use a lista `Alerts` do `IAlertManager`. Geralmente é renderizado logo antes do conteúdo da página (`RenderBody()`).

Consulte o documento [Alertas de Página](../../Page-Alerts.md) para saber mais.

#### Hooks de Layout

Como o Layout está no pacote do tema, o aplicativo final ou qualquer módulo não pode manipular diretamente o conteúdo do layout. O sistema de [Hooks de Layout](../../Layout-Hooks.md) permite injetar componentes em pontos específicos do layout.

O tema é responsável por renderizar os hooks no local correto.

**Exemplo: Renderizar o Hook `LayoutHooks.Head.First` no Layout de Aplicativo**

````html
<head>
    @await Component.InvokeLayoutHookAsync(LayoutHooks.Head.First, StandardLayouts.Application)
    ...
````

Consulte o documento [Hooks de Layout](../../Layout-Hooks.md) para saber mais sobre os hooks de layout padrão.

#### Seções de Script / Estilo

Todo layout deve renderizar as seguintes seções opcionais:

* A seção `styles` é renderizada no final do `head`, logo antes do `LayoutHooks.Head.Last`.
* A seção `scripts` é renderizada no final do `body`, logo antes do `LayoutHooks.Body.Last`.

Dessa forma, a página pode importar estilos e scripts para o layout.

**Exemplo: Renderizar a seção `styles`**

````csharp
@await RenderSectionAsync("styles", required: false)
````

#### Seção de Barra de Ferramentas de Conteúdo

Outra seção pré-definida é a seção de Barra de Ferramentas de Conteúdo, que pode ser usada pelas páginas para adicionar código logo antes do conteúdo da página. O Tema Básico a renderiza da seguinte forma:

````html
<div id="AbpContentToolbar">
    <div class="text-end mb-2">
        @RenderSection("content_toolbar", false)
    </div>
</div>
````

O id da div de contêiner deve ser `AbpContentToolbar`. Esta seção deve vir antes do `RenderBody()`.

#### Recursos de Widgets

O [Sistema de Widgets](../../Widgets.md) permite definir widgets reutilizáveis com seus próprios arquivos de estilo/script. Todos os layouts devem renderizar o estilo e os scripts do widget.

**Estilos de Widget** são renderizados da seguinte forma, logo antes da seção `styles`, após o pacote de estilo global:

````csharp
@await Component.InvokeAsync(typeof(WidgetStylesViewComponent))
````

**Scripts de Widget** são renderizados da seguinte forma, logo antes da seção `scripts`, após o pacote de script global:

````csharp
@await Component.InvokeAsync(typeof(WidgetScriptsViewComponent))
````

#### Scripts ABP

O ABP tem alguns scripts especiais que devem ser incluídos em todos os layouts. Eles não estão incluídos nos pacotes globais, pois são criados dinamicamente com base no usuário atual.

Os scripts do ABP (`ApplicationConfigurationScript` e `ServiceProxyScript`) devem ser adicionados logo após o pacote de script global, como mostrado abaixo:

````html
<script src="~/Abp/ApplicationConfigurationScript"></script>
<script src="~/Abp/ServiceProxyScript"></script>
````

#### Título da Página, Item de Menu Selecionado e Breadcrumbs

O serviço `IPageLayout` pode ser injetado por qualquer página para definir o título da página, o nome do item de menu selecionado e os itens de breadcrumbs. Em seguida, o tema pode usar este serviço para obter esses valores e renderizá-los na UI.

O Tema Básico não implementa este serviço, mas o Tema Lepton implementa:

![breadcrumbs-example](../../images/breadcrumbs-example.png)

Consulte o documento [Cabeçalho da Página](../../Page-Header.md) para saber mais.

#### Troca de Locatário

O Layout de Conta deve permitir ao usuário trocar o locatário atual se a aplicação for multi-tenant e o locatário for resolvido a partir dos cookies. Consulte o [Layout de Conta do Tema Básico](https://github.com/abpframework/abp/blob/dev/modules/basic-theme/src/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic/Themes/Basic/Layouts/Account.cshtml) como um exemplo de implementação.

### Classes de Layout

Os Layouts Padrão (`Application`, `Account` e `Empty`) devem adicionar as seguintes classes CSS à tag `body`:

* `abp-application-layout` para o layout `Application`.
* `abp-account-layout` para o layout `Account`.
* `abp-empty-layout` para o layout `Empty`.

Dessa forma, os aplicativos ou módulos podem ter seletores com base no layout atual.

### RTL

Para oferecer suporte a idiomas da direita para a esquerda, o Layout deve verificar a cultura atual e adicionar `dir="rtl"` à tag `html` e a classe CSS `rtl` à tag `body`.

Você pode verificar `CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft` para entender se o idioma atual é um idioma RTL.

### O Pacote NPM

Um tema deve ter um pacote NPM que dependa do pacote [@abp/aspnetcore.mvc.ui.theme.shared](https://www.npmjs.com/package/@abp/aspnetcore.mvc.ui.theme.shared). Dessa forma, ele herda todas as Bibliotecas Base. Se o tema exigir bibliotecas adicionais, ele deve definir essas dependências também.

As aplicações usam o sistema de [Gerenciamento de Pacotes do Lado do Cliente](../../Client-Side-Package-Management.md) para adicionar bibliotecas do lado do cliente ao projeto. Portanto, se uma aplicação usar seu tema, ela deve adicionar a dependência do pacote NPM do seu tema, bem como a dependência do pacote NuGet.

