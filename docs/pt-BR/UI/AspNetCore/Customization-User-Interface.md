# Guia de Customização da Interface do Usuário do ASP.NET Core (MVC / Razor Pages)

Este documento explica como substituir a interface do usuário de um módulo de aplicativo dependente [módulo de aplicativo](../../Modules/Index.md) ou [tema](Theming.md) para aplicativos ASP.NET Core MVC / Razor Page.

## Substituindo uma Página

Esta seção aborda o desenvolvimento de [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/), que é a abordagem recomendada para criar interfaces de usuário renderizadas no servidor para o ASP.NET Core. Os módulos pré-construídos geralmente usam a abordagem Razor Pages em vez do padrão clássico MVC (as próximas seções também abordarão o padrão MVC).

Normalmente, você tem três tipos de requisitos de substituição para uma página:

* Substituir apenas o lado do **Modelo de Página** (C#) para executar lógica adicional sem alterar a interface do usuário da página.
* Substituir apenas a **Página Razor** (arquivo .chtml) para alterar a interface do usuário sem alterar o código C# por trás da página.
* **Substituir completamente** a página.

### Substituindo um Modelo de Página (C#)

````csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity.Users;

namespace Acme.BookStore.Web.Pages.Identity.Users
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(EditModalModel))]
    public class MyEditModalModel : EditModalModel
    {
        public MyEditModalModel(
            IIdentityUserAppService identityUserAppService,
            IIdentityRoleAppService identityRoleAppService
            ) : base(
                identityUserAppService,
                identityRoleAppService)
        {
        }

        public async override Task<IActionResult> OnPostAsync()
        {
            //TODO: Lógica adicional
            await base.OnPostAsync();
            //TODO: Lógica adicional
        }
    }
}
````

* Esta classe herda e substitui o `EditModalModel` para os usuários e substitui o método `OnPostAsync` para executar lógica adicional antes e depois do código subjacente.
* Ele usa os atributos `ExposeServices` e `Dependency` para substituir a classe.

### Substituindo uma Página Razor (.CSHTML)

Substituir um arquivo `.cshtml` (página razor, visualização razor, componente de visualização... etc.) é possível criando o mesmo arquivo `.cshtml` no mesmo caminho.

#### Exemplo

Este exemplo substitui a interface do usuário da **página de login** definida pelo [Módulo de Conta](../../Modules/Account.md).

O módulo de conta define um arquivo `Login.cshtml` na pasta `Pages/Account`. Portanto, você pode substituí-lo criando um arquivo no mesmo caminho:

![substituindo-login-cshtml](../../images/overriding-login-cshtml.png)

