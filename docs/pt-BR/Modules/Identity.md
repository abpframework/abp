# Módulo de Gerenciamento de Identidade

O módulo de identidade é usado para gerenciar funções, usuários e suas permissões, com base na biblioteca de identidade da Microsoft.

## Como instalar

Este módulo já vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` [CLI](../CLI.md)) para desenvolver seu próprio módulo personalizado.

### O Código-fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/identity). O código-fonte é licenciado com [MIT](https://choosealicense.com/licenses/mit/), então você pode usá-lo e personalizá-lo livremente.

## Interface do Usuário

Este módulo fornece opções de interface do usuário [Blazor](../UI/Blazor/Overall.md), [Angular](../UI/Angular/Quick-Start.md) e [MVC / Razor Pages](../UI/AspNetCore/Overall.md).

### Itens do Menu

Este módulo adiciona um item de menu *Gerenciamento de Identidade* no menu *Administração*:

![identity-module-menu](../images/identity-module-menu.png)

Os itens do menu e as páginas relacionadas são autorizados. Isso significa que o usuário atual deve ter as permissões relacionadas para torná-los visíveis. A função `admin` (e os usuários com essa função - como o usuário `admin`) já possui essas permissões. Se você deseja habilitar permissões para outras funções/usuários, abra a caixa de diálogo *Permissões* na página *Funções* ou *Usuários* e marque as permissões conforme mostrado abaixo:

![identity-module-permissions](../images/identity-module-permissions.png)

Consulte o documento de [Autorização](../Authorization.md) para entender o sistema de permissões.

### Páginas

Esta seção apresenta as principais páginas fornecidas por este módulo.

#### Usuários

Esta página é usada para ver a lista de usuários. Você pode criar/editar e excluir usuários, atribuir usuários a funções.

![identity-module-users](../images/identity-module-users.png)

Um usuário pode ter zero ou mais funções. Os usuários herdam permissões de suas funções. Além disso, você pode atribuir permissões diretamente aos usuários (clicando no botão *Ações*, em seguida, selecionando *Permissões*).

#### Funções

As funções são usadas para agrupar permissões e atribuí-las aos usuários.

![identity-module-roles](../images/identity-module-roles.png)

Além do nome da função, existem duas propriedades de uma função:

* `Padrão`: Se uma função for marcada como "padrão", essa função será atribuída aos novos usuários por padrão quando eles se registrarem na aplicação (usando o [Módulo de Conta](Account.md)).
* `Público`: Uma função pública de um usuário pode ser vista por outros usuários na aplicação. Essa funcionalidade não tem uso no módulo de identidade, mas é fornecida como uma funcionalidade que você pode querer usar em sua própria aplicação.

## Outras Funcionalidades

Esta seção abrange algumas outras funcionalidades fornecidas por este módulo que não possuem páginas de interface do usuário.

### Unidades Organizacionais

As unidades organizacionais (OU) podem ser usadas para agrupar usuários e entidades de forma hierárquica.

#### Entidade Unidade Organizacional

Uma OU é representada pela entidade **UnidadeOrganizacional**. As propriedades fundamentais desta entidade são:

- **TenantId**: Id do locatário desta OU. Pode ser nulo para OUs do host.
- **ParentId**: Id da OU pai. Pode ser nulo se esta for uma OU raiz.
- **Código**: Um código de string hierárquico que é único para um locatário.
- **DisplayName**: Nome exibido da OU.

#### Árvore de Organização

Como uma OU pode ter um pai, todas as OUs de um locatário estão em uma estrutura de **árvore**. Existem algumas regras para esta árvore:

- Pode haver mais de uma raiz (onde o `ParentId` é `null`).
- Há um limite para a contagem de filhos de primeiro nível de uma OU (por causa do comprimento fixo da unidade de código OU explicado abaixo).

#### Código da OU

O código da OU é gerado automaticamente e mantido pelo serviço `GerenciadorUnidadeOrganizacional`. É uma string que se parece com isso:

"**00001.00042.00005**"

Este código pode ser usado para consultar facilmente o banco de dados para todos os filhos de uma OU (recursivamente). Existem algumas regras para este código (aplicadas automaticamente quando você usa o `GerenciadorUnidadeOrganizacional`):

- É **único** para um [locatário](../Multi-Tenancy.md).
- Todos os filhos da mesma OU têm códigos que **começam com o código da OU pai**.
- É de **comprimento fixo** e baseado no nível da OU na árvore, conforme mostrado no exemplo.
- Embora o código da OU seja único, ele pode ser **alterado** se você mover a OU relacionada.

Observe que você deve referenciar uma OU pelo Id, não pelo Código, porque o Código pode ser alterado posteriormente.

#### Gerenciador de Unidade Organizacional

A classe `GerenciadorUnidadeOrganizacional` pode ser [injetada](../Dependency-Injection.md) e usada para gerenciar OUs. Casos de uso comuns são:

- Criar, atualizar ou excluir uma OU
- Mover uma OU na árvore de OUs.
- Obter informações sobre a árvore de OUs e seus itens.

### Log de Segurança de Identidade

O sistema de log de segurança registra algumas operações ou alterações importantes em sua conta (como *login* e *alteração de senha*). Você também pode salvar o log de segurança, se necessário.

Você pode injetar e usar `GerenciadorLogSegurancaIdentidade` ou `IGerenciadorLogSeguranca` para gravar logs de segurança. Ele criará um objeto de log por padrão e preencherá alguns valores comuns, como `CreationTime`, `ClientIpAddress`, `BrowserInfo`, `usuário/locatário atual`, etc. Claro, você pode substituí-los.

```cs
await GerenciadorLogSegurancaIdentidade.SalvarAsync(new ContextoLogSegurancaIdentidade()
{
	Identidade = "IdentityServer",
	Ação = "AlterarSenha"
});
```

Configure `OpcoesLogSegurancaAbp` para fornecer o nome do aplicativo (no caso de você ter várias aplicações e desejar distinguir as aplicações nos logs) para o log ou desativar esse recurso.

```cs
Configure<OpcoesLogSegurancaAbp>(opcoes =>
{
	opcoes.NomeAplicativo = "AbpSecurityTest";
});
```

## Opções

`OpcoesIdentidade` é a classe de [opções](../Options.md) padrão fornecida pela biblioteca de [identidade](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) da Microsoft. Portanto, você pode definir essas opções no método `ConfigureServices` da sua classe de [módulo](../Module-Development-Basics.md).

**Exemplo: Definir o comprimento mínimo necessário das senhas**

````csharp
Configure<OpcoesIdentidade>(opcoes =>
{
    opcoes.Senha.ComprimentoMinimo = 5;
});
````

O ABP leva essas opções um passo adiante e permite que você as altere em tempo de execução usando o [sistema de configurações](../Settings.md). Você pode [injetar](../Dependency-Injection.md) `IGerenciadorConfiguracao` e usar um dos métodos `Set...` para alterar os valores das opções para um usuário, um locatário ou globalmente para todos os usuários.

**Exemplo: Alterar o comprimento mínimo necessário das senhas para o locatário atual**

````csharp
public class MeuServico : IDependencyTransient
{
    private readonly IGerenciadorConfiguracao _gerenciadorConfiguracao;

    public MeuServico(IGerenciadorConfiguracao gerenciadorConfiguracao)
    {
        _gerenciadorConfiguracao = gerenciadorConfiguracao;
    }

    public async Task AlterarComprimentoMinSenha(int comprimentoMin)
    {
        await _gerenciadorConfiguracao.DefinirParaLocatarioAtualAsync(
            NomesConfiguracaoIdentidade.Senha.ComprimentoMinimo,
            comprimentoMin.ToString()
        );
    }
}
````

A classe `NomesConfiguracaoIdentidade` (no namespace `Volo.Abp.Identity.Settings`) define constantes para os nomes das configurações.

## Eventos Distribuídos

Este módulo define os seguintes ETOs (Event Transfer Objects) para permitir que você se inscreva em alterações nas entidades do módulo;

* `UserEto` é publicado em alterações feitas em uma entidade `IdentityUser`.
* `IdentityRoleEto` é publicado em alterações feitas em uma entidade `IdentityRole`.
* `IdentityClaimTypeEto` é publicado em alterações feitas em uma entidade `IdentityClaimType`.
* `OrganizationUnitEto` é publicado em alterações feitas em uma entidade `OrganizationUnit`.

**Exemplo: Ser notificado quando um novo usuário for criado**

````csharp
public class MeuManipulador :
    IManipuladorEventoDistribuido<EntityCreatedEto<UserEto>>,
    IDependencyTransient
{
    public async Task ManipularEventoAsync(EntityCreatedEto<UserEto> evento)
    {
        UserEto user = evento.Entity;
        // TODO: ...
    }
}
````

`UserEto` e `IdentityRoleEto` são configurados para publicar automaticamente os eventos. Você deve configurar você mesmo para os outros. Consulte o documento de [Distributed Event Bus](../Distributed-Event-Bus.md) para aprender detalhes dos eventos pré-definidos.

> A inscrição nos eventos distribuídos é especialmente útil para cenários distribuídos (como arquitetura de microsserviços). Se você está construindo uma aplicação monolítica ou ouvindo eventos no mesmo processo que executa o Módulo de Identidade, então a inscrição nos [eventos locais](../Local-Event-Bus.md) pode ser mais eficiente e fácil.

## Internos

Esta seção abrange alguns detalhes internos do módulo que você não precisa muito, mas pode precisar usar em alguns casos.

### Camada de Domínio

#### Agregados

##### Usuário

Um usuário é geralmente uma pessoa que faz login e usa a aplicação.

* `IdentityUser` (raiz do agregado): Representa um usuário no sistema.
  * `IdentityUserRole` (coleção): Funções do usuário.
  * `IdentityUserClaim` (coleção): Reivindicações personalizadas do usuário.
  * `IdentityUserLogin` (coleção): Logins externos do usuário.
  * `IdentityUserToken` (coleção): Tokens do usuário (usados pelos serviços de identidade da Microsoft).

##### Função

Uma função é tipicamente um grupo de permissões para atribuir aos usuários.

* `IdentityRole` (raiz do agregado): Representa uma função no sistema.
  * `IdentityRoleClaim` (coleção): Reivindicações personalizadas da função.

##### Tipo de Reivindicação

Um tipo de reivindicação é uma definição de uma reivindicação personalizada que pode ser atribuída a outras entidades (como funções e usuários) no sistema.

* `IdentityClaimType` (raiz do agregado): Representa uma definição de tipo de reivindicação. Ele contém algumas propriedades (por exemplo, Obrigatório, Regex, Descrição, ValueType) para definir o tipo de reivindicação e as regras de validação.

##### Log de Segurança de Identidade

Um objeto `IdentitySecurityLog` representa uma operação relacionada à autenticação (como *login*) no sistema.

* `IdentitySecurityLog` (raiz do agregado): Representa um log de segurança no sistema.

##### Unidade Organizacional

Uma unidade organizacional é uma entidade em uma estrutura hierárquica.

* ```OrganizationUnit``` (raiz do agregado): Representa uma unidade organizacional no sistema.
  * ```Roles``` (coleção): Funções da unidade organizacional.

#### Repositórios

Os seguintes repositórios personalizados são definidos para este módulo:

* `IIdentityUserRepository`
* `IIdentityRoleRepository`
* `IIdentityClaimTypeRepository`
* ```IIdentitySecurityLogRepository```
* ```IOrganizationUnitRepository```

#### Serviços de Domínio

##### Gerenciador de Usuário

`IdentityUserManager` é usado para gerenciar usuários, suas funções, reivindicações, senhas, e-mails, etc. Ele é derivado da classe `UserManager<T>` da Microsoft Identity, onde `T` é `IdentityUser`.

##### Gerenciador de Função

`IdentityRoleManager` é usado para gerenciar funções e suas reivindicações. Ele é derivado da classe `RoleManager<T>` da Microsoft Identity, onde `T` é `IdentityRole`.

##### Gerenciador de Tipo de Reivindicação

`IdenityClaimTypeManager` é usado para realizar algumas operações para a raiz do agregado `IdentityClaimType`.

##### Gerenciador de Unidade Organizacional

```OrganizationUnitManager``` é usado para realizar algumas operações para a raiz do agregado ```OrganizationUnit```.

##### Gerenciador de Log de Segurança

```IdentitySecurityLogManager``` é usado para salvar logs de segurança.

### Camada de Aplicação

#### Serviços de Aplicação

* `IdentityUserAppService` (implementa `IIdentityUserAppService`): Implementa os casos de uso da interface do usuário de gerenciamento de usuários.
* `IdentityRoleAppService` (implementa `IIdentityRoleAppService`): Implementa os casos de uso da interface do usuário de gerenciamento de funções.
* `IdentityClaimTypeAppService` (implementa `IIdentityClaimTypeAppService`): Implementa os casos de uso da interface do usuário de gerenciamento de tipos de reivindicação.
* `IdentitySettingsAppService` (implementa `IIdentitySettingsAppService`): Usado para obter e atualizar configurações para o módulo de identidade.
* `IdentityUserLookupAppService` (implementa `IIdentityUserLookupAppService`): Usado para obter informações de um usuário por `id` ou `userName`. É destinado a ser usado internamente pelo framework ABP.
* `ProfileAppService` (implementa `IProfileAppService`): Usado para alterar o perfil de um usuário e a senha.
* ```IdentitySecurityLogAppService``` (implementa ```IIdentitySecurityLogAppService```): Implementa os casos de uso da interface do usuário de logs de segurança.
* ```OrganizationUnitAppService``` (implementa ```OrganizationUnitAppService```): Implementa os casos de uso da interface do usuário de gerenciamento de unidades organizacionais.

### Provedores de Banco de Dados

Este módulo fornece opções de [Entity Framework Core](../Entity-Framework-Core.md) e [MongoDB](../MongoDB.md) para o banco de dados.

#### Entity Framework Core

O pacote NuGet [Volo.Abp.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.Identity.EntityFrameworkCore) implementa a integração do EF Core.

##### Tabelas do Banco de Dados

* **AbpRoles**
  * AbpRoleClaims
* **AbpUsers**
  * AbpUserClaims
  * AbpUserLogins
  * AbpUserRoles
  * AbpUserTokens
* **AbpClaimTypes**
* **AbpOrganizationUnits**
  * AbpOrganizationUnitRoles
  * AbpUserOrganizationUnits
* **AbpSecurityLogs**

#### MongoDB

O pacote NuGet [Volo.Abp.Identity.MongoDB](https://www.nuget.org/packages/Volo.Abp.Identity.MongoDB) implementa a integração do MongoDB.

##### Coleções do Banco de Dados

* **AbpRoles**
* **AbpUsers**
* **AbpClaimTypes**
* **AbpOrganizationUnits**
* **AbpSecurityLogs**

#### Propriedades Comuns do Banco de Dados

Você pode definir as seguintes propriedades da classe `AbpIdentityDbProperties` para alterar as opções do banco de dados:

* `DbTablePrefix` (`Abp` por padrão) é o prefixo para os nomes das tabelas/coleções.
* `DbSchema` (`null` por padrão) é o esquema do banco de dados.
* `ConnectionStringName` (`AbpIdentity` por padrão) é o nome da [string de conexão](../Connection-Strings.md) para este módulo.

Essas são propriedades estáticas. Se você quiser definir, faça isso no início de sua aplicação (normalmente, em `Program.cs`).