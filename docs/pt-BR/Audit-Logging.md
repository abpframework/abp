# Registro de Auditoria

[Wikipedia](https://en.wikipedia.org/wiki/Audit_trail): "*Um rastro de auditoria (também chamado de **log de auditoria**) é um registro cronológico relevante para a segurança, conjunto de registros e/ou destino e origem de registros que fornecem evidências documentais da sequência de atividades que afetaram a qualquer momento uma operação, procedimento ou evento específico*".

O ABP Framework fornece um **sistema de registro de auditoria extensível** que automatiza o registro de auditoria por **convenção** e fornece **pontos de configuração** para controlar o nível dos logs de auditoria.

Um **objeto de log de auditoria** (consulte a seção Objeto de Log de Auditoria abaixo) é tipicamente criado e salvo por solicitação da web. Ele inclui;

* Detalhes da **solicitação e resposta** (como URL, método Http, informações do navegador, código de status HTTP... etc.).
* **Ações realizadas** (ações do controlador e chamadas de métodos de serviço de aplicação com seus parâmetros).
* **Mudanças de entidade** ocorridas na solicitação da web.
* Informações de **exceção** (se houve um erro durante a execução da solicitação).
* **Duração da solicitação** (para medir o desempenho da aplicação).

> Os [modelos de inicialização](Startup-Templates/Index.md) são configurados para o sistema de registro de auditoria, o que é adequado para a maioria das aplicações. Use este documento para um controle detalhado sobre o sistema de log de auditoria.

### Suporte do Provedor de Banco de Dados

* Totalmente suportado pelo provedor [Entity Framework Core](Entity-Framework-Core.md).
* O log de alterações de entidade não é suportado pelo provedor [MongoDB](MongoDB.md). Outros recursos funcionam conforme o esperado.

## UseAuditing()

O middleware `UseAuditing()` deve ser adicionado ao pipeline de solicitações do ASP.NET Core para criar e salvar os logs de auditoria. Se você criou suas aplicações usando [os modelos de inicialização](Startup-Templates/Index.md), ele já está adicionado.

## AbpAuditingOptions

`AbpAuditingOptions` é o principal [objeto de opções](Options.md) para configurar o sistema de log de auditoria. Você pode configurá-lo no método `ConfigureServices` do seu [módulo](Module-Development-Basics.md):

````csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IsEnabled = false; //Desativa o sistema de auditoria
});
````

Aqui, uma lista das opções que você pode configurar:

* `IsEnabled` (padrão: `true`): Uma chave raiz para habilitar ou desabilitar o sistema de auditoria. Outras opções não são usadas se esse valor for `false`.
* `HideErrors` (padrão: `true`): O sistema de log de auditoria oculta e escreve [logs](Logging.md) regulares se ocorrer algum erro ao salvar os objetos de log de auditoria. Se salvar os logs de auditoria for crítico para o seu sistema, defina isso como `false` para lançar uma exceção em caso de ocultação de erros.
* `IsEnabledForAnonymousUsers` (padrão: `true`): Se você deseja escrever logs de auditoria apenas para os usuários autenticados, defina isso como `false`. Se você salvar logs de auditoria para usuários anônimos, verá `null` para os valores de `UserId` desses usuários.
* `AlwaysLogOnException` (padrão: `true`): Se definido como verdadeiro, sempre salva o log de auditoria em caso de exceção/erro sem verificar outras opções (exceto `IsEnabled`, que desativa completamente o registro de auditoria).
* `IsEnabledForIntegrationService` (padrão: `false`): O Registro de Auditoria é desativado para [serviços de integração](Integration-Services.md) por padrão. Defina essa propriedade como `true` para habilitá-la.
* `IsEnabledForGetRequests` (padrão: `false`): As solicitações HTTP GET normalmente não devem fazer nenhuma alteração no banco de dados e o sistema de log de auditoria não salva objetos de log de auditoria para solicitações GET. Defina isso como `true` para habilitá-lo também para as solicitações GET.
* `DisableLogActionInfo` (padrão: `false`): Se definido como verdadeiro, não registrará mais `AuditLogActionInfo`.
* `ApplicationName`: Se várias aplicações estiverem salvando logs de auditoria em um único banco de dados, defina essa propriedade com o nome da sua aplicação, para que você possa distinguir os logs de diferentes aplicações. Se você não definir, ele será definido a partir do valor `IApplicationInfoAccessor.ApplicationName`, que é o nome da assembly de entrada por padrão.
* `IgnoredTypes`: Uma lista de `Type`s a serem ignorados para o registro de auditoria. Se for um tipo de entidade, as alterações para esse tipo de entidades não serão salvas. Esta lista também é usada ao serializar os parâmetros de ação.
* `EntityHistorySelectors`: Uma lista de seletores usados para determinar se um tipo de entidade é selecionado para salvar a alteração da entidade. Consulte a seção abaixo para detalhes.
* `SaveEntityHistoryWhenNavigationChanges` (padrão: `true`): Se definido como verdadeiro, salvará as alterações da entidade no log de auditoria quando houver alterações em propriedades de navegação.
* `Contributors`: Uma lista de implementações de `AuditLogContributor`. Um contribuidor é uma forma de estender o sistema de log de auditoria. Consulte a seção "Contribuidores de Log de Auditoria" abaixo.
* `AlwaysLogSelectors`: Uma lista de seletores para salvar os logs de auditoria para os critérios correspondentes.