Normalmente, você deseja copiar o arquivo `.cshtml` original do módulo e fazer as alterações necessárias. Você pode encontrar o arquivo original [aqui](https://github.com/abpframework/abp/blob/dev/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml). Não copie o arquivo `Login.cshtml.cs`, que é o arquivo de código por trás da página razor e não queremos substituí-lo ainda (veja a próxima seção).

> Não se esqueça de adicionar [_ViewImports.cshtml](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/layout?view=aspnetcore-7.0#importing-shared-directives) se a página que você deseja substituir contiver [ABP Tag Helpers](../AspNetCore/Tag-Helpers/Index.md).

````csharp
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

Isso é tudo, você pode alterar o conteúdo do arquivo como desejar.

### Substituindo Completamente uma Página Razor

Você pode querer substituir completamente uma página; a página razor e o arquivo C# relacionado à página.

Nesse caso;

1. Substitua a classe do modelo de página C# como descrito acima, mas não substitua a classe de modelo de página existente.
2. Substitua a Página Razor conforme descrito acima, mas também altere a diretiva @model para apontar para o novo modelo de página.

#### Exemplo

Este exemplo substitui a **página de login** definida pelo [Módulo de Conta](../../Modules/Account.md).

Crie uma classe de modelo de página derivada de `LoginModel` (definida no namespace `Volo.Abp.Account.Web.Pages.Account`):

````csharp
public class MyLoginModel : LoginModel
{
    public MyLoginModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions
        ) : base(
        schemeProvider,
        accountOptions)
    {

    }

    public override Task<IActionResult> OnPostAsync(string action)
    {
        //TODO: Adicionar lógica
        return base.OnPostAsync(action);
    }

    //TODO: Adicionar novos métodos e propriedades...
}
````

Você pode substituir qualquer método ou adicionar novas propriedades/métodos, se necessário.

> Observe que não usamos `[Dependency(ReplaceServices = true)]` ou `[ExposeServices(typeof(LoginModel))]` porque não queremos substituir a classe existente na injeção de dependência, definimos uma nova.

Copie o arquivo `Login.cshtml` para a sua solução conforme descrito acima. Altere a diretiva **@model** para apontar para o `MyLoginModel`:

````xml
@page
...
@model Acme.BookStore.Web.Pages.Account.MyLoginModel
...
````

Isso é tudo! Faça qualquer alteração na visualização e execute seu aplicativo.

#### Substituindo o Modelo de Página Sem Herança

Você não precisa herdar da classe de modelo de página original (como feito no exemplo anterior). Em vez disso, você pode **reimplementar completamente** a página você mesmo. Nesse caso, basta derivar de `PageModel`, `AbpPageModel` ou qualquer classe base adequada que você precise.

## Substituindo um Componente de Visualização

O ABP Framework, temas pré-construídos e módulos definem alguns **componentes de visualização reutilizáveis**. Esses componentes de visualização podem ser substituídos da mesma forma que uma página descrita acima.

### Exemplo

A captura de tela abaixo foi tirada do [Tema Básico](Basic-Theme.md) fornecido com o modelo de inicialização do aplicativo.

![bookstore-brand-area-highlighted](../../images/bookstore-brand-area-highlighted.png)

O [Tema Básico](Basic-Theme.md) define alguns componentes de visualização para o layout. Por exemplo, a área destacada com o retângulo vermelho acima é chamada de **componente de marca**. Você provavelmente deseja personalizar esse componente adicionando seu **próprio logotipo do aplicativo**. Vamos ver como fazer isso.

Primeiro, crie seu logotipo e coloque-o em uma pasta em seu aplicativo da web. Usamos o caminho `wwwroot/logos/bookstore-logo.png`. Em seguida, copie a visualização do componente de marca ([aqui](https://github.com/abpframework/abp/blob/dev/modules/basic-theme/src/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic/Themes/Basic/Components/Brand/Default.cshtml)) dos arquivos do tema básico na pasta `Themes/Basic/Components/Brand`. O resultado deve ser semelhante à imagem abaixo:

![bookstore-added-brand-files](../../images/bookstore-added-brand-files.png)

Em seguida, altere o `Default.cshtml` como desejar. O conteúdo de exemplo pode ser assim:

````xml
<a href="/">
    <img src="~/logos/bookstore-logo.png" width="250" height="60"/>
</a>
````

Agora, você pode executar o aplicativo para ver o resultado:

![bookstore-added-logo](../../images/bookstore-added-logo.png)

Se necessário, você também pode substituir [o arquivo c# de código por trás](https://github.com/abpframework/abp/blob/dev/modules/basic-theme/src/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic/Themes/Basic/Components/Brand/MainNavbarBrandViewComponent.cs) do componente usando o sistema de injeção de dependência.

### Substituindo o Tema

Assim como explicado acima, você pode substituir qualquer componente, layout ou classe c# do tema usado. Consulte o documento [tematização](Theming.md) para obter mais informações sobre o sistema de tematização.

## Substituindo Recursos Estáticos

Substituir um recurso estático incorporado (como arquivos JavaScript, CSS ou de imagem) de um módulo é bastante fácil. Basta colocar um arquivo no mesmo caminho em sua solução e deixar o [Sistema de Arquivos Virtual](../../Virtual-File-System.md) lidar com ele.

## Manipulando os Pacotes

O sistema de [Empacotamento e Minificação](Bundling-Minification.md) fornece um sistema **extensível e dinâmico** para criar pacotes de **scripts** e **estilos**. Ele permite que você estenda e manipule os pacotes existentes.

### Exemplo: Adicionar um Arquivo CSS Global

Por exemplo, o ABP Framework define um **pacote de estilo global** que é adicionado a todas as páginas (na verdade, adicionado ao layout pelos temas). Vamos adicionar um **arquivo de estilo personalizado** ao final dos arquivos do pacote, para que possamos substituir qualquer estilo global.

Primeiro, crie um arquivo CSS e coloque-o em uma pasta dentro do `wwwroot`:

![bookstore-global-css-file](../../images/bookstore-global-css-file.png)

Defina algumas regras CSS personalizadas dentro do arquivo. Exemplo:

````css
.card-title {
    color: orange;
    font-size: 2em;
    text-decoration: underline;
}

.btn-primary {
    background-color: red;
}
````

Em seguida, adicione este arquivo ao pacote global de estilo padrão no método `ConfigureServices` do seu [módulo](../../Module-Development-Basics.md):

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.StyleBundles.Configure(
        StandardBundles.Styles.Global, //O nome do pacote!
        bundleConfiguration =>
        {
            bundleConfiguration.AddFiles("/styles/my-global-styles.css");
        }
    );
});
````

#### O Pacote de Script Global

Assim como o `StandardBundles.Styles.Global`, há um `StandardBundles.Scripts.Global` que você pode adicionar arquivos ou manipular os existentes.

### Exemplo: Manipular os Arquivos do Pacote

O exemplo acima adiciona um novo arquivo ao pacote. Você pode fazer mais se criar uma classe de **contribuinte de pacote**. Exemplo:

````csharp
public class MyGlobalStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Clear();
        context.Files.Add("/styles/my-global-styles.css");
    }
}
````

