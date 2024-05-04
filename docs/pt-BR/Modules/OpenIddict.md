## Módulo ABP OpenIddict

O módulo OpenIddict fornece uma integração com o [OpenIddict](https://github.com/openiddict/openiddict-core), que oferece recursos avançados de autenticação, como logon único, logoff único e controle de acesso à API. Este módulo persiste aplicativos, escopos e outros objetos relacionados ao OpenIddict no banco de dados.

## Como instalar

Este módulo já vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como um pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` [CLI](../CLI.md)) para desenvolver seu próprio módulo personalizado.

### O código-fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/openiddict). O código-fonte é licenciado pela [MIT](https://choosealicense.com/licenses/mit/), portanto, você pode usá-lo e personalizá-lo livremente.

## Interface do usuário

Este módulo implementa a lógica de domínio e as integrações com o banco de dados, mas não fornece nenhuma interface do usuário. A interface de gerenciamento é útil se você precisar adicionar aplicativos e escopos dinamicamente. Nesse caso, você pode construir a interface de gerenciamento por conta própria ou considerar a compra do [ABP Commercial](https://commercial.abp.io/), que fornece a interface de gerenciamento para este módulo.

## Relações com outros módulos

Este módulo é baseado no [Módulo de Identidade](Identity.md) e possui um [pacote de integração](https://www.nuget.org/packages/Volo.Abp.Account.Web.OpenIddict) com o [Módulo de Conta](Account.md).

## Opções

### OpenIddictBuilder

O `OpenIddictBuilder` pode ser configurado no método `PreConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics) OpenIddict.

Exemplo:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictBuilder>(builder =>
	{
    	// Defina as opções aqui...		
	});
}
```

O `OpenIddictBuilder` contém vários métodos de extensão para configurar os serviços do OpenIddict:

- `AddServer()` registra os serviços do servidor de token OpenIddict no contêiner de DI. Contém as configurações do `OpenIddictServerBuilder`.
- `AddCore()` registra os serviços principais do OpenIddict no contêiner de DI. Contém as configurações do `OpenIddictCoreBuilder`.
- `AddValidation()` registra os serviços de validação de token OpenIddict no contêiner de DI. Contém as configurações do `OpenIddictValidationBuilder`.

### OpenIddictCoreBuilder

O `OpenIddictCoreBuilder` contém métodos de extensão para configurar os serviços principais do OpenIddict.

Exemplo:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictCoreBuilder>(builder =>
	{
    	// Defina as opções aqui...		
	});
}
```

Esses serviços contêm:

- Adição de `ApplicationStore`, `AuthorizationStore`, `ScopeStore`, `TokenStore`.
- Substituição de `ApplicationManager`, `AuthorizationManager`, `ScopeManager`, `TokenManager`.
- Substituição de `ApplicationStoreResolver`, `AuthorizationStoreResolver`, `ScopeStoreResolver`, `TokenStoreResolver`.
- Definição de `DefaultApplicationEntity`, `DefaultAuthorizationEntity`, `DefaultScopeEntity`, `DefaultTokenEntity`.

### OpenIddictServerBuilder

O `OpenIddictServerBuilder` contém métodos de extensão para configurar os serviços do servidor OpenIddict.

Exemplo:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictServerBuilder>(builder =>
	{
    	// Defina as opções aqui...		
	});
}
```

Esses serviços contêm:

- Registro de claims, escopos.
- Definição do URI `Issuer` que é usado como endereço base para os URIs de endpoint retornados pelo endpoint de descoberta.
- Adição de chaves de assinatura de desenvolvimento, chaves de criptografia/assinatura, credenciais e certificados.
- Adição/remoção de manipuladores de eventos.
- Habilitação/desabilitação de tipos de concessão.
- Definição de URIs de endpoint do servidor de autenticação.

### OpenIddictValidationBuilder

O `OpenIddictValidationBuilder` contém métodos de extensão para configurar os serviços de validação do OpenIddict.

Exemplo:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<OpenIddictValidationBuilder>(builder =>
	{
    	// Defina as opções aqui...		
	});
}
```

