# ASP.NET Core MVC / Razor Pages UI: Menu de Navegação

Toda aplicação possui um menu principal que permite aos usuários navegar para páginas/telas da aplicação. Algumas aplicações podem conter mais de um menu em diferentes seções da interface do usuário.

O ABP Framework é um framework de desenvolvimento de aplicações [modular](../../Module-Development-Basics.md). **Cada módulo pode precisar adicionar itens ao menu**.

Portanto, o ABP Framework **fornece uma infraestrutura de menu** onde:

* A aplicação ou os módulos podem adicionar itens a um menu, sem saber como o menu é renderizado.
* O [tema](Theming.md) renderiza corretamente o menu.

## Adicionando Itens ao Menu

Para adicionar itens ao menu (ou manipular os itens existentes), você precisa criar uma classe que implemente a interface `IMenuContributor`.

> O [modelo de inicialização da aplicação](../../Startup-Templates/Application.md) já contém uma implementação do `IMenuContributor`. Portanto, você pode adicionar itens dentro dessa classe em vez de criar uma nova.

**Exemplo: Adicionar um item de menu *CRM* com subitens *Clientes* e *Pedidos***

```csharp
using System.Threading.Tasks;
using MyProject.Localization;
using Volo.Abp.UI.Navigation;

namespace MyProject.Web.Menus
{
    public class MyProjectMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<MyProjectResource>();

            context.Menu.AddItem(
                new ApplicationMenuItem("MyProject.Crm", l["Menu:CRM"])
                    .AddItem(new ApplicationMenuItem(
                        name: "MyProject.Crm.Customers", 
                        displayName: l["Menu:Customers"], 
                        url: "/crm/customers")
                    ).AddItem(new ApplicationMenuItem(
                        name: "MyProject.Crm.Orders", 
                        displayName: l["Menu:Orders"],
                        url: "/crm/orders")
                     )
            );
        }
    }
}
```

* Este exemplo adiciona itens apenas ao menu principal (`StandardMenus.Main`: veja a seção *Menus Padrão* abaixo).
* Ele obtém um `IStringLocalizer` do `context` para [localizar](../../Localization.md) os nomes de exibição dos itens do menu.
* Adiciona os Clientes e Pedidos como filhos do menu CRM.

Depois de criar um contribuidor de menu, você precisa adicioná-lo às `AbpNavigationOptions` no método `ConfigureServices` do seu módulo:

````csharp
Configure<AbpNavigationOptions>(options =>
{
    options.MenuContributors.Add(new MyProjectMenuContributor());
});
````

Este exemplo usa algumas chaves de localização como nomes de exibição, que devem ser definidas no arquivo de localização:

````json
"Menu:CRM": "CRM",
"Menu:Orders": "Pedidos",
"Menu:Customers": "Clientes"
````

Consulte o [documento de localização](../../Localization.md) para saber mais sobre a localização.

Quando você executar a aplicação, verá os itens do menu adicionados ao menu principal:

![nav-main-menu](../../images/nav-main-menu.png)

> O menu é renderizado pelo tema de interface do usuário atual. Portanto, a aparência do menu principal pode ser completamente diferente com base no seu tema.

Aqui estão algumas observações sobre os contribuidores de menu;

* O ABP Framework chama o método `ConfigureMenuAsync` **sempre que precisar renderizar** o menu.
* Cada item do menu pode ter **filhos**. Portanto, você pode adicionar itens de menu com **profundidade ilimitada** (no entanto, seu tema de interface do usuário pode não suportar profundidade ilimitada).
* Apenas os itens de menu folha normalmente têm `url`s. Quando você clica em um menu pai, seu submenu é aberto ou fechado, você não navega para a `url` de um item de menu pai.
* Se um item de menu não tiver filhos e não tiver uma `url` definida, ele não será renderizado na interface do usuário. Isso simplifica a autorização dos itens do menu: você só autoriza os itens filhos (veja a próxima seção). Se nenhum dos filhos for autorizado, o pai desaparece automaticamente.

### Propriedades do Item do Menu

Existem mais opções de um item do menu (o construtor da classe `ApplicationMenuItem`). Aqui está a lista de todas as opções disponíveis;