Em seguida, você pode adicionar o contribuinte a um pacote existente:

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.StyleBundles.Configure(
        StandardBundles.Styles.Global,
        bundleConfiguration =>
        {
            bundleConfiguration.AddContributors(typeof(MyGlobalStyleBundleContributor));
        }
    );
});
````

Não é uma boa ideia limpar todos os arquivos CSS. Em um cenário do mundo real, você pode encontrar e substituir um arquivo específico pelo seu próprio arquivo.

### Exemplo: Adicionar um Arquivo JavaScript para uma Página Específica

Os exemplos acima funcionam com o pacote global adicionado ao layout. E se você quiser adicionar um arquivo CSS/JavaScript (ou substituir um arquivo) para uma página específica definida em um módulo dependente?

Suponha que você queira executar um código **JavaScript** assim que o usuário entrar na página de **Gerenciamento de Funções** do Módulo de Identidade.

Primeiro, crie um arquivo JavaScript padrão no `wwwroot`, `Pages` ou `Views` (o ABP suporta adicionar recursos estáticos dentro dessas pastas por padrão). Preferimos a pasta `Pages/Identity/Roles` para seguir as convenções:

![bookstore-added-role-js-file](../../images/bookstore-added-role-js-file.png)

O conteúdo do arquivo é simples:

````js
$(function() {
    abp.log.info('Meu arquivo de script de função personalizado foi carregado!');
});
````

Em seguida, adicione este arquivo ao pacote da página de gerenciamento de funções:

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.ScriptBundles
        .Configure(
            typeof(Volo.Abp.Identity.Web.Pages.Identity.Roles.IndexModel).FullName,
            bundleConfig =>
            {
                bundleConfig.AddFiles("/Pages/Identity/Roles/my-role-script.js");
            });
});
````

`typeof(Volo.Abp.Identity.Web.Pages.Identity.Roles.IndexModel).FullName` é a maneira segura de obter o nome do pacote para a página de gerenciamento de funções.

> Observe que nem todas as páginas definem esses pacotes de página. Eles definem apenas se necessário.

Além de adicionar um novo arquivo CSS/JavaScript a uma página, você também pode substituir o existente (definindo um contribuinte de pacote).

## Customização do Layout

