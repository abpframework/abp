# Módulo de Gerenciamento de Permissões

Este módulo implementa o `IPermissionStore` para armazenar e gerenciar valores de permissões em um banco de dados.

> Este documento aborda apenas o módulo de gerenciamento de permissões que persiste os valores de permissão em um banco de dados. Consulte o documento de [Autorização](../Authorization.md) para entender os sistemas de autorização e permissão.

## Como Instalar

Este módulo já vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` da [CLI](../CLI.md)) para desenvolver seu próprio módulo personalizado.

### O Código-fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/permission-management). O código-fonte é licenciado com a licença [MIT](https://choosealicense.com/licenses/mit/), portanto, você pode usá-lo e personalizá-lo livremente.

## Interface do Usuário

### Diálogo de Gerenciamento de Permissões

O módulo de gerenciamento de permissões fornece um diálogo reutilizável para gerenciar permissões relacionadas a um objeto. Por exemplo, o [Módulo de Identidade](Identity.md) o utiliza para gerenciar as permissões de usuários e funções. A imagem a seguir mostra a página de Gerenciamento de Funções do Módulo de Identidade:

![permissions-module-open-dialog](../images/permissions-module-open-dialog.png)

Quando você clica em *Ações* -> *Permissões* para uma função, o diálogo de gerenciamento de permissões é aberto. Uma captura de tela de exemplo deste diálogo:

![permissions-module-dialog](../images/permissions-module-dialog.png)

Neste diálogo, você pode conceder permissões para a função selecionada. As abas no lado esquerdo representam os principais grupos de permissões e o lado direito contém as permissões definidas no grupo selecionado.

## IPermissionManager

`IPermissionManager` é o serviço principal fornecido por este módulo. Ele é usado para ler e alterar os valores de permissão. `IPermissionManager` é normalmente usado pelo *Diálogo de Gerenciamento de Permissões*. No entanto, você pode injetá-lo se precisar definir um valor de permissão.

> Se você apenas deseja ler/verificar os valores de permissão para o usuário atual, use o `IAuthorizationService` ou o atributo `[Authorize]`, conforme explicado no documento de [Autorização](../Authorization.md).

**Exemplo: Conceder permissões para funções e usuários usando o serviço `IPermissionManager`**

````csharp
public class MeuServico : ITransientDependency
{
    private readonly IPermissionManager _permissionManager;

    public MeuServico(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    public async Task ConcederPermissaoParaFuncaoDemoAsync(
        string nomeFuncao, string permissao)
    {
        await _permissionManager
            .SetForRoleAsync(nomeFuncao, permissao, true);
    }

    public async Task ConcederPermissaoParaUsuarioDemoAsync(
        Guid idUsuario, string nomeFuncao, string permissao)
    {
        await _permissionManager
            .SetForUserAsync(idUsuario, permissao, true);
    }
}
````

## Provedores de Gerenciamento de Permissões

O Módulo de Gerenciamento de Permissões é extensível, assim como o [sistema de permissões](../Authorization.md). Você pode estendê-lo definindo provedores de gerenciamento de permissões.

O [Módulo de Identidade](Identity.md) define os seguintes provedores de gerenciamento de permissões:

* `UserPermissionManagementProvider`: Gerencia permissões baseadas em usuários.
* `RolePermissionManagementProvider`: Gerencia permissões baseadas em funções.

`IPermissionManager` usa esses provedores quando você obtém/define permissões. Você pode definir seu próprio provedor implementando o `IPermissionManagementProvider` ou herdando da classe base `PermissionManagementProvider`.

**Exemplo:**

````csharp
public class CustomPermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => "Custom";

    public CustomPermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
    }
}
````

A classe base `PermissionManagementProvider` faz a implementação padrão (usando o `IPermissionGrantRepository`) para você. Você pode substituir os métodos base conforme necessário. Cada provedor deve ter um nome exclusivo, que é `Custom` neste exemplo (mantenha-o curto, pois ele é salvo no banco de dados para cada registro de valor de permissão).

Depois de criar sua classe de provedor, você deve registrá-la usando a classe de opções `PermissionManagementOptions` [options class](../Options.md):

````csharp
Configure<PermissionManagementOptions>(options =>
{
    options.ManagementProviders.Add<CustomPermissionManagementProvider>();
});
````

A ordem dos provedores é importante. Os provedores são executados na ordem inversa. Isso significa que o `CustomPermissionManagementProvider` é executado primeiro neste exemplo. Você pode inserir seu provedor em qualquer ordem na lista `Providers`.

## Veja também

* [Autorização](../Authorization.md)