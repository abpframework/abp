# ASP.NET Core MVC / Razor Pages: Alertas de Página

É comum mostrar alertas de erro, aviso ou informação para informar o usuário. Um exemplo de alerta de *Interrupção de Serviço* é mostrado abaixo:

![exemplo-alerta-pagina](../../images/exemplo-alerta-pagina.png)

## Uso Básico

Se você herda diretamente ou indiretamente de `AbpPageModel`, você pode usar a propriedade `Alerts` para adicionar alertas que serão renderizados após a conclusão da solicitação.

**Exemplo: Mostrar um alerta de aviso**

```csharp
namespace MyProject.Web.Pages
{
    public class IndexModel : MyProjectPageModel //ou herde de AbpPageModel
    {
        public void OnGet()
        {
            Alerts.Warning(
                text: "Teremos uma interrupção de serviço entre 02:00 e 04:00 em 23 de outubro de 2023!",
                title: "Interrupção de Serviço"
            );
        }
    }
}
```

Este uso renderiza um alerta que foi mostrado acima. Se você precisar localizar as mensagens, você sempre pode usar o sistema padrão de [localização](../../Localization.md).

### Exceções / Estados Inválidos do Modelo

É comum mostrar alertas quando você manipula manualmente exceções (com declarações try/catch) ou deseja lidar com o caso `!ModelState.IsValid` e avisar o usuário. Por exemplo, o Módulo de Conta mostra um aviso se o usuário inserir um nome de usuário ou senha incorretos:

![layout-alerta-conta](../../images/layout-alerta-conta.png)

> Observe que geralmente você não precisa manipular exceções manualmente, pois o ABP Framework fornece um sistema automático de [manipulação de exceções](../../Exception-Handling.md).

### Tipos de Alerta

`Warning` é usado para mostrar um alerta de aviso. Outros métodos comuns são `Info`, `Danger` e `Success`.

Além dos métodos padrão, você pode usar o método `Alerts.Add` passando um `enum` `AlertType` com um desses valores: `Default`, `Primary`, `Secondary`, `Success`, `Danger`, `Warning`, `Info`, `Light`, `Dark`.

### Dispensável

Todos os métodos de alerta recebem um parâmetro opcional `dismissible`. O valor padrão é `true`, o que torna a caixa de alerta dispensável. Defina-o como `false` para criar uma caixa de alerta fixa.

## IAlertManager

Se você precisar adicionar mensagens de alerta de outra parte do seu código, você pode injetar o serviço `IAlertManager` e usar sua lista `Alerts`.

**Exemplo: Injetar o `IAlertManager`** 

```csharp
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.DependencyInjection;

namespace MyProject.Web.Pages
{
    public class MyService : ITransientDependency
    {
        private readonly IAlertManager _alertManager;

        public MyService(IAlertManager alertManager)
        {
            _alertManager = alertManager;
        }

        public void Test()
        {
            _alertManager.Alerts.Add(AlertType.Danger, "Mensagem de teste!");
        }
    }
}
```

## Notas

### Requisições AJAX

O sistema de Alerta de Página foi projetado para ser usado em uma solicitação regular de página completa. Não é para requisições AJAX/partial. Os alertas são renderizados no layout da página, portanto, é necessário atualizar a página inteira.

Para requisições AJAX, é mais adequado lançar exceções (por exemplo, `UserFriendlyException`). Consulte o documento de [manipulação de exceções](../../Exception-Handling.md).