Os layouts são definidos pelo tema ([consulte a documentação de tematização](Theming.md)) por design. Eles não estão incluídos em uma solução de aplicativo baixada. Dessa forma, você pode facilmente **atualizar** o tema e obter novos recursos. Você não pode **alterar diretamente** o código do layout em seu aplicativo, a menos que o substitua por seu próprio layout (será explicado nas próximas seções).

Existem algumas maneiras comuns de **personalizar o layout** descritas nas seções a seguir.

### Contribuintes de Menu

Existem dois **menus padrão** definidos pelo ABP Framework:

![bookstore-menus-highlighted](../../images/bookstore-menus-highlighted.png)

* `StandardMenus.Main`: O menu principal do aplicativo.
* `StandardMenus.User`: O menu do usuário (geralmente no canto superior direito da tela).

A renderização dos menus é de responsabilidade do tema, mas os **itens do menu** são determinados pelos módulos e pelo código do seu aplicativo. Basta implementar a interface `IMenuContributor` e **manipular os itens do menu** no método `ConfigureMenuAsync`.

Os contribuintes de menu são executados sempre que precisam renderizar o menu. Já existe um contribuinte de menu definido no **modelo de inicialização do aplicativo**, para que você possa usá-lo como exemplo e melhorar, se necessário. Consulte o documento [menu de navegação](Navigation-Menu.md) para obter mais informações.

### Contribuintes de Barra de Ferramentas

O sistema de [barra de ferramentas](Toolbars.md) é usado para definir **barras de ferramentas** na interface do usuário. Os módulos (ou seu aplicativo) podem adicionar **itens** a uma barra de ferramentas e, em seguida, o tema renderiza a barra de ferramentas no **layout**.

Existe apenas uma **barra de ferramentas padrão** (chamada "Principal" - definida como uma constante: `StandardToolbars.Main`). Para o tema básico, ele é renderizado como mostrado abaixo:![bookstore-toolbar-highlighted](../../images/bookstore-toolbar-highlighted.png)

Na captura de tela acima, existem dois itens adicionados à barra de ferramentas principal: componente de troca de idioma e menu do usuário. Você pode adicionar seus próprios itens aqui.

#### Exemplo: Adicionar um Ícone de Notificação

Neste exemplo, adicionaremos um **ícone de notificação (sino)** à esquerda do item de troca de idioma. Um item na barra de ferramentas deve ser um **componente de visualização**. Portanto, primeiro, crie um novo componente de visualização em seu projeto:

![bookstore-notification-view-component](../../images/bookstore-notification-view-component.png)

**NotificationViewComponent.cs**

````csharp
public class NotificationViewComponent : AbpViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("/Pages/Shared/Components/Notification/Default.cshtml");
    }
}
````

**Default.cshtml**

````xml
<div id="MainNotificationIcon" style="color: white; margin: 8px;">
    <i class="far fa-bell"></i>
</div>
````

Agora, podemos criar uma classe que implementa a interface `IToolbarContributor`:

````csharp
public class MyToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items
                .Insert(0, new ToolbarItem(typeof(NotificationViewComponent)));
        }

        return Task.CompletedTask;
    }
}
````

Esta classe adiciona o `NotificationViewComponent` como o primeiro item na barra de ferramentas `Main`.

Finalmente, você precisa adicionar este contribuinte ao `AbpToolbarOptions`, no `ConfigureServices` do seu módulo:

````csharp
Configure<AbpToolbarOptions>(options =>
{
    options.Contributors.Add(new MyToolbarContributor());
});
````

Isso é tudo, você verá o ícone de notificação na barra de ferramentas quando executar o aplicativo:

![bookstore-notification-icon-on-toolbar](../../images/bookstore-notification-icon-on-toolbar.png)

O `NotificationViewComponent` neste exemplo simplesmente retorna uma visualização sem nenhum dado. Na vida real, você provavelmente desejará **consultar o banco de dados** (ou chamar uma API HTTP) para obter notificações e passá-las para a visualização. Se necessário, você pode adicionar um arquivo `JavaScript` ou `CSS` ao pacote global (como descrito anteriormente) para o item da barra de ferramentas.

Consulte o documento [barras de ferramentas](Toolbars.md) para obter mais informações sobre o sistema de barras de ferramentas.