Esses serviços contêm:

- `AddAudiences()` para servidores de recursos.
- `SetIssuer()` define o URI usado para determinar a localização real do documento de configuração do OAuth 2.0/OpenID Connect ao usar a descoberta do provedor.
- `SetConfiguration()` para configurar `OpenIdConnectConfiguration`.
- `UseIntrospection()` para usar a introspecção em vez da validação local/direta.
- Adição de chave de criptografia, credenciais e certificados.
- Adição/remoção de manipuladores de eventos.
- `SetClientId()` para definir o identificador do cliente `client_id` ao se comunicar com o servidor de autorização remoto (por exemplo, para introspecção).
- `SetClientSecret()` para definir o identificador `client_secret` ao se comunicar com o servidor de autorização remoto (por exemplo, para introspecção).
- `EnableAuthorizationEntryValidation()` para habilitar a validação de autorização para garantir que o `access token` ainda seja válido fazendo uma chamada ao banco de dados para cada solicitação da API. *Observação:* Isso pode ter um impacto negativo no desempenho e só pode ser usado com um servidor de autorização baseado no OpenIddict.
- `EnableTokenEntryValidation()` para habilitar a validação de autorização para garantir que o `access token` ainda seja válido fazendo uma chamada ao banco de dados para cada solicitação da API. *Observação:* Isso pode ter um impacto negativo no desempenho e é necessário quando o servidor OpenIddict está configurado para usar tokens de referência.
- `UseLocalServer()` para registrar os serviços de integração de validação/servidor OpenIddict.
- `UseAspNetCore()` para registrar os serviços de validação do OpenIddict para o ASP.NET Core no contêiner de DI.

## Internos

### Camada de Domínio

#### Agregados

##### OpenIddictApplication

OpenIddictApplications representam os aplicativos que podem solicitar tokens do seu servidor OpenIddict.

- `OpenIddictApplications` (raiz do agregado): Representa um aplicativo OpenIddict.
  - `ClientId` (string): O identificador do cliente associado ao aplicativo atual.
  - `ClientSecret` (string): O segredo do cliente associado ao aplicativo atual. Pode ser criptografado ou hash para fins de segurança.
  - `ConsentType` (string): O tipo de consentimento associado ao aplicativo atual.
  - `DisplayName` (string): O nome de exibição associado ao aplicativo atual.
  - `DisplayNames` (string): Os nomes de exibição localizados associados ao aplicativo atual serializados como um objeto JSON.
  - `Permissions` (string): As permissões associadas ao aplicativo atual, serializadas como um array JSON.
  - `PostLogoutRedirectUris` (string): As URLs de retorno de chamada de logoff associadas ao aplicativo atual, serializadas como um array JSON.
  - `Properties` (string): As propriedades adicionais associadas ao aplicativo atual serializadas como um objeto JSON ou nulo.
  - `RedirectUris` (string): As URLs de retorno de chamada associadas ao aplicativo atual, serializadas como um array JSON.
  - `Requirements` (string): Os requisitos associados ao aplicativo atual.
  - `Type` (string): O tipo de aplicativo associado ao aplicativo atual.
  - `ClientUri` (string): URI para obter mais informações sobre o cliente.
  - `LogoUri` (string): URI para o logotipo do cliente.

##### OpenIddictAuthorization

OpenIddictAuthorizations são usadas para manter os escopos permitidos e os tipos de fluxo de autorização.

- `OpenIddictAuthorization` (raiz do agregado): Representa uma autorização OpenIddict.

  - `ApplicationId` (Guid?): O aplicativo associado à autorização atual.

  - `Properties` (string): As propriedades adicionais associadas à autorização atual serializadas como um objeto JSON ou nulo.

  - `Scopes` (string): Os escopos associados à autorização atual, serializados como um array JSON.

  - `Status` (string): O status da autorização atual.

  - `Subject` (string): O assunto associado à autorização atual.

  - `Type` (string): O tipo da autorização atual.

##### OpenIddictScope

