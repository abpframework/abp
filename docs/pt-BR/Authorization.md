# Autorização

A autorização é usada para verificar se um usuário tem permissão para realizar operações específicas na aplicação.

O ABP estende a [Autorização do ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/introduction) adicionando **permissões** como [políticas](https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/policies) automáticas e permitindo que o sistema de autorização seja utilizado nos **[serviços de aplicação](Application-Services.md)** também.

Portanto, todos os recursos de autorização do ASP.NET Core e a documentação são válidos em uma aplicação baseada no ABP. Este documento se concentra nos recursos adicionados ao sistema de autorização do ASP.NET Core.

## Atributo Authorize

O ASP.NET Core define o atributo [**Authorize**](https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/simple) que pode ser usado para uma ação, um controlador ou uma página. O ABP permite que você use o mesmo atributo para um [serviço de aplicação](Application-Services.md).

Exemplo:

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;

namespace Acme.BookStore
{
    [Authorize]
    public class AuthorAppService : ApplicationService, IAuthorAppService
    {
        public Task<List<AuthorDto>> GetListAsync()
        {
            ...
        }

        [AllowAnonymous]
        public Task<AuthorDto> GetAsync(Guid id)
        {
            ...
        }

        [Authorize("BookStore_Author_Create")]
        public Task CreateAsync(CreateAuthorDto input)
        {
            ...
        }
    }
}

```

- O atributo `Authorize` obriga o usuário a fazer login na aplicação para usar os métodos do `AuthorAppService`. Portanto, o método `GetListAsync` está disponível apenas para usuários autenticados.
- `AllowAnonymous` suprime a autenticação. Portanto, o método `GetAsync` está disponível para todos, incluindo usuários não autorizados.
- `[Authorize("BookStore_Author_Create")]` define uma política (consulte [autorização baseada em políticas](https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/policies)) que é verificada para autorizar o usuário atual.

"BookStore_Author_Create" é um nome de política arbitrário. Se você declarar um atributo como esse, o sistema de autorização do ASP.NET Core espera que uma política seja definida anteriormente.

Você pode, é claro, implementar suas próprias políticas conforme descrito na documentação do ASP.NET Core. Mas para condições simples de verdadeiro/falso, como se uma política foi concedida a um usuário ou não, o ABP define o sistema de permissões, que será explicado na próxima seção.

## Sistema de Permissões

Uma permissão é uma política simples que é concedida ou proibida para um usuário, função ou cliente específico.

### Definindo Permissões

Para definir permissões, crie uma classe que herde de `PermissionDefinitionProvider`, conforme mostrado abaixo:

```csharp
using Volo.Abp.Authorization.Permissions;

namespace Acme.BookStore.Permissions
{
    public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("BookStore");

            myGroup.AddPermission("BookStore_Author_Create");
        }
    }
}
```

> O ABP descobre automaticamente essa classe. Nenhuma configuração adicional é necessária!

> Normalmente, você define essa classe dentro do projeto `Application.Contracts` da sua [aplicação](Startup-Templates/Application.md). O modelo de inicialização já vem com uma classe vazia chamada *YourProjectNamePermissionDefinitionProvider* com a qual você pode começar.

No método `Define`, você precisa adicionar um **grupo de permissões** ou obter um grupo existente e adicionar **permissões** a esse grupo.

Quando você define uma permissão, ela se torna utilizável no sistema de autorização do ASP.NET Core como um nome de **política**. Ela também se torna visível na interface do usuário. Veja o diálogo de permissões para uma função:

![authorization-new-permission-ui](images/authorization-new-permission-ui.png)

- O grupo "BookStore" é mostrado como uma nova guia no lado esquerdo.
- "BookStore_Author_Create" no lado direito é o nome da permissão. Você pode concedê-la ou proibi-la para a função.

Quando você salva o diálogo, ele é salvo no banco de dados e usado no sistema de autorização.

> A tela acima está disponível quando você instalou o módulo de identidade, que é usado principalmente para gerenciamento de usuários e funções. Os modelos de inicialização já vêm com o módulo de identidade pré-instalado.

#### Localizando o Nome da Permissão

"BookStore_Author_Create" não é um bom nome de permissão para a interface do usuário. Felizmente, os métodos `AddPermission` e `AddGroup` podem receber um `LocalizableString` como segundo parâmetro:

```csharp
var myGroup = context.AddGroup(
    "BookStore",
    LocalizableString.Create<BookStoreResource>("BookStore")
);