* `name` (`string`, obrigatório): O **nome único** do item do menu.
* `displayName` (`string`, obrigatório): Nome de exibição/texto do item do menu. Você pode [localizar](../../Localization.md) isso como mostrado anteriormente.
* `url` (`string`): A URL do item do menu.
* `icon` (`string`): Um nome de ícone. Classes de ícone gratuitas do [Font Awesome](https://fontawesome.com/) são suportadas por padrão. Exemplo: `fa fa-book`. Você pode usar qualquer classe de ícone de fonte CSS, desde que inclua os arquivos CSS necessários em sua aplicação.
* `order` (`int`): A ordem do item do menu. O valor padrão é `1000`. Os itens são classificados pela ordem de adição, a menos que você especifique um valor de ordem.
* `customData` (`Dictionary<string, object>`): Um dicionário que permite armazenar objetos personalizados que você pode associar ao item do menu e usá-lo ao renderizar o item do menu.
* `target` (`string`): Destino do item do menu. Pode ser `null` (padrão), "\_*blank*", "\_*self*", "\_*parent*", "\_*top*" ou um nome de frame para aplicações web.
* `elementId` (`string`): Pode ser usado para renderizar o elemento com um atributo HTML `id` específico.
* `cssClass` (`string`): Classes de string adicionais para o item do menu.
* `groupName` (`string`): Pode ser usado para agrupar itens do menu.

### Autorização

Como visto acima, um contribuidor de menu contribui para o menu dinamicamente. Portanto, você pode executar qualquer lógica personalizada ou obter itens do menu de qualquer fonte.

Um caso de uso é a [autorização](../../Authorization.md). Normalmente, você deseja adicionar itens de menu verificando uma permissão.

**Exemplo: Verificar se o usuário atual possui uma permissão**

````csharp
if (await context.IsGrantedAsync("NomeDaMinhaPermissao"))
{
    //...adicionar itens do menu
}
````

Para a autorização, você pode usar o método de extensão `RequirePermissions` como atalho. Ele também é mais eficiente, o ABP otimiza a verificação de permissão para todos os itens.

````csharp
context.Menu.AddItem(
    new ApplicationMenuItem("MyProject.Crm", l["Menu:CRM"])
        .AddItem(new ApplicationMenuItem(
                name: "MyProject.Crm.Customers",
                displayName: l["Menu:Customers"],
                url: "/crm/customers")
            .RequirePermissions("MyProject.Crm.Customers")
        ).AddItem(new ApplicationMenuItem(
                name: "MyProject.Crm.Orders",
                displayName: l["Menu:Orders"],
                url: "/crm/orders")
            .RequirePermissions("MyProject.Crm.Orders")
        )
);
````

> Você pode usar `context.AuthorizationService` para acessar diretamente o `IAuthorizationService`.

### Resolvendo Dependências

`context.ServiceProvider` pode ser usado para resolver qualquer dependência de serviço.

**Exemplo: Obter um serviço**

````csharp
var meuServico = context.ServiceProvider.GetRequiredService<IMeuServico>();
//...usar o serviço
````

> Você não precisa se preocupar em liberar/descartar serviços. O ABP Framework cuida disso.

### O Menu de Administração

Há um item de menu especial no menu que é adicionado pelo ABP Framework: O menu *Administração*. Ele é normalmente usado pelos módulos de administração pré-construídos [application modules](../../Modules/Index.md):

![nav-main-menu-administration](../../images/nav-main-menu-administration.png)

Se você deseja adicionar itens de menu sob o item de menu *Administração*, pode usar o método de extensão `context.Menu.GetAdministration()`:

````csharp
context.Menu.GetAdministration().AddItem(...)
````

### Manipulando os Itens do Menu Existente

O ABP Framework executa os contribuidores de menu pela [ordem de dependência do módulo](../../Module-Development-Basics.md). Portanto, você pode manipular os itens do menu nos quais sua aplicação ou módulo (direta ou indiretamente) depende.

**Exemplo: Definir um ícone para o item de menu `Usuários` adicionado pelo [Módulo de Identidade](../../Modules/Identity.md)**

````csharp
var menuUsuario = context.Menu.FindMenuItem(IdentityMenuNames.Users);
menuUsuario.Icon = "fa fa-users";
````

> `context.Menu` permite acessar todos os itens do menu que foram adicionados pelos contribuidores de menu anteriores.

### Grupos de Menu

Você pode definir grupos e associar itens de menu a um grupo.

Exemplo:

```csharp
using System.Threading.Tasks;
using MyProject.Localization;
using Volo.Abp.UI.Navigation;

namespace MyProject.Web.Menus
{
    public class MyProjectMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<MyProjectResource>();

            context.Menu.AddGroup(
                new ApplicationMenuGroup(
                    name: "Main",
                    displayName: l["Main"]
                )
            )
            context.Menu.AddItem(
                new ApplicationMenuItem("MyProject.Crm", l["Menu:CRM"], groupName: "Main")
                    .AddItem(new ApplicationMenuItem(
                        name: "MyProject.Crm.Customers", 
                        displayName: l["Menu:Customers"], 
                        url: "/crm/customers")
                    ).AddItem(new ApplicationMenuItem(
                        name: "MyProject.Crm.Orders", 
                        displayName: l["Menu:Orders"],
                        url: "/crm/orders")
                     )
            );      
        }
    }
}
```

> O tema de interface do usuário decidirá se renderizará ou não os grupos e, se decidir renderizar, a forma como é renderizado depende do tema. Apenas o tema LeptonX implementa o grupo de menu.

## Menus Padrão

Um menu é um componente **nomeado**. Uma aplicação pode conter mais de um menu com nomes diferentes e exclusivos. Existem dois menus padrão pré-definidos:

* `Main`: O menu principal da aplicação. Contém links para as páginas da aplicação. Definido como uma constante: `Volo.Abp.UI.Navigation.StandardMenus.Main`.
* `User`: Menu do perfil do usuário. Definido como uma constante: `Volo.Abp.UI.Navigation.StandardMenus.User`.

O menu `Main` já foi abordado acima. O menu `User` está disponível quando um usuário faz login:

![user-menu](../../images/user-menu.png)

Você pode adicionar itens ao menu `User` verificando o `context.Menu.Name` como mostrado abaixo:

```csharp
if (context.Menu.Name == StandardMenus.User)
{
    //...adicionar itens
}
```

## IMenuManager

O `IMenuManager` é geralmente usado pelo [tema](Theming.md) de interface do usuário para renderizar os itens do menu na interface do usuário. Portanto, **geralmente você não precisa usar diretamente** o `IMenuManager`.

**Exemplo: Obtendo os itens do menu `Main`**

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.UI.Navigation;

namespace MyProject.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMenuManager _menuManager;

        public IndexModel(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }
        
        public async Task OnGetAsync()
        {
            var mainMenu = await _menuManager.GetAsync(StandardMenus.Main);
            
            foreach (var menuItem in mainMenu.Items)
            {
                //...
            }
        }
    }
}
```