OpenIddictScopes são usados para manter os escopos dos recursos.

- `OpenIddictScope` (raiz do agregado): Representa um escopo OpenIddict.

  - `Description` (string): A descrição pública associada ao escopo atual.

  - `Descriptions` (string): As descrições públicas localizadas associadas ao escopo atual, serializadas como um objeto JSON.

  - `DisplayName` (string): O nome de exibição associado ao escopo atual.

  - `DisplayNames` (string): Os nomes de exibição localizados associados ao escopo atual serializados como um objeto JSON.

  - `Name` (string): O nome único associado ao escopo atual.
  - `Properties` (string): As propriedades adicionais associadas ao escopo atual serializadas como um objeto JSON ou nulo.
  - `Resources` (string): Os recursos associados ao escopo atual, serializados como um array JSON.

##### OpenIddictToken

OpenIddictTokens são usados para persistir os tokens do aplicativo.

- `OpenIddictToken` (raiz do agregado): Representa um token OpenIddict.

  - `ApplicationId` (Guid?): O aplicativo associado ao token atual.
  - `AuthorizationId` (Guid?): A autorização associada ao token atual.
  - `CreationDate` (DateTime?): A data de criação UTC do token atual.
  - `ExpirationDate` (DateTime?): A data de expiração UTC do token atual.
  - `Payload` (string): O payload do token atual, se aplicável. Usado apenas para tokens de referência e pode ser criptografado por motivos de segurança.

  - `Properties` (string): As propriedades adicionais associadas ao token atual serializadas como um objeto JSON ou nulo.
  - `RedemptionDate` (DateTime?): A data de resgate UTC do token atual.
  - `Status` (string): O status da autorização atual.

  - `ReferenceId` (string): O identificador de referência associado ao token atual, se aplicável. Usado apenas para tokens de referência e pode ser criptografado ou hash por motivos de segurança.

  - `Status` (string): O status do token atual.

  - `Subject` (string): O assunto associado ao token atual.

  - `Type` (string): O tipo do token atual.

#### Armazenamentos

Este módulo implementa os armazenamentos do OpenIddict:

- `IAbpOpenIdApplicationStore`
- `IOpenIddictAuthorizationStore`
- `IOpenIddictScopeStore`
- `IOpenIddictTokenStore`

#### AbpOpenIddictStoreOptions

Você pode configurar o `PruneIsolationLevel/DeleteIsolationLevel` do `AbpOpenIddictStoreOptions` para definir o nível de isolamento para as operações de armazenamento, pois diferentes bancos de dados têm diferentes níveis de isolamento.

##### Repositórios

Os seguintes repositórios personalizados são definidos neste módulo:

- `IOpenIddictApplicationRepository`
- `IOpenIddictAuthorizationRepository`
- `IOpenIddictScopeRepository`
- `IOpenIddictTokenRepository`

##### Serviços de Domínio

Este módulo não contém nenhum serviço de domínio, mas substitui o serviço abaixo:

- `AbpApplicationManager` usado para popular/obter informações do `AbpApplicationDescriptor` que contém `ClientUri` e `LogoUri`.

### Provedores de Banco de Dados

#### Comum

##### Prefixo de Tabela/Collection e Esquema

Todas as tabelas/collections usam o prefixo `OpenIddict` por padrão. Defina as propriedades estáticas na classe `AbpOpenIddictDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de Conexão

Este módulo usa `AbpOpenIddict` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`.