myGroup.AddPermission(
    "BookStore_Author_Create",
    LocalizableString.Create<BookStoreResource>("Permission:BookStore_Author_Create")
);
```

Em seguida, você pode definir os textos para as chaves "BookStore" e "Permission:BookStore_Author_Create" no arquivo de localização:

```json
"BookStore": "Livraria",
"Permission:BookStore_Author_Create": "Criar um novo autor"
```

> Para mais informações, consulte a [documentação de localização](Localization.md) sobre o sistema de localização.

A interface do usuário localizada será como mostrado abaixo:

![authorization-new-permission-ui-localized](images/authorization-new-permission-ui-localized.png)

#### Multi-Tenancy

O ABP suporta [multi-tenancy](Multi-Tenancy.md) como um recurso de primeira classe. Você pode definir a opção de lado de multi-tenancy ao definir uma nova permissão. Ela pode ter um dos três valores definidos abaixo:

- **Host**: A permissão está disponível apenas para o lado do host.
- **Tenant**: A permissão está disponível apenas para o lado do tenant.
- **Ambos** (padrão): A permissão está disponível tanto para o lado do tenant quanto para o lado do host.

> Se sua aplicação não é multi-tenant, você pode ignorar essa opção.

Para definir a opção de lado de multi-tenancy, passe para o terceiro parâmetro do método `AddPermission`:

```csharp
myGroup.AddPermission(
    "BookStore_Author_Create",
    LocalizableString.Create<BookStoreResource>("Permission:BookStore_Author_Create"),
    multiTenancySide: MultiTenancySides.Tenant //defina o lado de multi-tenancy!
);
```

#### Habilitar/Desabilitar Permissões

Uma permissão está habilitada por padrão. É possível desabilitar uma permissão. Uma permissão desabilitada será proibida para todos. Você ainda pode verificar a permissão, mas ela sempre retornará proibida.

Exemplo de definição:

````csharp
myGroup.AddPermission("Author_Management", isEnabled: false);
````

Normalmente, você não precisa definir uma permissão desabilitada (a menos que queira desabilitar temporariamente um recurso da sua aplicação). No entanto, você pode querer desabilitar uma permissão definida em um módulo dependente. Dessa forma, você pode desabilitar a funcionalidade relacionada à aplicação. Consulte a seção "*Alterando as Definições de Permissão de um Módulo Dependente*" abaixo para um exemplo de uso.

> Observação: Verificar uma permissão não definida lançará uma exceção, enquanto verificar uma permissão desabilitada simplesmente retornará proibida (falso).

#### Permissões Filhas

Uma permissão pode ter permissões filhas. Isso é especialmente útil quando você deseja criar uma árvore de permissões hierárquica, onde uma permissão pode ter permissões secundárias adicionais que estão disponíveis apenas se a permissão pai for concedida.

Exemplo de definição:

```csharp
var authorManagement = myGroup.AddPermission("Author_Management");
authorManagement.AddChild("Author_Management_Create_Books");
authorManagement.AddChild("Author_Management_Edit_Books");
authorManagement.AddChild("Author_Management_Delete_Books");
```

O resultado na interface do usuário é mostrado abaixo (provavelmente você desejará localizar as permissões para sua aplicação):

![authorization-new-permission-ui-hierarcy](images/authorization-new-permission-ui-hierarcy.png)

Para o código de exemplo, é assumido que uma função/usuário com a permissão "Author_Management" concedida pode ter permissões adicionais. Em seguida, um serviço de aplicação típico que verifica permissões pode ser definido como mostrado abaixo:

```csharp
[Authorize("Author_Management")]
public class AuthorAppService : ApplicationService, IAuthorAppService
{
    public Task<List<AuthorDto>> GetListAsync()
    {
        ...
    }