### Seletores de Histórico de Entidade

Salvar todas as alterações de todas as suas entidades exigiria muito espaço no banco de dados. Por esse motivo, **o sistema de log de auditoria não salva nenhuma alteração para as entidades a menos que você configure explicitamente**.

Para salvar todas as alterações de todas as entidades, simplesmente use o método de extensão `AddAllEntities()`.

````csharp
Configure<AbpAuditingOptions>(options =>
{
    options.EntityHistorySelectors.AddAllEntities();
});
````

`options.EntityHistorySelectors` na verdade é uma lista de predicados de tipo. Você pode escrever uma expressão lambda para definir seu filtro.

O seletor de exemplo abaixo faz o mesmo do método de extensão `AddAllEntities()` definido acima:

````csharp
Configure<AbpAuditingOptions>(options =>
{
    options.EntityHistorySelectors.Add(
        new NamedTypeSelector(
            "MeuSeletorNome",
            type =>
            {
                if (typeof(IEntity).IsAssignableFrom(type))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        )
    );
});
````

A condição `typeof(IEntity).IsAssignableFrom(type)` será `true` para qualquer classe que implemente a interface `IEntity` (tecnicamente, todas as entidades em sua aplicação). Você pode verificar condicionalmente e retornar `true` ou `false` com base em sua preferência.

`options.EntityHistorySelectors` é uma forma flexível e dinâmica de selecionar as entidades para o registro de auditoria. Outra forma é usar os atributos `Audited` e `DisableAuditing` por entidade.

## AbpAspNetCoreAuditingOptions

`AbpAspNetCoreAuditingOptions` é o [objeto de opções](Options.md) para configurar o registro de auditoria na camada ASP.NET Core. Você pode configurá-lo no método `ConfigureServices` do seu [módulo](Module-Development-Basics.md):

````csharp
Configure<AbpAspNetCoreAuditingOptions>(options =>
{
    options.IgnoredUrls.Add("/produtos");
});
````

`IgnoredUrls` é a única opção. É uma lista de prefixos de URLs ignorados. No exemplo anterior, todas as URLs que começam com `/produtos` serão ignoradas para o registro de auditoria.

## Habilitando/Desabilitando o Registro de Auditoria para Serviços

### Habilitar/Desabilitar para Controladores e Ações

Todas as ações do controlador são registradas por padrão (consulte `IsEnabledForGetRequests` acima para solicitações GET).

Você pode usar o `[DisableAuditing]` para desativá-lo para um tipo de controlador específico:

````csharp
[DisableAuditing]
public class HomeController : AbpController
{
    //...
}
````

Use `[DisableAuditing]` para qualquer ação para controlá-la no nível da ação:

````csharp
public class HomeController : AbpController
{
    [DisableAuditing]
    public async Task<ActionResult> Home()
    {
        //...
    }

    public async Task<ActionResult> OutraAcaoRegistrada()
    {
        //...
    }
}
````

### Habilitar/Desabilitar para Serviços de Aplicação e Métodos

As chamadas de métodos de [serviço de aplicação](Application-Services.md) também são incluídas no log de auditoria por padrão. Você pode usar o `[DisableAuditing]` no nível do serviço ou do método.

#### Habilitar/Desabilitar para Outros Serviços

O registro de auditoria de ação pode ser habilitado para qualquer tipo de classe (registrado e resolvido da [injeção de dependência](Dependency-Injection.md)) enquanto é habilitado apenas para os controladores e os serviços de aplicação por padrão.

Use `[Audited]` e `[DisableAuditing]` para qualquer classe ou método que precisa ser registrado no log de auditoria. Além disso, sua classe pode (direta ou implicitamente) implementar a interface `IAuditingEnabled` para habilitar o registro de auditoria para essa classe por padrão.

### Habilitar/Desabilitar para Entidades e Propriedades

Uma entidade é ignorada no registro de alteração de entidade nos seguintes casos;

* Se você adicionar um tipo de entidade às `AbpAuditingOptions.IgnoredTypes` (como explicado anteriormente), ele é completamente ignorado no sistema de registro de auditoria.
* Se o objeto não for uma [entidade](Entities.md) (não implementa `IEntity` diretamente ou implicitamente - Todas as entidades implementam essa interface por padrão).
* Se o tipo de entidade não for público.

Caso contrário, você pode usar `Audited` para habilitar o registro de alteração de entidade para uma entidade:

````csharp
[Audited]
public class MinhaEntidade : Entity<Guid>
{
    //...
}
````

Ou desativá-lo para uma entidade:

````csharp
[DisableAuditing]
public class MinhaEntidade : Entity<Guid>
{
    //...
}
````

Desativar o registro de auditoria pode ser necessário apenas se a entidade estiver sendo selecionada pelos `AbpAuditingOptions.EntityHistorySelectors` que foram explicados anteriormente.

Você pode desativar o registro de auditoria apenas para algumas propriedades de suas entidades para um controle detalhado sobre o registro de auditoria:

````csharp
[Audited]
public class MeuUsuario : Entity<Guid>
{
    public string Nome { get; set; }
        
    public string Email { get; set; }

    [DisableAuditing] //Ignora a Senha no registro de auditoria
    public string Senha { get; set; }
}
````

O sistema de log de auditoria salvará as alterações para a entidade `MeuUsuario` enquanto ignora a propriedade `Senha`, que pode ser perigosa de salvar por motivos de segurança.

Em alguns casos, você pode querer salvar apenas algumas propriedades de suas entidades e ignorar todas as outras. Escrever `[DisableAuditing]` para todas as outras propriedades seria tedioso. Em tais casos, use `[Audited]` apenas para as propriedades desejadas e marque a entidade com o atributo `[DisableAuditing]`:

````csharp
[DisableAuditing]
public class MeuUsuario : Entity<Guid>
{
    [Audited] //Apenas registra a alteração do Nome
    public string Nome { get; set; }

    public string Email { get; set; }

    public string Senha { get; set; }
}
````
## IAuditingStore

`IAuditingStore` é uma interface usada para salvar os objetos de log de auditoria (explicados abaixo) pelo Framework ABP. Se você precisa salvar os objetos de log de auditoria em um armazenamento de dados personalizado, pode implementar o `IAuditingStore` em sua própria aplicação e substituir usando o [sistema de injeção de dependência](Dependency-Injection.md).

`SimpleLogAuditingStore` é usado se nenhum armazenamento de auditoria estiver registrado. Ele simplesmente escreve o objeto de auditoria no sistema padrão de [logging](Logging.md).

[O Módulo de Registro de Auditoria](Modules/Audit-Logging.md) foi configurado nos [modelos de inicialização](Startup-Templates/Index.md) para salvar objetos de log de auditoria em um banco de dados (ele suporta vários provedores de banco de dados). Portanto, na maioria das vezes, você não precisa se preocupar com como o `IAuditingStore` foi implementado e usado.

## Objeto de Log de Auditoria

Um **objeto de log de auditoria** é criado para cada **solicitação web** por padrão. Um objeto de log de auditoria pode ser representado pelo seguinte diagrama de relação:

![**auditlog-object-diagram**](images/auditlog-object-diagram.png)

* **AuditLogInfo**: O objeto raiz com as seguintes propriedades:
  * `ApplicationName`: Quando você salva logs de auditoria de diferentes aplicações no mesmo banco de dados, essa propriedade é usada para distinguir os logs das aplicações.
  * `UserId`: Id do usuário atual, se o usuário estiver logado.
  * `UserName`: Nome do usuário atual, se o usuário estiver logado (esse valor está aqui para não depender do módulo/sistema de identidade para pesquisa).
  * `TenantId`: Id do locatário atual, para uma aplicação multi-locatário.
  * `TenantName`: Nome do locatário atual, para uma aplicação multi-locatário.
  * `ExecutionTime`: O momento em que este objeto de log de auditoria foi criado.
  * `ExecutionDuration`: Duração total da execução da solicitação, em milissegundos. Isso pode ser usado para observar o desempenho da aplicação.
  * `ClientId`: Id do cliente atual, se o cliente estiver autenticado. Um cliente é geralmente uma aplicação de terceiros que usa o sistema por meio de uma API HTTP.
  * `ClientName`: Nome do cliente atual, se disponível.
  * `ClientIpAddress`: Endereço IP do cliente/dispositivo do usuário.
  * `CorrelationId`: Id de [Correlação Atual](CorrelationId.md). O Id de correlação é usado para relacionar os logs de auditoria escritos por diferentes aplicações (ou microsserviços) em uma única operação lógica.
  * `BrowserInfo`: Informações do nome/versão do navegador do usuário atual, se disponível.
  * `HttpMethod`: Método HTTP da solicitação atual (GET, POST, PUT, DELETE... etc.).
  * `HttpStatusCode`: Código de status da resposta HTTP para esta solicitação.
  * `Url`: URL da solicitação.
* **AuditLogActionInfo**: Um log de auditoria de ação é tipicamente uma ação de controlador ou uma chamada de método de [serviço de aplicação](Application-Services.md) durante a solicitação web. Um log de ação pode conter várias ações. Um objeto de ação tem as seguintes propriedades:
  * `ServiceName`: Nome do controlador/serviço executado.
  * `MethodName`: Nome do método executado do controlador/serviço.
  * `Parameters`: Um texto formatado em JSON representando os parâmetros passados para o método.
  * `ExecutionTime`: O momento em que este método foi executado.
  * `ExecutionDuration`: Duração da execução do método, em milissegundos. Isso pode ser usado para observar o desempenho do método.
* **EntityChangeInfo**: Representa uma alteração de uma entidade nesta solicitação web. Um log de auditoria pode conter zero ou mais alterações de entidade. Uma alteração de entidade tem as seguintes propriedades:
  * `ChangeTime`: O momento em que a entidade foi alterada.
  * `ChangeType`: Um enum com os seguintes campos: `Criado` (0), `Atualizado` (1) e `Excluído` (2).
  * `EntityId`: Id da entidade que foi alterada.
  * `EntityTenantId`: Id do locatário a que esta entidade pertence.
  * `EntityTypeFullName`: Nome do tipo (classe) da entidade com namespace completo (como *Acme.BookStore.Book* para a entidade Book).
* **EntityPropertyChangeInfo**: Representa uma alteração de uma propriedade de uma entidade. Uma informação de alteração de entidade (explicada acima) pode conter uma ou mais alterações de propriedade com as seguintes propriedades:
  * `NewValue`: Novo valor da propriedade. É `null` se a entidade foi excluída.
  * `OriginalValue`: Valor antigo/original antes da alteração. É `null` se a entidade foi recém-criada.
  * `PropertyName`: O nome da propriedade na classe da entidade.
  * `PropertyTypeFullName`: Nome do tipo (classe) da propriedade com namespace completo.
* **Exception**: Um objeto de log de auditoria pode conter zero ou mais exceções. Dessa forma, você pode obter um relatório das solicitações com falha.
* **Comment**: Um valor de string arbitrário para adicionar mensagens personalizadas à entrada de log de auditoria. Um objeto de log de auditoria pode conter zero ou mais comentários.

Além das propriedades padrão explicadas acima, os objetos `AuditLogInfo`, `AuditLogActionInfo` e `EntityChangeInfo` implementam a interface `IHasExtraProperties`, para que você possa adicionar propriedades personalizadas a esses objetos.

## Contribuidores de Log de Auditoria

Você pode estender o sistema de auditoria criando uma classe derivada da classe `AuditLogContributor`, que define os métodos `PreContribute` e `PostContribute`.

O único contribuidor pré-construído é a classe `AspNetCoreAuditLogContributor`, que define as propriedades relacionadas a uma solicitação HTTP.

Um contribuidor pode definir propriedades e coleções da classe `AuditLogInfo` para adicionar mais informações.

Exemplo:

````csharp
public class MyAuditLogContributor : AuditLogContributor
{
    public override void PreContribute(AuditLogContributionContext context)
    {
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        context.AuditInfo.SetProperty(
            "MyCustomClaimValue",
            currentUser.FindClaimValue("MyCustomClaim")
        );
    }

    public override void PostContribute(AuditLogContributionContext context)
    {
        context.AuditInfo.Comments.Add("Algum comentário...");
    }
}
````

* `context.ServiceProvider` pode ser usado para resolver serviços da [injeção de dependência](Dependency-Injection.md).
* `context.AuditInfo` pode ser usado para acessar o objeto de log de auditoria atual para manipulá-lo.

Após criar um contribuidor, você deve adicioná-lo à lista `AbpAuditingOptions.Contributors`:

````csharp
Configure<AbpAuditingOptions>(options =>
{
    options.Contributors.Add(new MyAuditLogContributor());
});
````

## IAuditLogScope & IAuditingManager

Esta seção explica os serviços `IAuditLogScope` e `IAuditingManager` para casos de uso avançados.

Um **escopo de log de auditoria** é um [escopo ambiente](Ambient-Context-Pattern.md) que **constrói** e **salva** um objeto de log de auditoria (explicado anteriormente). Por padrão, um escopo de log de auditoria é criado para uma solicitação web pelo Middleware de Log de Auditoria (veja a seção `UseAuditing()` acima).

### Acesso ao Escopo Atual de Log de Auditoria

Os contribuidores de log de auditoria, explicados acima, são uma maneira global de manipular o objeto de log de auditoria. É bom se você puder obter um valor de um serviço.

Se você precisar manipular o objeto de log de auditoria em um ponto arbitrário de sua aplicação, pode acessar o escopo de log de auditoria atual e obter o objeto de log de auditoria atual (independente de como o escopo é gerenciado). Exemplo:

````csharp
public class MeuServico : ITransientDependency
{
    private readonly IAuditingManager _auditingManager;

    public MeuServico(IAuditingManager auditingManager)
    {
        _auditingManager = auditingManager;
    }

    public async Task FazerIssoAsync()
    {
        var escopoAtualDeLogDeAuditoria = _auditingManager.Current;
        if (escopoAtualDeLogDeAuditoria != null)
        {
            escopoAtualDeLogDeAuditoria.Log.Comments.Add(
                "Executou o método MeuServico.FazerIssoAsync :)"
            );

            escopoAtualDeLogDeAuditoria.Log.SetProperty("MinhaPropriedadePersonalizada", 42);
        }
    }
}
````

Sempre verifique se `_auditingManager.Current` é nulo ou não, porque é controlado em um escopo externo e você não pode saber se um escopo de log de auditoria foi criado antes de chamar seu método.

### Criar Manualmente um Escopo de Log de Auditoria

Raramente você precisa criar manualmente um escopo de log de auditoria, mas se precisar, pode criar um escopo de log de auditoria usando o `IAuditingManager` como no exemplo a seguir:

````csharp
public class MeuServico : ITransientDependency
{
    private readonly IAuditingManager _auditingManager;

    public MeuServico(IAuditingManager auditingManager)
    {
        _auditingManager = auditingManager;
    }

    public async Task FazerIssoAsync()
    {
        using (var escopoDeAuditoria = _auditingManager.BeginScope())
        {
            try
            {
                //Chame outros serviços...
            }
            catch (Exception ex)
            {
                //Adicione exceções
                _auditingManager.Current.Log.Exceptions.Add(ex);
                throw;
            }
            finally
            {
                //Sempre salve o log
                await escopoDeAuditoria.SaveAsync();
            }
        }
    }
}
````

Você pode chamar outros serviços, que podem chamar outros, que podem alterar entidades e assim por diante. Todas essas interações são salvas como um único objeto de log de auditoria no bloco finally.

## O Módulo de Registro de Auditoria

O Módulo de Registro de Auditoria basicamente implementa o `IAuditingStore` para salvar os objetos de log de auditoria em um banco de dados. Ele suporta vários provedores de banco de dados. Este módulo é adicionado aos modelos de inicialização por padrão.

Consulte o documento [Módulo de Registro de Auditoria](Modules/Audit-Logging.md) para mais informações.