### Hooks de Layout

O sistema de [Hooks de Layout](Layout-Hooks.md) permite que você **adicione código** em algumas partes específicas do layout. Todos os layouts de todos os temas devem implementar esses hooks. Em seguida, você pode adicionar um **componente de visualização** em um ponto de hook.

#### Exemplo: Adicionar Script do Google Analytics

Suponha que você precise adicionar o script do Google Analytics ao layout (que estará disponível para todas as páginas). Primeiro, **crie um componente de visualização** em seu projeto:

![bookstore-google-analytics-view-component](../../images/bookstore-google-analytics-view-component.png)

**GoogleAnalyticsViewComponent.cs**

````csharp
public class GoogleAnalyticsViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("/Pages/Shared/Components/GoogleAnalytics/Default.cshtml");
    }
}
````

**Default.cshtml**

````html
<script>
    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
            (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
            m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
    })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

    ga('create', 'UA-xxxxxx-1', 'auto');
    ga('send', 'pageview');
</script>
````

Altere `UA-xxxxxx-1` para o seu próprio código.

Você pode então adicionar este componente a qualquer um dos pontos de hook no `ConfigureServices` do seu módulo:

````csharp
Configure<AbpLayoutHookOptions>(options =>
{
    options.Add(
        LayoutHooks.Head.Last, //O nome do hook
        typeof(GoogleAnalyticsViewComponent) //O componente a ser adicionado
    );
});
````

Agora, o código do GA será inserido no `head` da página como o último item. Você (ou os módulos que você está usando) pode adicionar vários itens ao mesmo hook. Todos eles serão adicionados ao layout.

A configuração acima adiciona o `GoogleAnalyticsViewComponent` a todos os layouts. Talvez você queira adicionar apenas a um layout específico:

````csharp
Configure<AbpLayoutHookOptions>(options =>
{
    options.Add(
        LayoutHooks.Head.Last,
        typeof(GoogleAnalyticsViewComponent),
        layout: StandardLayouts.Application //Defina o layout a ser adicionado
    );
});
````

Consulte a seção de layouts abaixo para saber mais sobre o sistema de layout.

### Layouts

O sistema de layout permite que os temas definam layouts padrão e nomeados e permite que qualquer página selecione um layout adequado para seu propósito. Existem três layouts predefinidos:

* "**Application**": O layout principal (e o padrão) para um aplicativo. Normalmente, contém cabeçalho, menu (barra lateral), rodapé, barra de ferramentas... etc.
* "**Account**": Este layout é usado para login, registro e outras páginas semelhantes. É usado para as páginas na pasta `/Pages/Account` por padrão.
* "**Empty**": Layout vazio e mínimo.

Esses nomes são definidos na classe `StandardLayouts` como constantes. Você pode definitivamente criar seus próprios layouts, mas esses são os nomes padrão dos layouts e são implementados por todos os temas por padrão.

#### Localização do Layout

Você pode encontrar os arquivos de layout [aqui](https://github.com/abpframework/abp/blob/dev/modules/basic-theme/src/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic/Themes/Basic/Layouts) para o tema básico. Você pode usá-los como referência para criar seus próprios layouts ou pode substituí-los, se necessário.

#### ITheme

O ABP Framework usa o serviço `ITheme` para obter a localização do layout pelo nome do layout. Você pode substituir este serviço para selecionar dinamicamente a localização do layout.

#### IThemeManager

`IThemeManager` é usado para obter o tema atual e obter o caminho do layout. Qualquer página pode determinar seu próprio layout. Exemplo:

````html
@using Volo.Abp.AspNetCore.Mvc.UI.Theming
@inject IThemeManager ThemeManager
@{
    Layout = ThemeManager.CurrentTheme.GetLayout(StandardLayouts.Empty);
}
````

Esta página usará o layout vazio. Você usa o método de extensão `ThemeManager.CurrentTheme.GetEmptyLayout();` como atalho.

Se você deseja definir o layout para todas as páginas em uma pasta específica, escreva o código acima em um arquivo `_ViewStart.cshtml` dentro dessa pasta.