    public Task<AuthorDto> GetAsync(Guid id)
    {
        ...
    }

    [Authorize("Author_Management_Create_Books")]
    public Task CreateAsync(CreateAuthorDto input)
    {
        ...
    }

    [Authorize("Author_Management_Edit_Books")]
    public Task UpdateAsync(CreateAuthorDto input)
    {
        ...
    }

    [Authorize("Author_Management_Delete_Books")]
    public Task DeleteAsync(CreateAuthorDto input)
    {
        ...
    }
}
```

- `GetListAsync` e `GetAsync` estarão disponíveis para usuários se a permissão `Author_Management` for concedida.
- Outros métodos requerem permissões adicionais.

### Substituindo uma Permissão por uma Política Personalizada

Se você definir e registrar uma política no sistema de autorização do ASP.NET Core com o mesmo nome de uma permissão, sua política substituirá a permissão existente. Isso é uma maneira poderosa de estender a autorização para um módulo pré-construído que você está usando em sua aplicação.

Consulte o documento [autorização baseada em políticas](https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/policies) para aprender como definir uma política personalizada.

### Alterando as Definições de Permissão de um Módulo Dependente

Uma classe derivada de `PermissionDefinitionProvider` (assim como o exemplo acima) também pode obter definições de permissão existentes (definidas pelos [módulos](Module-Development-Basics.md) dependentes) e alterar suas definições.

Exemplo:

````csharp
context
    .GetPermissionOrNull(IdentityPermissions.Roles.Delete)
    .IsEnabled = false;
````

Quando você escreve esse código dentro do seu provedor de definição de permissão, ele encontra a permissão de "exclusão de função" do [Módulo de Identidade](Modules/Identity.md) e desabilita a permissão, para que ninguém possa excluir uma função na aplicação.

> Dica: É melhor verificar o valor retornado pelo método `GetPermissionOrNull`, pois ele pode retornar nulo se a permissão fornecida não foi definida.

### Provedores de Valor de Permissão

O sistema de verificação de permissões é extensível. Qualquer classe derivada de `PermissionValueProvider` (ou que implemente `IPermissionValueProvider`) pode contribuir para a verificação de permissões. Existem três provedores de valor predefinidos:

- `UserPermissionValueProvider` verifica se o usuário atual tem a permissão concedida. Ele obtém o ID do usuário das reivindicações atuais. O nome da reivindicação do usuário é definido pela propriedade estática `AbpClaimTypes.UserId`.
- `RolePermissionValueProvider` verifica se algum dos papéis do usuário atual tem a permissão concedida. Ele obtém os nomes dos papéis das reivindicações atuais. O nome das reivindicações de papéis é definido pela propriedade estática `AbpClaimTypes.Role`.
- `ClientPermissionValueProvider` verifica se o cliente atual tem a permissão concedida. Isso é especialmente útil em uma interação máquina a máquina, onde não há usuário atual. Ele obtém o ID do cliente das reivindicações atuais. O nome da reivindicação do cliente é definido pela propriedade estática `AbpClaimTypes.ClientId`.

Você pode estender o sistema de verificação de permissões definindo seu próprio provedor de valor de permissão.

Exemplo:

```csharp
public class SystemAdminPermissionValueProvider : PermissionValueProvider
{
    public SystemAdminPermissionValueProvider(IPermissionStore permissionStore)
        : base(permissionStore)
    {
    }

    public override string Name => "SystemAdmin";