Consulte a documentação sobre [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter detalhes.

#### Entity Framework Core

##### Tabelas

- **OpenIddictApplications**
- **OpenIddictAuthorizations**
- **OpenIddictScopes**
- **OpenIddictTokens**

#### MongoDB

##### Coleções

- **OpenIddictApplications**
- **OpenIddictAuthorizations**
- **OpenIddictScopes**
- **OpenIddictTokens**

## Módulo ASP.NET Core

Este módulo integra o ASP.NET Core, com controladores MVC embutidos para quatro protocolos. Ele usa o modo de passagem do OpenIddict [Pass-through mode](https://documentation.openiddict.com/guides/index.html#pass-through-mode).

```cs
AuthorizeController -> connect/authorize
TokenController     -> connect/token
LogoutController    -> connect/logout
UserInfoController  -> connect/userinfo
```

> A implementação do **fluxo de dispositivo** será feita no módulo comercial.

#### AbpOpenIddictAspNetCoreOptions

`AbpOpenIddictAspNetCoreOptions` pode ser configurado no método `PreConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics) OpenIddict.

Exemplo:

```csharp
PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
{
    // Defina as opções aqui...
});
```

Propriedades do `AbpOpenIddictAspNetCoreOptions`:

- `UpdateAbpClaimTypes(default: true)`: Atualiza `AbpClaimTypes` para ser compatível com as reivindicações do Openiddict.
- `AddDevelopmentEncryptionAndSigningCertificate(default: true)`: Registra (e gera, se necessário) um certificado de criptografia/assinatura de desenvolvimento específico do usuário. Este é um certificado usado para assinar e criptografar os tokens e apenas para **ambiente de desenvolvimento**. Você deve defini-lo como **false** para ambientes não de desenvolvimento.

> `AddDevelopmentEncryptionAndSigningCertificate` não pode ser usado em aplicativos implantados no IIS ou no Azure App Service: tentar usá-los no IIS ou no Azure App Service resultará em uma exceção lançada em tempo de execução (a menos que o pool de aplicativos esteja configurado para carregar um perfil de usuário). Para evitar isso, considere criar certificados autoassinados e armazená-los no repositório de certificados X.509 da(s) máquina(s) host. Consulte: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-development-certificate

#### Removendo automaticamente Tokens/Autorizações Órfãs

A tarefa em segundo plano que remove automaticamente tokens/autorizações órfãs pode ser configurada por `TokenCleanupOptions`.

`TokenCleanupOptions` pode ser configurado no método `ConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics) OpenIddict.

Exemplo:

```csharp
Configure<TokenCleanupOptions>(options =>
{
    // Defina as opções aqui...
});
```

Propriedades do `TokenCleanupOptions`:

- `IsCleanupEnabled` (padrão: true): Habilita/desabilita a limpeza de token.
- `CleanupPeriod` (padrão: 3.600.000 ms): Define o período de limpeza.
- `DisableAuthorizationPruning`: Define um booleano indicando se a poda de autorizações deve ser desabilitada.
- `DisableTokenPruning`: Define um booleano indicando se a poda de tokens deve ser desabilitada.
- `MinimumAuthorizationLifespan` (padrão: 14 dias): Define a vida útil mínima que as autorizações devem ter para serem podadas. Não pode ser inferior a 10 minutos.
- `MinimumTokenLifespan` (padrão: 14 dias): Define a vida útil mínima que os tokens devem ter para serem podados. Não pode ser inferior a 10 minutos.

#### Atualizando Reivindicações em Access_token e Id_token

