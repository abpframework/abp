# Extensões da Barra de Ferramentas da Página para a Interface do Usuário ASP.NET Core

O sistema de barra de ferramentas da página permite adicionar componentes à barra de ferramentas de qualquer página. A barra de ferramentas da página é a área à direita do cabeçalho de uma página. Um botão ("Importar usuários do Excel") foi adicionado à página de gerenciamento de usuários abaixo:

![botão-barra-ferramentas-página](../../images/page-toolbar-button.png)

Você pode adicionar qualquer tipo de componente de visualização à barra de ferramentas da página ou modificar os itens existentes.

## Como Configurar

Neste exemplo, adicionaremos um botão "Importar usuários do Excel" e executaremos um código JavaScript para a página de gerenciamento de usuários do [Módulo de Identidade](../../Modules/Identity.md).

### Adicionar um Novo Botão à Página de Gerenciamento de Usuários

Escreva o seguinte código dentro do método `ConfigureServices` da classe do seu módulo web:

````csharp
Configure<AbpPageToolbarOptions>(options =>
{
    options.Configure<Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel>(toolbar =>
    {
        toolbar.AddButton(
            LocalizableString.Create<MyProjectNameResource>("ImportFromExcel"),
            icon: "file-import",
            id: "ImportUsersFromExcel",
            type: AbpButtonType.Secondary
        );
    });
});
````

`AddButton` é um atalho para simplesmente adicionar um componente de botão. Observe que você precisa adicionar a chave `ImportFromExcel` ao seu dicionário de localização (arquivo json) para localizar o texto.

Quando você executar a aplicação, verá o botão adicionado ao lado da lista de botões atual. Existem outros parâmetros do método `AddButton` (por exemplo, use `order` para definir a ordem do componente de botão em relação aos outros componentes).

### Criar um Arquivo JavaScript

Agora, podemos ir para o lado do cliente para lidar com o evento de clique do novo botão. Primeiro, adicione um novo arquivo JavaScript à sua solução. Nós adicionamos dentro da pasta `/Pages/Identity/Users` do projeto `.Web`:

![extensão-ação-usuário-na-solução](../../images/user-action-extension-on-solution.png)

Aqui está o conteúdo deste arquivo JavaScript:

````js
$(function () {
    $('#ImportUsersFromExcel').click(function (e) {
        e.preventDefault();
        alert('TODO: importar usuários do Excel');
    });
});
````

No evento `click`, você pode fazer qualquer coisa que precise fazer.

### Adicionar o Arquivo à Página de Gerenciamento de Usuários

Em seguida, você precisa adicionar este arquivo JavaScript à página de gerenciamento de usuários. Você pode aproveitar o poder do sistema de [Agrupamento e Minificação](Bundling-Minification.md).

Escreva o seguinte código dentro do método `ConfigureServices` da classe do seu módulo:

````csharp
Configure<AbpBundlingOptions>(options =>
{
    options.ScriptBundles.Configure(
        typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel).FullName,
        bundleConfiguration =>
        {
            bundleConfiguration.AddFiles(
                "/Pages/Identity/Users/my-user-extensions.js"
            );
        });
});
````

Essa configuração adiciona `my-user-extensions.js` à página de gerenciamento de usuários do Módulo de Identidade. `typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel).FullName` é o nome do pacote na página de gerenciamento de usuários. Essa é uma convenção comum usada para todos os módulos comerciais do ABP.

## Casos de Uso Avançados

Embora você normalmente queira adicionar uma ação de botão à barra de ferramentas da página, é possível adicionar qualquer tipo de componente.

### Adicionar um Componente de Visualização à Barra de Ferramentas da Página

Primeiro, crie um novo componente de visualização em seu projeto:

![componente-personalizado-barra-ferramentas-página](../../images/page-toolbar-custom-component.png)

Para este exemplo, criamos um componente de visualização `MyToolbarItem` na pasta `/Pages/Identity/Users/MyToolbarItem`.

Conteúdo de `MyToolbarItemViewComponent.cs`:

````csharp
public class MyToolbarItemViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("~/Pages/Identity/Users/MyToolbarItem/Default.cshtml");
    }
}
````

Conteúdo de `Default.cshtml`:

````xml
<span>
    <button type="button" class="btn btn-dark">CLIQUE AQUI</button>
</span>
````

* O arquivo `.cshtml` pode conter qualquer tipo de componente(s). É um componente de visualização típico.
* `MyToolbarItemViewComponent` pode injetar e usar qualquer serviço, se necessário.

Em seguida, você pode adicionar o `MyToolbarItemViewComponent` à página de gerenciamento de usuários:

````csharp
Configure<AbpPageToolbarOptions>(options =>
{
    options.Configure<Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel>(
        toolbar =>
        {
            toolbar.AddComponent<MyToolbarItemViewComponent>();
        }
    );
});
````

* Se o seu componente aceitar argumentos (no método `Invoke`/`InvokeAsync`), você pode passá-los para o método `AddComponent` como um objeto anônimo.

#### Permissões

Se o seu botão/componente deve estar disponível com base em uma [permissão/política](../../Authorization.md), você pode passar o nome da permissão/política como parâmetro `requiredPolicyName` para os métodos `AddButton` e `AddComponent`.

### Adicionar um Contribuidor da Barra de Ferramentas da Página

Se você realizar uma lógica personalizada avançada ao adicionar um item à barra de ferramentas de uma página, pode criar uma classe que implementa a interface `IPageToolbarContributor` ou herda da classe `PageToolbarContributor`:

````csharp
public class MyToolbarContributor : PageToolbarContributor
{
    public override Task ContributeAsync(PageToolbarContributionContext context)
    {
        context.Items.Insert(0, new PageToolbarItem(typeof(MyToolbarItemViewComponent)));

        return Task.CompletedTask;
    }
}
````

* Você pode usar `context.ServiceProvider` para resolver dependências, se necessário.

Em seguida, adicione sua classe à lista `Contributors`:

````csharp
Configure<AbpPageToolbarOptions>(options =>
{
    options.Configure<Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel>(
        toolbar =>
        {
            toolbar.Contributors.Add(new MyToolbarContributor());
        }
    );
});
````