    public async override Task<PermissionGrantResult>
           CheckAsync(PermissionValueCheckContext context)
    {
        if (context.Principal?.FindFirst("User_Type")?.Value == "SystemAdmin")
        {
            return PermissionGrantResult.Granted;
        }

        return PermissionGrantResult.Undefined;
    }
}
```

Esse provedor permite que todas as permissões sejam concedidas a um usuário com uma reivindicação `User_Type` que tenha o valor `SystemAdmin`. É comum usar as reivindicações atuais e o `IPermissionStore` em um provedor de valor de permissão.

Um provedor de valor de permissão deve retornar um dos seguintes valores do método `CheckAsync`:

- `PermissionGrantResult.Granted` é retornado para conceder a permissão ao usuário. Se qualquer um dos provedores retornar `Granted`, o resultado será `Granted`, se nenhum outro provedor retornar `Prohibited`.
- `PermissionGrantResult.Prohibited` é retornado para proibir a permissão ao usuário. Se qualquer um dos provedores retornar `Prohibited`, o resultado será sempre `Prohibited`. Não importa o que os outros provedores retornem.
- `PermissionGrantResult.Undefined` é retornado se esse provedor de valor de permissão não puder decidir sobre o valor da permissão. Retorne isso para permitir que outros provedores verifiquem a permissão.

Uma vez que um provedor é definido, ele deve ser adicionado às `AbpPermissionOptions`, como mostrado abaixo:

```csharp
Configure<AbpPermissionOptions>(options =>
{
    options.ValueProviders.Add<SystemAdminPermissionValueProvider>();
});
```

### Armazenamento de Permissões

`IPermissionStore` é a única interface que precisa ser implementada para ler o valor das permissões de uma fonte de persistência, geralmente um sistema de banco de dados. O módulo de gerenciamento de permissões a implementa e é pré-instalado no modelo de inicialização da aplicação. Consulte a [documentação do módulo de gerenciamento de permissões](Modules/Permission-Management.md) para obter mais informações.

### AlwaysAllowAuthorizationService

`AlwaysAllowAuthorizationService` é uma classe usada para ignorar o serviço de autorização. Geralmente é usado em testes de integração, onde você pode querer desabilitar o sistema de autorização.

Use o método de extensão `IServiceCollection.AddAlwaysAllowAuthorization()` para registrar o `AlwaysAllowAuthorizationService` no sistema de [injeção de dependência](Dependency-Injection.md):

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAlwaysAllowAuthorization();
}
```

Isso já é feito para testes de integração do modelo de inicialização.

### Fábrica de Claims Principal

As reivindicações são elementos importantes da autenticação e autorização. O ABP usa o serviço `IAbpClaimsPrincipalFactory` para criar reivindicações na autenticação. Esse serviço foi projetado para ser extensível. Se você precisar adicionar suas próprias reivindicações ao ticket de autenticação, poderá implementar `IAbpClaimsPrincipalContributor` em sua aplicação.

**Exemplo: Adicionar uma reivindicação `SocialSecurityNumber` e obtê-la:**

```csharp
public class SocialSecurityNumberClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        var userId = identity?.FindUserId();
        if (userId.HasValue)
        {
            var userService = context.ServiceProvider.GetRequiredService<IUserService>(); //Seu serviço personalizado
            var socialSecurityNumber = await userService.GetSocialSecurityNumberAsync(userId.Value);
            if (socialSecurityNumber != null)
            {
                identity.AddClaim(new Claim("SocialSecurityNumber", socialSecurityNumber));
            }
        }
    }
}


public static class CurrentUserExtensions
{
    public static string GetSocialSecurityNumber(this ICurrentUser currentUser)
    {
        return currentUser.FindClaimValue("SocialSecurityNumber");
    }
}
```

> Se você estiver usando o Identity Server, adicione suas reivindicações a `RequestedClaims` de `AbpClaimsServiceOptions`.

```csharp
Configure<AbpClaimsServiceOptions>(options =>
{
    options.RequestedClaims.AddRange(new[]{ "SocialSecurityNumber" });
});
```

## Veja também

* [Módulo de Gerenciamento de Permissões](Modules/Permission-Management.md)
* [API de Autenticação JavaScript do ASP.NET Core MVC / Razor Pages](UI/AspNetCore/JavaScript-API/Auth.md)
* [Gerenciamento de Permissões na Interface do Usuário Angular](UI/Angular/Permission-Management.md)
* [Tutorial em vídeo](https://abp.io/video-courses/essentials/authorization)</source>