[Claims Principal Factory](https://docs.abp.io/en/abp/latest/Authorization#claims-principal-factory) pode ser usado para adicionar/remover reivindicações ao `ClaimsPrincipal`.

O serviço `AbpDefaultOpenIddictClaimsPrincipalHandler` adicionará tipos de reivindicações `Name`, `Email` e `Role` ao `access_token` e `id_token`, outras reivindicações são adicionadas apenas ao `access_token` por padrão e remove a reivindicação secreta `SecurityStampClaimType` do `Identity`.

Crie um serviço que herde de `IAbpOpenIddictClaimsPrincipalHandler` e adicione-o ao DI para controlar totalmente os destinos das reivindicações.

```cs
public class MyClaimDestinationsHandler : IAbpOpenIddictClaimsPrincipalHandler, ITransientDependency
{
    public virtual Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        foreach (var claim in context.Principal.Claims)
        {
            if (claim.Type == MyClaims.MyClaimsType)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
            }
	    
	    if (claim.Type == MyClaims.MyClaimsType2)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
            }
        }

        return Task.CompletedTask;
    }
}

Configure<AbpOpenIddictClaimsPrincipalOptions>(options =>
{
    options.ClaimsPrincipalHandlers.Add<MyClaimDestinationsHandler>();
});
```

Para obter informações detalhadas, consulte: [OpenIddict claim destinations](https://documentation.openiddict.com/configuration/claim-destinations.html)

#### Desabilitar a Criptografia do AccessToken

O ABP desabilita a `criptografia do access token` por padrão para compatibilidade, mas pode ser habilitada manualmente, se necessário.

```cs
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        builder.Configure(options => options.DisableAccessTokenEncryption = false);
    });
}
```

https://documentation.openiddict.com/configuration/token-formats.html#disabling-jwt-access-token-encryption

### Processo de Solicitação/Resposta

O `OpenIddict.Server.AspNetCore` adiciona um esquema de autenticação (`Name: OpenIddict.Server.AspNetCore, handler: OpenIddictServerAspNetCoreHandler`) e implementa a interface `IAuthenticationRequestHandler`.

Ele será executado primeiro no `AuthenticationMiddleware` e pode interromper o processamento da solicitação atual. Caso contrário, o `DefaultAuthenticateScheme` será chamado e continuará a executar o pipeline.

O `OpenIddictServerAspNetCoreHandler` chamará vários manipuladores embutidos (manipulando solicitações e respostas) e o manipulador processará de acordo com o contexto ou ignorará a lógica que não tem relação com ele.

Exemplo de uma solicitação de token:

```
POST /connect/token HTTP/1.1
Content-Type: application/x-www-form-urlencoded

    grant_type=password&
    client_id=AbpApp&
    client_secret=1q2w3e*&
    username=admin&
    password=1q2w3E*&
    scope=AbpAPI offline_access
```

Esta solicitação será processada por vários manipuladores. Eles confirmarão o tipo de endpoint da solicitação, verificarão `HTTP/HTTPS`, verificarão se os parâmetros da solicitação (`client, scope`, etc.) são válidos e existem no banco de dados, etc. Várias verificações de protocolo. E construir um objeto `OpenIddictRequest`, Se houver erros, o conteúdo da resposta pode ser definido e interromper diretamente a solicitação atual.

Se tudo estiver ok, a solicitação irá para nosso controlador de processamento (por exemplo, `TokenController`), podemos obter um `OpenIddictRequest` da solicitação HTTP neste momento. O restante será baseado neste objeto.

Verifique o `username` e `password` na solicitação. Se estiver correto, crie um objeto `ClaimsPrincipal` e retorne um `SignInResult`, que usa o nome do esquema de autenticação `OpenIddict.Validation.AspNetCore`, chamará o `OpenIddictServerAspNetCoreHandler` para processamento.

O `OpenIddictServerAspNetCoreHandler` fará algumas verificações para gerar json e substituir o conteúdo da resposta HTTP.

O `ForbidResult` `ChallengeResult` são todos os tipos de processamento acima.

Se você precisar personalizar o OpenIddict, será necessário substituir/excluir/adicionar novos manipuladores e fazer com que ele seja executado na ordem correta.

Consulte: https://documentation.openiddict.com/guides/index.html#events-model

### PKCE

https://documentation.openiddict.com/configuration/proof-key-for-code-exchange.html

### Definindo o Tempo de Vida dos Tokens

Atualize o método `PreConfigureServices` do arquivo AuthServerModule (ou HttpApiHostModule se você não tiver um servidor de autenticação separado) :

```csharp
PreConfigure<OpenIddictServerBuilder>(builder =>
{
    builder.SetAuthorizationCodeLifetime(TimeSpan.FromMinutes(30));
    builder.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
    builder.SetIdentityTokenLifetime(TimeSpan.FromMinutes(30));
    builder.SetRefreshTokenLifetime(TimeSpan.FromDays(14));
});
```

### Token de Atualização

Para usar o token de atualização, ele deve ser suportado pelo OpenIddictServer e o `refresh_token` deve ser solicitado pelo aplicativo.

> **Observação:** O aplicativo Angular já está configurado para usar o `refresh_token`.

#### Configurando o OpenIddictServer

Atualize o **OpenIddictDataSeedContributor**, adicione `OpenIddictConstants.GrantTypes.RefreshToken` aos tipos de concessão no método `CreateApplicationAsync`:

```csharp
await CreateApplicationAsync(
    ...
    grantTypes: new List<string> //Fluxo híbrido
    {
        OpenIddictConstants.GrantTypes.AuthorizationCode,
        OpenIddictConstants.GrantTypes.Implicit,
        OpenIddictConstants.GrantTypes.RefreshToken,
    },
    ...
```

> **Observação:** Você precisa recriar esse cliente se já tiver gerado o banco de dados.

#### Configurando o Aplicativo:

Você precisa solicitar o escopo **offline_access** para poder receber o `refresh_token`.

Nos aplicativos **Razor/MVC, Blazor-Server**, adicione `options.Scope.Add("offline_access");` às opções **OpenIdConnect**. Esses modelos de aplicativo usam autenticação por cookie por padrão e têm as opções de expiração do cookie definidas como:

```csharp
.AddCookie("Cookies", options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
})
```

[Cookie ExpireTimeSpan ignorará a expiração do access_token](https://learn.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions.ExpireTimeSpan?view=aspnetcore-7.0&viewFallbackFrom=net-7.0) e o access_token expirado ainda será válido se for definido com um valor maior que o `refresh_token lifetime`. É recomendável manter o **Cookie ExpireTimeSpan** e o **Refresh Token lifetime** iguais, para que o novo token seja persistido no cookie.

Nos aplicativos **Blazor wasm**, adicione `options.ProviderOptions.DefaultScopes.Add("offline_access");` às opções **AddOidcAuthentication**.

Nos aplicativos **Angular**, adicione `offline_access` aos escopos **oAuthConfig** no arquivo *environment.ts*. (Os aplicativos Angular já têm essa configuração).

## Sobre a localização

Não localizamos nenhuma mensagem de erro no módulo OpenIddict, porque a especificação OAuth 2.0 restringe o conjunto de caracteres que você pode usar para os parâmetros de erro e error_description:

> A.7. "error" Syntax
> O elemento "error" é definido nas Seções 4.1.2.1, 4.2.2.1, 5.2, 7.2 e 8.5:

```
error = 1*NQSCHAR
```

> A.8. "error_description" Syntax
> O elemento "error_description" é definido nas Seções 4.1.2.1, 4.2.2.1, 5.2 e 7.2:

```
error-description = 1*NQSCHAR
NQSCHAR = %x20-21 / %x23-5B / %x5D-7E
```

## Projetos de demonstração

No diretório `app` do módulo, existem seis projetos (incluindo `angular`)

* `OpenIddict.Demo.Server`: Um aplicativo abp com módulos integrados (possui dois `clientes` e um `escopo`).
* `OpenIddict.Demo.API`: Aplicativo ASP NET Core API usando autenticação JwtBearer.
* `OpenIddict.Demo.Client.Mvc`: Aplicativo ASP NET Core MVC usando `OpenIdConnect` para autenticação.
* `OpenIddict.Demo.Client.Console`: Use `IdentityModel` para testar os vários endpoints do OpenIddict e chamar a API do `OpenIddict.Demo.API`.
* `OpenIddict.Demo.Client.BlazorWASM:` Aplicativo Blazor ASP NET Core usando `OidcAuthentication` para autenticação.
* `angular`: Um aplicativo angular que integra os módulos abp ng e usa oauth para autenticação.

#### Como executar?

Confirme a string de conexão do `appsettings.json` no projeto `OpenIddict.Demo.Server`. A execução do projeto criará automaticamente o banco de dados e inicializará os dados.
Após executar o projeto `OpenIddict.Demo.API`, você pode executar o restante dos projetos para testar. 

## Guia de Migração

[Guia de Migração Passo a Passo do IdentityServer para o OpenIddict](../Migration-Guides/OpenIddict-Step-by-Step.md)