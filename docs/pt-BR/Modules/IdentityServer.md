# Módulo IdentityServer

O módulo IdentityServer fornece uma integração completa com o framework [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) (IDS), que oferece recursos avançados de autenticação, como logon único e controle de acesso a API. Este módulo persiste clientes, recursos e outros objetos relacionados ao IDS no banco de dados. **Este módulo foi substituído pelo** [módulo OpenIddict](https://docs.abp.io/en/abp/latest/Modules/OpenIddict) após o ABP v6.0 nos modelos de inicialização.

> Observação: Você não pode usar os módulos IdentityServer e OpenIddict juntos. Eles são bibliotecas separadas de provedor OpenID para a mesma função.

## Como instalar

Você não precisa deste módulo quando estiver usando o módulo OpenIddict. No entanto, se você deseja continuar usando o IdentityServer4 para suas aplicações, você pode instalar este módulo e remover o módulo OpenIddict. Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` [CLI](../CLI.md)) para desenvolver seu módulo personalizado.

### O código-fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/identityserver). O código-fonte é licenciado com [MIT](https://choosealicense.com/licenses/mit/), então você pode usá-lo e personalizá-lo livremente.

## Interface do usuário

Este módulo implementa a lógica de domínio e as integrações com o banco de dados, mas não fornece nenhuma interface do usuário. A interface de gerenciamento é útil se você precisar adicionar clientes e recursos dinamicamente. Nesse caso, você pode construir a interface de gerenciamento por conta própria ou considerar a compra do [ABP Commercial](https://commercial.abp.io/), que fornece a interface de gerenciamento para este módulo.

## Relações com outros módulos

Este módulo é baseado no [módulo Identity](Identity.md) e possui um [pacote de integração](https://www.nuget.org/packages/Volo.Abp.Account.Web.IdentityServer) com o [módulo Account](Account.md).

## Opções

### AbpIdentityServerBuilderOptions

`AbpIdentityServerBuilderOptions` pode ser configurado no método `PreConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics) do Identity Server. Exemplo:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<AbpIdentityServerBuilderOptions>(builder =>
	{
    	// Defina as opções aqui...		
	});
}
````

Propriedades de `AbpIdentityServerBuilderOptions`:

* `UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap` (padrão: true): Atualiza `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap` para ser compatível com as reivindicações do Identity Server.
* `UpdateAbpClaimTypes` (padrão: true): Atualiza `AbpClaimTypes` para ser compatível com as reivindicações do Identity Server.
* `IntegrateToAspNetIdentity` (padrão: true): Integra ao ASP.NET Identity.
* `AddDeveloperSigningCredential` (padrão: true): Defina como false para suprimir a chamada AddDeveloperSigningCredential() no IIdentityServerBuilder.

`IIdentityServerBuilder` pode ser configurado no método `PreConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics) do Identity Server. Exemplo:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
	PreConfigure<IIdentityServerBuilder>(builder =>
	{
    	builder.AddSigningCredential(...);	
	});
}
````

## Internos

### Camada de Domínio

#### Agregados

##### ApiResource

Os recursos da API são necessários para permitir que os clientes solicitem tokens de acesso.

* `ApiResource` (raiz do agregado): Representa um recurso da API no sistema.
  * `ApiSecret` (coleção): segredos do recurso da API.
  * `ApiScope` (coleção): escopos do recurso da API.
  * `ApiResourceClaim` (coleção): reivindicações do recurso da API.

##### Client

Os clientes representam aplicativos que podem solicitar tokens do seu Identity Server.

* `Client` (raiz do agregado): Representa um aplicativo cliente do Identity Server.
  * `ClientScope` (coleção): Escopos do cliente.
  * `ClientSecret` (coleção): Segredos do cliente.
  * `ClientGrantType` (coleção): Tipos de concessão do cliente.
  * `ClientCorsOrigin` (coleção): Origens CORS do cliente.
  * `ClientRedirectUri` (coleção): URIs de redirecionamento do cliente.
  * `ClientPostLogoutRedirectUri` (coleção): URIs de redirecionamento de logout do cliente.
  * `ClientIdPRestriction` (coleção): Restrições de provedor do cliente.
  * `ClientClaim` (coleção): Reivindicações do cliente.
  * `ClientProperty` (coleção): Propriedades personalizadas do cliente.

##### PersistedGrant

Persisted Grants armazena AuthorizationCodes, RefreshTokens e UserConsent.

* `PersistedGrant` (raiz do agregado): Representa um PersistedGrant para o servidor de identidade.

##### IdentityResource

Os recursos de identidade são dados como ID do usuário, nome ou endereço de e-mail de um usuário.

* `IdentityResource` (raiz do agregado): Representa um recurso de identidade do Identity Server.
  * `IdentityClaim` (coleção): Reivindicações do recurso de identidade.

#### Repositórios

Os seguintes repositórios personalizados são definidos para este módulo:

* `IApiResourceRepository`
* `IClientRepository`
* `IPersistentGrantRepository`
* `IIdentityResourceRepository`

#### Serviços de Domínio

Este módulo não contém nenhum serviço de domínio, mas substitui os serviços abaixo;

* `AbpProfileService` (Usado quando `AbpIdentityServerBuilderOptions.IntegrateToAspNetIdentity` é true)
* `AbpClaimsService`
* `AbpCorsPolicyService`

### Configurações

Este módulo não define nenhuma configuração.

### Camada de Aplicação

#### Serviços de Aplicação

* `ApiResourceAppService` (implementa `IApiResourceAppService`): Implementa os casos de uso da interface de gerenciamento de recursos da API.
* `IdentityServerClaimTypeAppService` (implementa `IIdentityServerClaimTypeAppService`): Usado para obter a lista de reivindicações.
* `ApiResourceAppService` (implementa `IApiResourceAppService`): Implementa os casos de uso da interface de gerenciamento de recursos da API.
* `IdentityResourceAppService` (implementa `IIdentityResourceAppService`): Implementa os casos de uso da interface de gerenciamento de recursos de identidade.

### Provedores de Banco de Dados

#### Comum

##### Prefixo de Tabela/Collection e Esquema

Todas as tabelas/collections usam o prefixo `IdentityServer` por padrão. Defina as propriedades estáticas na classe `AbpIdentityServerDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de Conexão

Este módulo usa `AbpIdentityServer` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`.

Consulte a documentação sobre [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter mais detalhes.

#### Entity Framework Core

##### Tabelas

* **IdentityServerApiResources**
  * IdentityServerApiSecrets
  * IdentityServerApiScopes
    * IdentityServerApiScopeClaims
  * IdentityServerApiClaims
* **IdentityServerClients**
  * IdentityServerClientScopes
  * IdentityServerClientSecrets
  * IdentityServerClientGrantTypes
  * IdentityServerClientCorsOrigins
  * IdentityServerClientRedirectUris
  * IdentityServerClientPostLogoutRedirectUris
  * IdentityServerClientIdPRestrictions
  * IdentityServerClientClaims
  * IdentityServerClientProperties
* **IdentityServerPersistedGrants**
* **IdentityServerIdentityResources**
  * IdentityServerIdentityClaims

#### MongoDB

##### Coleções

* **IdentityServerApiResources**
* **IdentityServerClients**
* **IdentityServerPersistedGrants**
* **IdentityServerIdentityResources**