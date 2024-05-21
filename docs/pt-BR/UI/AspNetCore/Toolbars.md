# ASP.NET Core MVC / Razor Pages UI: Barras de Ferramentas

O sistema de barras de ferramentas é usado para definir **barras de ferramentas** na interface do usuário. Módulos (ou sua aplicação) podem adicionar **itens** a uma barra de ferramentas e, em seguida, o [tema](Theming.md) renderiza a barra de ferramentas no **layout**.

Existe apenas uma **barra de ferramentas padrão** chamada "Principal" (definida como uma constante: `StandardToolbars.Main`). O [Tema Básico](Basic-Theme) renderiza a barra de ferramentas principal como mostrado abaixo:

![bookstore-toolbar-highlighted](../../images/bookstore-toolbar-highlighted.png)

Na captura de tela acima, existem dois itens adicionados à barra de ferramentas principal: o componente de alternância de idioma e o menu do usuário. Você pode adicionar seus próprios itens aqui.

Além disso, o [Tema LeptonX Lite](../../Themes/LeptonXLite/AspNetCore.md) possui duas barras de ferramentas diferentes para visualizações de desktop e móveis, definidas como constantes: `LeptonXLiteToolbars.Main`, `LeptonXLiteToolbars.MainMobile`.

| LeptonXLiteToolbars.Main | LeptonXLiteToolbars.MainMobile |
| :---: | :---: |
| ![leptonx](../../images/leptonxlite-toolbar-main-example.png) | ![leptonx](../../images/leptonxlite-toolbar-mainmobile-example.png) |

## Exemplo: Adicionar um Ícone de Notificação

Neste exemplo, vamos adicionar um **ícone de notificação (sino)** à esquerda do item de alternância de idioma. Um item na barra de ferramentas deve ser um **componente de visualização**. Então, primeiro, crie um novo componente de visualização em seu projeto:

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

Você pode usar a [autorização](../../Authorization.md) para decidir se deve adicionar um `ToolbarItem`.

````csharp
if (await context.IsGrantedAsync("NomeDaMinhaPermissao"))
{
    //...adicionar itens da barra de ferramentas
}
````

Você pode usar o método de extensão `RequirePermissions` como um atalho. Também é mais eficiente, o ABP otimiza a verificação de permissão para todos os itens.

````csharp
context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(NotificationViewComponent)).RequirePermissions("NomeDaMinhaPermissao"));
````

Essa classe adiciona o `NotificationViewComponent` como o primeiro item na barra de ferramentas `Main`.

Por fim, você precisa adicionar esse contribuidor às `AbpToolbarOptions`, no `ConfigureServices` do seu [módulo](../../Module-Development-Basics.md):

````csharp
Configure<AbpToolbarOptions>(options =>
{
    options.Contributors.Add(new MyToolbarContributor());
});
````

Isso é tudo, você verá o ícone de notificação na barra de ferramentas quando executar a aplicação:

![bookstore-notification-icon-on-toolbar](../../images/bookstore-notification-icon-on-toolbar.png)

`NotificationViewComponent` neste exemplo simplesmente retorna uma visualização sem nenhum dado. Na vida real, provavelmente você desejará **consultar o banco de dados** (ou chamar uma API HTTP) para obter notificações e passá-las para a visualização. Se necessário, você pode adicionar um arquivo `JavaScript` ou `CSS` ao [pacote](Bundling-Minification.md) global para o item da barra de ferramentas.

## IToolbarManager

`IToolbarManager` é usado para renderizar a barra de ferramentas. Ele retorna os itens da barra de ferramentas por um nome de barra de ferramentas. Isso é geralmente usado pelos [temas](Theming.md) para renderizar a barra de ferramentas no layout.