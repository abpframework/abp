# ASP.NET Core MVC / Razor Pages: Hooks de Layout

O sistema de temas do ABP Framework coloca o layout da página nos pacotes NuGet do [tema](Theming.md). Isso significa que a aplicação final não inclui um `Layout.cshtml`, portanto, você não pode alterar diretamente o código do layout para personalizá-lo.

Você copia o código do tema para a sua solução. Nesse caso, você tem total liberdade para personalizá-lo. No entanto, você não poderá obter atualizações automáticas do tema (atualizando o pacote NuGet do tema).

O ABP Framework oferece diferentes maneiras de [personalizar a interface do usuário](Customization-User-Interface.md).

O **Sistema de Hooks de Layout** permite que você **adicione código** em partes específicas do layout. Todos os layouts de todos os temas devem implementar esses hooks. Por fim, você pode adicionar um **componente de visualização** em um ponto de hook.

## Exemplo: Adicionar Script do Google Analytics

Suponha que você precise adicionar o script do Google Analytics ao layout (que estará disponível para todas as páginas). Primeiro, **crie um componente de visualização** em seu projeto:

![bookstore-google-analytics-view-component](../../images/bookstore-google-analytics-view-component.png)

**NotificationViewComponent.cs**

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

Altere `UA-xxxxxx-1` pelo seu próprio código.

Em seguida, você pode adicionar esse componente a qualquer um dos pontos de hook no `ConfigureServices` do seu módulo:

````csharp
Configure<AbpLayoutHookOptions>(options =>
{
    options.Add(
        LayoutHooks.Head.Last, //O nome do hook
        typeof(GoogleAnalyticsViewComponent) //O componente a ser adicionado
    );
});
````

Agora, o código do GA será inserido no `head` da página como o último item.

### Especificando o Layout

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

Consulte a seção *Layouts* abaixo para saber mais sobre o sistema de layout.

## Pontos de Hook de Layout

Existem alguns pontos de hook de layout predefinidos. O `LayoutHooks.Head.Last` usado acima foi um deles. Os pontos de hook padrão são:

* `LayoutHooks.Head.First`: Usado para adicionar um componente como o primeiro item na tag HTML head.
* `LayoutHooks.Head.Last`: Usado para adicionar um componente como o último item na tag HTML head.
* `LayoutHooks.Body.First`: Usado para adicionar um componente como o primeiro item na tag HTML body.
* `LayoutHooks.Body.Last`: Usado para adicionar um componente como o último item na tag HTML body.
* `LayoutHooks.PageContent.First`: Usado para adicionar um componente logo antes do conteúdo da página (o `@RenderBody()` no layout).
* `LayoutHooks.PageContent.Last`: Usado para adicionar um componente logo após o conteúdo da página (o `@RenderBody()` no layout).

> Você (ou os módulos que você está usando) pode adicionar **vários itens ao mesmo ponto de hook**. Todos eles serão adicionados ao layout na ordem em que foram adicionados.

## Layouts

O sistema de layout permite que os temas definam layouts padrão e nomeados e permite que qualquer página selecione um layout adequado para o seu propósito. Existem três layouts predefinidos:

* "**Application**": O layout principal (e padrão) para uma aplicação. Normalmente, contém cabeçalho, menu (barra lateral), rodapé, barra de ferramentas, etc.
* "**Account**": Este layout é usado pelo login, registro e outras páginas semelhantes. É usado para as páginas na pasta `/Pages/Account` por padrão.
* "**Empty**": Layout vazio e mínimo.

Esses nomes são definidos na classe `StandardLayouts` como constantes. Você pode criar seus próprios layouts, mas esses são os nomes de layout padrão e implementados por todos os temas por padrão.

### Localização do Layout

Você pode encontrar os arquivos de layout [aqui](https://github.com/abpframework/abp/blob/dev/modules/basic-theme/src/Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic/Themes/Basic/Layouts) para o tema básico. Você pode usá-los como referência para construir seus próprios layouts ou pode substituí-los, se necessário.

## Veja também

* [Personalizando a Interface do Usuário](Customization-User-Interface.md)